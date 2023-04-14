using CorgiExtension;
using Editarrr.Level;
using Editarrr.LevelEditor;
using Editarrr.Managers;
using Editarrr.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSelectionManager", menuName = "Managers/Level/new Level Selection Manager")]
public class LevelSelectionManager : ManagerComponent
{
    private const string Documentation = "This handles loading levels for level selection.";

    [field: SerializeField, Info(Documentation)] private EditorLevelExchange Exchange { get; set; }

    [field: SerializeField, Header("Managers")] private LevelManager LevelManager { get; set; }

    // From system
    private LevelSelectionLoader _levelLoader { get; set; }


    public void SetLevelLoader(LevelSelectionLoader levelLoader)
    {
        _levelLoader = levelLoader;
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

    private void LevelStorage_AllLevelsLoadedCallback(LevelState[] levels)
    {
        foreach (var level in levels)
        {
            _levelLoader.AddLevelPrefabFromData(level);
        }
    }

    private void OnLevelDeleted(string code)
    {
        LevelManager.Delete(code);
        DestroyAndRefreshLevels();
    }

    private void OnLevelSelected(string code)
    {
        Exchange.SetCode(code);
        Exchange.SetAutoload(code.Length > 0);
    }

    public override void DoOnEnable()
    {
        EditorLevel.OnEditorLevelSelected += OnLevelSelected;
        EditorLevel.OnEditorLevelDelete += OnLevelDeleted;
    }

    public override void DoOnDisable()
    {
        EditorLevel.OnEditorLevelSelected -= OnLevelSelected;
        EditorLevel.OnEditorLevelDelete -= OnLevelDeleted;
    }
}
