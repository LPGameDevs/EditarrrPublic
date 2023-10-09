using Browser;
using CorgiExtension;
using Editarrr.Level;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Editarrr.UI;
using Gameplay.GUI;
using Level.Storage;
using UI;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSelectionManager", menuName = "Managers/Level/new Level Selection Manager")]
public class LevelSelectionManager : ManagerComponent
{
    private const string Documentation = "This handles loading levels for level selection.";

    [field: SerializeField, Info(Documentation)] private EditorLevelExchange Exchange { get; set; }

    [field: SerializeField, Header("Managers")] private LevelManager LevelManager { get; set; }

    // From system
    private LevelSelectionLoader _levelLoader { get; set; }
    private LeaderboardForm _leaderboard { get; set; }

    private IModalPopup _uploadModal { get; set; }
    private IModalPopup _deleteModal { get; set; }

    public void SetUploadModal(IModalPopup uploadModal)
    {
        _uploadModal = uploadModal;
    }

    public void SetDeleteModal(IModalPopup deleteModal)
    {
        _deleteModal = deleteModal;
    }

    public void SetLevelLoader(LevelSelectionLoader levelLoader)
    {
        _levelLoader = levelLoader;
    }

    public void SetLeaderboard(LeaderboardForm leaderboard)
    {
        _leaderboard = leaderboard;
        _leaderboard.gameObject.SetActive(false);
    }

    public override void DoAwake()
    {
        LevelManager.DoAwake();
    }

    public override void DoStart()
    {
        this.DestroyAndRefreshLevels();
    }

    private void DestroyAndRefreshLevels()
    {
        _levelLoader.DestroyLevels();

        LevelManager.LoadAll(this.LevelStorage_AllLevelsLoadedCallback);
    }

    private void LevelStorage_AllLevelsLoadedCallback(LevelStub[] levels)
    {
        foreach (var level in levels)
        {
            string screenshotPath = LevelManager.GetScreenshotPath(level.Code);
            _levelLoader.AddLevelPrefabFromData(level, screenshotPath);
        }
    }

    private void OnLevelDeleted(string code)
    {
        if (_deleteModal is ModalPopupConfirmation confirmModal)
        {
            confirmModal.SetConfirm(DeleteLevel);
        }

        _deleteModal.Open();

        void DeleteLevel()
        {
            LevelManager.Delete(code);
            DestroyAndRefreshLevels();
        }
    }

    private void OnLevelUploadRequested(string code)
    {
        if (_uploadModal is ModalPopupConfirmation confirmModal)
        {
            confirmModal.SetConfirm(UploadLevel);
        }

        _uploadModal.Open();

        void UploadLevel()
        {
            LevelManager.PublishAndUpload(code, OnLevelUploadComplete);
            DestroyAndRefreshLevels();
        }
    }

    private void OnLevelUploadComplete(LevelSave level)
    {
        // @todo fix this by not invoking null for aws.
        if (level != null)
        {
            Debug.Log("Upload finished for level " + level.Code + ".");
        }

        // Update display.
        DestroyAndRefreshLevels();
    }


    private void OnLevelSelected(string code)
    {
        Exchange.SetCode(code);
        Exchange.SetAutoload(code.Length > 0);
    }

    private void OnLeaderboardRequested(string code)
    {
        _leaderboard.gameObject.SetActive(true);
        _leaderboard.SetCode(code);

        LevelManager.LevelStorage.LoadLevelData(code, LeaderboardLevelDataLoaded);
        void LeaderboardLevelDataLoaded(LevelSave levelSave)
        {
            LevelManager.GetScoresForLevel(levelSave.RemoteId, LeaderboardScoresLoaded);

            void LeaderboardScoresLoaded(ScoreStub[] scoreStubs)
            {
                _leaderboard.SetScores(scoreStubs);
            }
        }
    }

    public override void DoOnEnable()
    {
        EditorLevel.OnEditorLevelSelected += OnLevelSelected;
        EditorLevel.OnEditorLevelDelete += OnLevelDeleted;
        EditorLevel.OnEditorLevelUpload += OnLevelUploadRequested;
        EditorLevel.OnLeaderboardRequest += OnLeaderboardRequested;
    }

    public override void DoOnDisable()
    {
        EditorLevel.OnEditorLevelSelected -= OnLevelSelected;
        EditorLevel.OnEditorLevelDelete -= OnLevelDeleted;
        EditorLevel.OnEditorLevelUpload -= OnLevelUploadRequested;
        EditorLevel.OnLeaderboardRequest -= OnLeaderboardRequested;
    }
}
