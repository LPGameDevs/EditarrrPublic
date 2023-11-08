using Browser;
using Editarrr.Level;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using Editarrr.UI;
using Level.Storage;
using LevelEditor;
using Singletons;
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

    private Canvas ModalCanvas { get; set; }

    private IModalPopup _invalidModal { get; set; }
    private IModalPopup _incompleteModal { get; set; }
    private IModalPopup _uploadModal { get; set; }
    private IModalPopup _deleteModal { get; set; }
    private AchievementPopupBlock _achievementBlock { get; set; }

    public void SetCanvas(Canvas modalCanvas)
    {
        this.ModalCanvas = modalCanvas;
    }

    public void SetUploadModal(IModalPopup uploadModal)
    {
        _uploadModal = uploadModal;
    }

    public void SetDeleteModal(IModalPopup deleteModal)
    {
        _deleteModal = deleteModal;
    }

    public void SetInvalidModal(IModalPopup invalidModal)
    {
        _invalidModal = invalidModal;
    }

    public void SetIncompleteModal(IModalPopup modal)
    {
        _incompleteModal = modal;
    }

    public void SetAchievementBlock(AchievementPopupBlock block)
    {
        _achievementBlock = block;
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

    private void OnEnable() => SceneTransitionManager.OnSceneRemoved += OnSceneClosed;

    private void OnDisable() => SceneTransitionManager.OnSceneRemoved -= OnSceneClosed;

    private void DestroyAndRefreshLevels()
    {
        _levelLoader.DestroyLevels();

        LevelManager.LoadAll(this.LevelStorage_AllLevelsLoadedCallback);
    }

    private void LevelStorage_AllLevelsLoadedCallback(LevelStub[] levels, string cursor = "")
    {
        foreach (var level in levels)
        {
            string screenshotPath = LevelManager.GetScreenshotPath(level.Code, level.IsDistro);
            _levelLoader.AddLevelPrefabFromData(level, screenshotPath);
        }
    }

    private void OnSceneClosed(string sceneName)
    {
        if (sceneName != SceneTransitionManager.BrowserSceneName)
            return;

        DestroyAndRefreshLevels();
    }

    private void OnLevelDeleted(string code)
    {
        if (_deleteModal is ModalPopupConfirmation confirmModal)
        {
            confirmModal.SetConfirm(DeleteLevel);
        }

        _deleteModal.Open(this.ModalCanvas.transform, true);

        void DeleteLevel()
        {
            LevelManager.Delete(code);
            DestroyAndRefreshLevels();
        }
    }

    private void OnLevelUploadRequested(string code)
    {
        this.LevelManager.LevelStorage.LoadLevelData(code, LevelLoadedForUpload);

        void LevelLoadedForUpload(LevelSave levelSave)
        {
            if (!levelSave.IsLevelValid())
            {
                this._invalidModal.Open(this.ModalCanvas.transform, true);
                return;
            }

            if (!levelSave.Completed)
            {
                this._incompleteModal.Open(this.ModalCanvas.transform, true);
                return;
            }

            if (_uploadModal is ModalPopupConfirmation confirmModal)
            {
                confirmModal.SetConfirm(UploadLevel);
            }

            this._uploadModal.Open(this.ModalCanvas.transform, true);

            void UploadLevel()
            {
                LevelManager.PublishAndUpload(code, OnLevelUploadComplete);
                DestroyAndRefreshLevels();
                AnalyticsManager.Instance.TrackEvent(AnalyticsEvent.LevelUpload, levelSave.Code);
            }
        }
    }

    private void OnLevelUploadComplete(LevelSave level)
    {
        // @todo fix this by not invoking null for aws.
        if (level != null)
        {
            Debug.Log("Upload finished for level " + level.Code + ".");
        }

        AchievementManager.Instance.UnlockAchievement(GameAchievement.LevelSubmitted);
        TwitchManager.Instance.SendNotification($"{level.CreatorName} just uploaded a new level: {level.Code}.");

        // Update display.
        DestroyAndRefreshLevels();
    }

    private void OnLevelSelected(string code)
    {
        Exchange.SetCode(code);
        Exchange.SetAutoload(code.Length > 0);
    }

    private void OnLevelPlayRequested()
    {
        if (!this.Exchange.LoadOnStart)
        {
            return;
        }

        string code = this.Exchange.CodeToLoad;
        this.LevelManager.Load(code, LevelLoadedForPlay);

        void LevelLoadedForPlay(LevelState levelState)
        {
            if (!levelState.IsLevelValid())
            {
                this._invalidModal.Open(this.ModalCanvas.transform, true);
                return;
            }

            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.TestLevelSceneName);
        }
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

    private void OnShowAchievement(PopupAchievement achievement)
    {

        var popup = Instantiate(this._achievementBlock, this.ModalCanvas.transform);
        popup.Setup(achievement);

    }

    public override void DoOnEnable()
    {
        EditorLevel.OnEditorLevelSelected += OnLevelSelected;
        EditorLevel.OnEditorLevelPlayRequest += OnLevelPlayRequested;
        EditorLevel.OnEditorLevelDelete += OnLevelDeleted;
        EditorLevel.OnEditorLevelUpload += OnLevelUploadRequested;
        EditorLevel.OnLeaderboardRequest += OnLeaderboardRequested;
        AchievementManager.OnShowAchievement += OnShowAchievement;
    }

    public override void DoOnDisable()
    {
        EditorLevel.OnEditorLevelSelected -= OnLevelSelected;
        EditorLevel.OnEditorLevelPlayRequest -= OnLevelPlayRequested;
        EditorLevel.OnEditorLevelDelete -= OnLevelDeleted;
        EditorLevel.OnEditorLevelUpload -= OnLevelUploadRequested;
        EditorLevel.OnLeaderboardRequest -= OnLeaderboardRequested;
        AchievementManager.OnShowAchievement -= OnShowAchievement;
    }
}
