using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Editarrr.Level;
using Proyecto26;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level.Storage
{
    public class RemoteLevelStorageProviderAws : IRemoteLevelStorageProvider
    {
        public const string AwsLevelUrl = "https://tlfb41owe5.execute-api.eu-north-1.amazonaws.com";
        private const string AwsScreenshotUrl = "https://editarrr-screenshots.s3.eu-north-1.amazonaws.com";
        private const bool ShowDebug = false;

        public void Initialize()
        {
            // No initialization needed.
        }

        public void Upload(LevelSave levelSave, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            var userId = PreferencesManager.Instance.GetUserId();

            var tileData = new AwsTileData()
            {
                tiles = levelSave.Tiles
            };
            var request = new AwsLevel
            {
                id = levelSave.RemoteId,
                name = levelSave.Code,
                creator = new AwsCreator()
                {
                    name = levelSave.CreatorName,
                    id = userId
                },
                status = levelSave.Published ? "PUBLISHED" : "DRAFT",
                version = levelSave.Version,
                data = new AwsLevelData()
                {
                    scaleX = levelSave.ScaleX,
                    scaleY = levelSave.ScaleY,
                    tiles = JsonUtility.ToJson(tileData),
                }
            };

            RestClient.Get<AwsLevel>($"{AwsLevelUrl}/dev/levels/{levelSave.RemoteId}").Then(res =>
            {
                Debug.Log("UPLOAD - Existing level found for ." + res.name);

                // Existing level found.
                this.Update(request, callback);
            }).Catch(_ =>
            {
                Debug.Log("UPLOAD - No level found for ." + request.name);
                // No level found.
                this.Insert(request, callback);
            });
        }

        private void Insert(AwsLevel request, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            RestClient.Post<AwsUploadResponse>($"{AwsLevelUrl}/dev/levels", JsonUtility.ToJson(request)).Then(res =>
            {
                callback?.Invoke(request.name, res.id);
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));

                UploadScreenshot(request.name);
            }).Catch(err => { this.LogMessage("Error", err.Message); });
        }

        private void Update(AwsLevel request, RemoteLevelStorage_LevelUploadedCallback callback)
        {
            RestClient.Patch<AwsUploadResponse>($"{AwsLevelUrl}/dev/levels/{request.id}", JsonUtility.ToJson(request))
                .Then(res =>
                {
                    callback?.Invoke(request.name, res.id.ToString());
                    this.LogMessage("Levels", JsonUtility.ToJson(res, true));

                    UploadScreenshot(request.name);
                }).Catch(err => { this.LogMessage("Error", err.Message); });
        }

        public void Download(string code, RemoteLevelStorage_LevelLoadedCallback callback)
        {
            RestClient.Get<AwsLevel>($"{AwsLevelUrl}/dev/levels/{code}").Then(res =>
            {
                // @todo make sure we store the remote level id in the save.
                var save = new LevelSave(res.creator.id, res.creator.name, res.name);

                save.SetRemoteId(res.id);

                var tiles = JsonUtility.FromJson<AwsTileData>(res.data.tiles);
                var tileStates = new TileState[res.data.scaleX, res.data.scaleY];

                foreach (var tileSave in tiles.tiles)
                {
                    tileStates[tileSave.X, tileSave.Y] = new TileState(tileSave);
                }

                save.SetTiles(tileStates);
                save.SetVersion(res.version);
                save.SetPublished(res.status == "PUBLISHED");
                callback?.Invoke(save);

                this.DoDownloadScreenshot(res.name);

                // @todo return level data.
                this.LogMessage(res.id, JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                callback?.Invoke(null);
                this.LogMessage("Error", err.Message);
            });
        }

        public void DownloadScreenshot(string code, RemoteLevelStorage_LevelScreenshotDownloadedCallback callback)
        {
            this.DoDownloadScreenshot(code, callback);
        }

        private async void UploadScreenshot(string code)
        {
#if UNITY_WEBGL
            // Webgl needs a custom solution for uploading image files.
            return;
#endif
            var uploadUrl = $"{AwsLevelUrl}/dev/screenshot/{code}.png";
            var imagePath = LocalLevelStorageManager.LocalRootDirectory + code + "/screenshot.png";
            using (var httpClient = new HttpClient())
            {
                // Load the image data from the file path
                var imageBytes = await File.ReadAllBytesAsync(imagePath);

                // Create a ByteArrayContent from the image data
                var imageContent = new ByteArrayContent(imageBytes);

                // Send the POST request
                var response = await httpClient.PostAsync(uploadUrl, imageContent);
                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Image uploaded successfully");
                }
                else
                {
                    Debug.LogError("Error uploading image. Status code: " + response.StatusCode);
                }
            }
        }

        private async void DoDownloadScreenshot(string code, RemoteLevelStorage_LevelScreenshotDownloadedCallback callback = null)
        {
            // @todo Move the storage to the LocalLevelStorageManager.
            var url = $"{AwsScreenshotUrl}/{code}.png";
            using (var httpClient = new HttpClient())
            {
                // Send an HTTP GET request to download the image
                var response = await httpClient.GetAsync(url);
                var saveDirectory = LocalLevelStorageManager.LocalRootDirectory + code;

                if (response.IsSuccessStatusCode)
                {
                    // Ensure the save directory exists
                    Directory.CreateDirectory(saveDirectory);

                    // Get the filename from the URL
                    var fileName = "screenshot.png";

                    // Combine the save directory and filename to get the full path
                    var fullPath = Path.Combine(saveDirectory, fileName);

                    // Read the image content and save it to the specified directory
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(fullPath, imageBytes);

                    Debug.Log("Image downloaded and saved to: " + fullPath);
                    callback?.Invoke();
                }
                else
                {
                    Debug.LogError("Error downloading image. Status code: " + response.StatusCode);
                }
            }
        }

        public void LoadAllLevelData(RemoteLevelStorage_AllLevelsLoadedCallback callback)
        {
            // Get request to /levels
            RestClient.Get<AwsLevels>($"{AwsLevelUrl}/dev/levels").Then(res =>
            {
                var levelStubs = new List<LevelStub>();
                foreach (var level in res.levels)
                {
                    var levelStub = new LevelStub(level.name, level.creator.id, level.creator.name, level.id,
                        level.status == "PUBLISHED");
                    levelStubs.Add(levelStub);
                }

                callback?.Invoke(levelStubs.ToArray());
                this.LogMessage("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                callback?.Invoke(null);
                this.LogMessage("Error", err.Message);
            });
        }

        public bool SupportsLeaderboards()
        {
            return true;
        }

        public void SubmitScore(float score, LevelSave levelSave, RemoteScoreStorage_ScoreSubmittedCallback callback)
        {
            string userId = PreferencesManager.Instance.GetUserId();
            string userName = PreferencesManager.Instance.GetUserName();

            var request = new AwsScoreRequest
            {
                code = levelSave.Code,
                score = score.ToString(),
                creator = userId,
                creatorName = userName
            };

            RestClient.Post<AwsUploadResponse>($"{AwsLevelUrl}/dev/levels/{levelSave.RemoteId}/scores", JsonUtility.ToJson(request)).Then(res =>
            {
                Debug.Log("Score uploaded for level: " + levelSave.Code);
                callback?.Invoke(levelSave.Code, res.id);
            }).Catch(err => { this.LogMessage("Error", err.Message); });
        }

        public void GetScoresForLevel(string code, RemoteScoreStorage_AllScoresLoadedCallback callback)
        {
            // Get request to /levels/{id}/scores
            RestClient.Get<AwsScores>($"{AwsLevelUrl}/dev/levels/{code}/scores").Then(res =>
            {
                var scoreStubs = new List<ScoreStub>();
                foreach (var score in res.scores)
                {
                    float scoreValue = float.Parse(score.score);
                    var levelStub = new ScoreStub(score.code, score.creator.id, score.creator.name, scoreValue);
                    scoreStubs.Add(levelStub);
                }

                callback?.Invoke(scoreStubs.ToArray());
                this.LogMessage("Scores", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                callback?.Invoke(null);
                this.LogMessage("Error", err.Message);
            });
        }

        public void SubmitRating(int rating, LevelSave levelSave, RemoteRatingStorage_RatingSubmittedCallback callback)
        {
            string userId = PreferencesManager.Instance.GetUserId();
            string userName = PreferencesManager.Instance.GetUserName();

            var request = new AwsRatingRequest
            {
                code = levelSave.Code,
                rating = rating,
                creator = userId,
                creatorName = userName
            };

            RestClient.Post<AwsUploadResponse>($"{AwsLevelUrl}/dev/levels/{levelSave.RemoteId}/ratings", JsonUtility.ToJson(request)).Then(res =>
            {
                Debug.Log("Rating uploaded for level: " + levelSave.Code);
                callback?.Invoke(levelSave.Code, res.id);
            }).Catch(err => { this.LogMessage("Error", err.Message); });
        }

        public void GetRatingsForLevel(string code, RemoteRatingStorage_AllRatingsLoadedCallback callback)
        {
            // Get request to /levels/{id}/ratings
            RestClient.Get<AwsRatings>($"{AwsLevelUrl}/dev/levels/{code}/ratings").Then(res =>
            {
                var ratingStubs = new List<RatingStub>();
                foreach (var rating in res.ratings)
                {
                    var levelStub = new RatingStub(rating.code, rating.creator.id, rating.creator.name, rating.rating);
                    ratingStubs.Add(levelStub);
                }

                callback?.Invoke(ratingStubs.ToArray());
                this.LogMessage("Ratings", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                callback?.Invoke(null);
                this.LogMessage("Error", err.Message);
            });
        }


        private void LogMessage(string title, string message)
        {
#if UNITY_EDITOR
            if (ShowDebug)
            {
                EditorUtility.DisplayDialog(title, message, "Ok");
            }
            else
            {
                Debug.Log(message);
            }
#else
		    Debug.Log(message);
#endif
        }
    }

    #region AWS Level Data Structures

    [Serializable]
    public class AwsLevels
    {
        public AwsLevel[] levels;
    }

    [Serializable]
    public class AwsLevel
    {
        public string id;
        public string name;
        public AwsCreator creator;
        public string status;
        public uint createdAt;
        public uint updatedAt;
        public int version;
        public AwsLevelData data;
    }

    [Serializable]
    public class AwsCreator
    {
        public string id;
        public string name;
    }

    [Serializable]
    public class AwsLevelData
    {
        public int scaleX;
        public int scaleY;
        public string tiles;
    }

    [Serializable]
    public class AwsTileData
    {
        public TileSave[] tiles;
    }

    [Serializable]
    public class AwsUploadResponse
    {
        public string message;

        // @todo update the web interface.
        public string id;
        public string data;
    }

    #endregion

    #region AWS Score Data Structures

    [Serializable]
    public class AwsScores
    {
        public AwsScore[] scores;
    }

    public class AwsScoreRequest
    {
        public string code;
        public string score;
        public string creator;
        public string creatorName;
    }

    [Serializable]
    public class AwsScore
    {
        public string scoreId;
        public string levelId;
        public string score;
        public string code;
        public AwsCreator creator;
        public uint submittedAt;
    }

    #endregion


    #region AWS Rating Data Structures

    [Serializable]
    public class AwsRatings
    {
        public AwsRating[] ratings;
    }

    public class AwsRatingRequest
    {
        public string code;
        public int rating;
        public string creator;
        public string creatorName;
    }

    [Serializable]
    public class AwsRating
    {
        public string scoreId;
        public string levelId;
        public int rating;
        public string code;
        public AwsCreator creator;
        public uint submittedAt;
    }

    #endregion
}
