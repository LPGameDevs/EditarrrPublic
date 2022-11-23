using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Proyecto26;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelEditor
{
    public class DrupalConnector : MonoBehaviour, IDbConnector
    {
        private const string codeChars = "bcdfghjklmnpqrstvwxyz123456789";

#if UNITY_EDITOR
        private const string drupalBaseUrl = "https://tower-offender.ddev.site";
        private const string drupalLevelUrl = "https://tower-offender.ddev.site/jsonapi/node/level";
        private const string drupalCommentUrl = "https://tower-offender.ddev.site/jsonapi/comment/comment";
        private const string drupalFileUrl = "https://tower-offender.ddev.site/jsonapi/file/file";
#else
        private const string drupalBaseUrl = "https://editarrr.trygamedev.com";
        private const string drupalLevelUrl = "https://editarrr.trygamedev.com/jsonapi/node/level";
        private const string drupalCommentUrl = "https://editarrr.trygamedev.com/jsonapi/comment/comment";
        private const string drupalFileUrl = "https://editarrr.trygamedev.com/jsonapi/file/file";
#endif
        private const string token = "YXBpOktabmJmVTFhYmwwYW9lYzZBNndpOFJ2QWZTa1BzVTZYS3IwMHNkR1ZBQmFDcjZUM1BndmN0WFo4WDRVNElIc3Q=";

        private bool _showDebug = false;

        private void Awake()
        {
            RestClient.DefaultRequestHeaders["Authorization"] = "Basic " + token;
            RestClient.DefaultRequestHeaders["Accept"] = "application/vnd.api+json";
            RestClient.DefaultRequestHeaders["Content-Type"] = "application/vnd.api+json";
        }


        public void UploadImage(string uuid, string code)
        {
#if UNITY_WEBGL
            // Dont upload images in WebGL.
            Debug.Log("Skipping image upload in WebGL");
            return;
#endif
            WebRequest request = WebRequest.Create($"{drupalLevelUrl}/{uuid}/field_screenshot");
            request.Method = "POST";
            byte[] byteArray = File.ReadAllBytes($"{EditorLevelStorage.ScreenshotStoragePath}{code}.png");
            request.ContentType = "application/octet-stream";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("Content-Disposition", $"file; filename=\"{code}.png\"");
            request.Headers.Add("Authorization",  "Basic " + token);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();

            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                this.LogMessage("Status", responseText);
            }

            // @todo ErrorHandling.
        }

        public void GetData(string code)
        {
            // Get request to /node/level?filter[title]={code}
            RestClient.Get<DrupalResponseMultiple>($"{drupalLevelUrl}?filter[title]={code}").Then(res => {
                PostRequestData responseData = new PostRequestData()
                {
                    code = res.data[0].attributes.title,
                    user = res.data[0].attributes.field_user,
                    data = res.data[0].attributes.field_data,
                };

                EditorLevelStorage.OnRequestComplete?.Invoke(DatabaseRequestType.GetData, responseData);

                string saveLevelScreenshotPath = $"{EditorLevelStorage.ScreenshotStoragePath}{code}.png";

                if (!File.Exists(saveLevelScreenshotPath))
                {
                    RestClient.Get<DrupalFileRequest>($"{drupalFileUrl}/{res.data[0].relationships.field_screenshot.data.id}").Then(fileRes =>
                    {
                        string url = drupalBaseUrl + fileRes.data.attributes.uri.url;
                        StartCoroutine(nameof(DownloadScreenshotImage), new FileDownloadData(code, url, saveLevelScreenshotPath));
                        this.LogMessage ("ScreenshotFile", JsonUtility.ToJson(res, true));
                    }).Catch(err =>
                    {
                        this.LogMessage("Error", err.Message);
                    });
                }

                this.LogMessage ("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });
        }


        IEnumerator DownloadScreenshotImage(FileDownloadData data)
        {
            string code = data.code;
            string url = data.url;
            string path = data.path;

            WWW www = new WWW(url);
            yield return www;
            File.WriteAllBytes(path, www.bytes);

            EditorLevelStorage.OnRequestComplete?.Invoke(DatabaseRequestType.FileDownload, new PostRequestData());
            Debug.Log("level created for " + code + " - " + url);
        }

        public void CreateData(string code, string data)
        {
            // Post request to /node/level
            // Error handling for code already exists (if unique code checks fail)
            LevelSave level = JsonUtility.FromJson<LevelSave>(data);
            string user = level.creator;

            DrupalRequest request = new DrupalRequest
            {
                data = new DrupalRequestData
                {
                    attributes = new DrupalNodeData()
                    {
                        title = code,
                        body = new DrupalFieldValue($"Level {code} by {user}"),
                        status = false,
                        field_data = data,
                        field_user = user,
                    }
                }
            };

            RestClient.Post<DrupalResponseSingle>(drupalLevelUrl, JsonUtility.ToJson(request)).Then(res =>
            {
                PostRequestData responseData = new PostRequestData()
                {
                    code = res.data.attributes.title,
                    user = res.data.attributes.field_user,
                    data = res.data.attributes.field_data,
                };

                UploadImage(res.data.id, res.data.attributes.title);

                EditorLevelStorage.OnRequestComplete?.Invoke(DatabaseRequestType.InsertData, responseData);
                this.LogMessage ("Levels", JsonUtility.ToJson(res, true));
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });
        }

        public void UpdateData(string code, string data)
        {
            // Get request to /node/level?filter[title]={code}
            // Get the node uuid.
            RestClient.Get<DrupalResponseMultiple>($"{drupalLevelUrl}?filter[title]={code}").Then(res => {
                if (res.data.Length > 0)
                {
                    this.LogMessage ("Levels", JsonUtility.ToJson(res, true));
                    string uuid = res.data[0].id;
                    DrupalUpdateRequest request = new DrupalUpdateRequest
                    {
                        data = new DrupalUpdateRequestData()
                        {
                            id = uuid,
                            attributes = new DrupalNodeUpdateData()
                            {
                                field_data = data,
                            }
                        }
                    };

                    // Patch request to /node/level/{uuid}
                    // Note id is required all else is should be what should be updated.
                    // @see https://www.drupal.org/docs/core-modules-and-themes/core-modules/jsonapi-module/updating-existing-resources-patch
                    RestClient.Patch<DrupalResponseSingle>($"{drupalLevelUrl}/{uuid}", request).Then(res =>
                    {
                        PostRequestData responseData = new PostRequestData()
                        {
                            code = res.data.attributes.title,
                            user = res.data.attributes.field_user,
                            data = res.data.attributes.field_data,
                        };

                        // Dont upload screenshots after publishing.
                        if (!res.data.attributes.status)
                        {
                            UploadImage(res.data.id, res.data.attributes.title);
                        }

                        EditorLevelStorage.OnRequestComplete?.Invoke(DatabaseRequestType.UpdateData, responseData);
                        this.LogMessage ("Update", JsonUtility.ToJson(res, true));
                    }).Catch(err =>
                    {
                        this.LogMessage("Error", err.Message);
                    });
                }
                else
                {
                    // Error handling for no code found -> probably just create instead
                    CreateData(code, data);
                }
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });


        }

        public void CreateComment(string code, string time)
        {
            CreateComment(code, time, "{}");
        }

        public void CreateComment(string code, string time, string ghost)
        {
            string user = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
            PostRequestData responseData = new PostRequestData()
            {
                code = code,
                user = user,
                data = time,
            };

             RestClient.Get<DrupalResponseMultiple>($"{drupalLevelUrl}?filter[title]={code}").Then(res => {
                if (res.data.Length > 0)
                {
                    this.LogMessage ("Levels", JsonUtility.ToJson(res, true));
                    string uuid = res.data[0].id;

                    DrupalCommentRequest request = new DrupalCommentRequest(uuid, time, user, ghost);

                    // Patch request to /node/level/{uuid}
                    // Note id is required all else is should be what should be updated.
                    // @see https://www.drupal.org/docs/core-modules-and-themes/core-modules/jsonapi-module/updating-existing-resources-patch
                    RestClient.Post<DrupalResponseSingle>($"{drupalCommentUrl}", request).Then(res =>
                    {
                        EditorLevelStorage.OnRequestComplete?.Invoke(DatabaseRequestType.InsertComment, responseData);

                        this.LogMessage ("Update", JsonUtility.ToJson(res, true));
                    }).Catch(err =>
                    {
                        // EditorLevelStorage.OnRequestComplete?.Invoke(DatabaseRequestType.InsertComment, responseData);
                        this.LogMessage("Error", err.Message);
                    });
                }
             }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);
            });
        }

        public void GetComments(string code)
        {
            RestClient.Get<DrupalResponseMultiple>($"{drupalLevelUrl}?filter[title]={code}").Then(res =>
            {
                if (res.data.Length > 0)
                {
                    this.LogMessage ("Levels", JsonUtility.ToJson(res, true));
                    string uuid = res.data[0].id;
                    RestClient.Get<DrupalCommentRequestMultiple>($"{drupalCommentUrl}?filter[entity_id.id][value]={uuid}&filter[status]=1").Then(res =>
                    {
                        List<CommentResponseData> comments = new List<CommentResponseData>();
                        foreach (DrupalCommentRequestData data in res.data)
                        {
                            comments.Add(new CommentResponseData(data.attributes.comment_body.value, data.attributes.subject));
                        }

                        CommentsResponseData responseData = new CommentsResponseData()
                        {
                            comments = comments.ToArray()
                        };
                        EditorLevelStorage.OnCommentsRequestComplete?.Invoke(DatabaseRequestType.GetLevelComments, responseData);

                        this.LogMessage ("Comments", JsonUtility.ToJson(res, true));

                    }).Catch(err =>
                    {
                        this.LogMessage("Error", err.Message);

                    });

                }
            }).Catch(err =>
            {
                this.LogMessage("Error", err.Message);

            });




        }


        private void LogMessage(string title, string message) {
        #if UNITY_EDITOR
            if (_showDebug)
            {
                EditorUtility.DisplayDialog (title, message, "Ok");
            }
            else
            {
                Debug.Log(message);
            }
        #else
		    Debug.Log(message);
        #endif
        }

        public string GetUniqueCode()
        {
            // @todo Check with database for uniqueness.
            return GetRandomCode();
        }

        private string GetRandomCode(int length = 5)
        {
            string code = "";
            for(int i=0; i<length; i++)
            {
                code += codeChars[Random.Range(0, codeChars.Length)];
            }

            return code;
        }

        [Serializable]
        public abstract class DrupalResponse
        {
            public JsonApiData jsonapi;
            public string links;
            public string status;
            public string code;
            public string error_description;
        }

        [Serializable]
        public class DrupalResponseSingle : DrupalResponse
        {
            public DrupalNodeWrapper data;
        }

        [Serializable]
        public class DrupalResponseMultiple : DrupalResponse
        {
            public DrupalNodeWrapper[] data;
        }

        [Serializable]
        public class DrupalNodeWrapper
        {
            public string type;
            public string id;
            public DrupalNodeData attributes;
            public DrupalNodeRelationships relationships;
        }

        [Serializable]
        public class JsonApiData
        {
            public string version;
        }

        [Serializable]
        public class DrupalRequest
        {
            public DrupalRequestData data;
        }

        [Serializable]
        public class DrupalRequestData
        {
            public string type = "node--level";
            public DrupalNodeData attributes;
        }

        [Serializable]
        public class DrupalUpdateRequest
        {
            public DrupalUpdateRequestData data;
        }


        [Serializable]
        public class DrupalUpdateRequestData
        {
            public string id;
            public string type = "node--level";
            public DrupalNodeUpdateData attributes;
        }

        [Serializable]
        public class DrupalNodeData
        {
            public string title;
            public DrupalFieldValue body;
            public string field_data;
            public bool status;
            public string field_user;
        }

        [Serializable]
        public class DrupalNodeRelationships
        {
            public DrupalRelationshipsEntityId field_screenshot;

            public DrupalNodeRelationships(string uuid)
            {
                field_screenshot = new DrupalRelationshipsEntityId()
                {
                    data = new DrupalRelationshipsEntityIdData()
                    {
                        id = uuid
                    }
                };
            }
        }

        [Serializable]
        public class DrupalNodeUpdateData
        {
            public string field_data;
        }

        [Serializable]
        public class DrupalFieldValue
        {
            public string value;

            public DrupalFieldValue(string inputValue)
            {
                value = inputValue;
            }
        }

        [Serializable]
        public class DrupalCommentRequestMultiple
        {
            public DrupalCommentRequestData[] data;
        }

        [Serializable]
        public class DrupalCommentRequest
        {
            public DrupalCommentRequestData data;

            public DrupalCommentRequest(string uuid, string subject, string body, string ghost)
            {
                data = new DrupalCommentRequestData()
                {
                    attributes = new DrupalCommentAttributes(subject, body, ghost),
                    relationships = new DrupalCommentRelationships(uuid)
                };
            }
        }

        [Serializable]
        public class DrupalCommentRequestData
        {
            public string type = "comment--comment";
            public DrupalCommentAttributes attributes;
            public DrupalCommentRelationships relationships;
        }

        [Serializable]
        public class DrupalCommentAttributes
        {
            public string subject;
            public DrupalFieldValue comment_body;
            public string field_name = "comment";
            public string field_ghost;
            public string entity_type = "node";
            public bool status = true;

            public DrupalCommentAttributes(string subject, string body, string ghost)
            {
                this.subject = subject;
                field_ghost = ghost;
                comment_body = new DrupalFieldValue(body);
            }
        }

        [Serializable]
        public class DrupalCommentRelationships
        {
            public DrupalRelationshipsEntityId entity_id;

            public DrupalCommentRelationships(string uuid)
            {
                entity_id = new DrupalRelationshipsEntityId()
                {
                    data = new DrupalRelationshipsEntityIdData()
                    {
                        id = uuid
                    }
                };
            }
        }

        [Serializable]
        public class DrupalFileRequest
        {
            public DrupalFileRequestData data;
        }


        [Serializable]
        public class DrupalFileRequestData
        {
            public string type = "file--file";
            public DrupalFileAttributes attributes;
        }

        [Serializable]
        public class DrupalFileAttributes
        {
            public DrupalFileUri uri;
        }

        [Serializable]
        public class DrupalFileUri
        {
            public string url;
        }

        [Serializable]
        public class DrupalRelationshipsEntityId
        {
            public DrupalRelationshipsEntityIdData data;
        }

        [Serializable]
        public class DrupalRelationshipsEntityIdData
        {
            public string type = "node--level";
            public string id;
        }

        public class FileDownloadData
        {
            public string code;
            public string url;
            public string path;

            public FileDownloadData(string code, string url, string path)
            {
                this.code = code;
                this.url = url;
                this.path = path;
            }
        }

        /*
{
  "data": {
    "type": "comment--comment",
    "relationships": {
      "entity_id": {
        "data": {
          "type": "node--level",
          "id": "ebacb42b-425e-49d1-b730-65fddda97979"
        }
      }
    },
    "attributes": {
      "entity_type": "node",
      "field_name": "comments",
      "field_ghost": "{}",
      "subject": "Boom",
      "status": true,
      "comment_body": {
        "value": "New high score by yanniboi",
        "format": "plain_text"
      }
    }
  }
}
*/

    }
}
