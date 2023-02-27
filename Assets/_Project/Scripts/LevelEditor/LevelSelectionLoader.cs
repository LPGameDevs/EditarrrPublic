using System.Collections.Generic;
using System.IO;
using CorgiExtension;
using LevelEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionLoader : MonoBehaviour
{
    public EditorLevel LevelPrefab;
    public EditorLevel DraftPrefab;

    private List<Transform> _loadedLevels = new List<Transform>();
    private Carousel _carousel;

    private void Awake()
    {
        _carousel = GetComponentInParent<Carousel>();
    }

    void Start()
    {
        DestroyAndRefreshLevels();
    }

    private void SetupLevelPrefabByCode(string code)
    {
        LevelSave levelData = EditorLevelStorage.Instance.GetLevelData(code);
        string userName = PlayerPrefs.GetString(UserNameForm.UserNameStorageKey);
        EditorLevel level;

        if (levelData.published)
        {
             level = Instantiate(LevelPrefab, transform);
        }
        else
        {
            level = Instantiate(DraftPrefab, transform);
        }

        level.setTitle(code.ToUpper());
        level.Creator.text = levelData.creator.ToUpper();

        if (levelData.creator.ToLower() != userName.ToLower())
        {
            level.HideEditorTools();
        }

        _loadedLevels.Add(level.transform);

        // create texture from image file
        RawImage image = level.GetComponentInChildren<RawImage>();
        string path = $"{EditorLevelStorage.ScreenshotStoragePath}{code}.png";

        bool isDistroLevel = false;
        if (!File.Exists(path) && File.Exists($"{EditorLevelStorage.DistroLevelStoragePath}{code}.png"))
        {
            path = $"{EditorLevelStorage.DistroLevelStoragePath}{code}.png";
            isDistroLevel = true;
        }

        if (isDistroLevel)
        {
            level.HideDeleteButton();
        }

        if (image && File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            image.texture = tex;
            image.color = Color.white;
        }

    }

    /**
     * Destroy all levels in the selection scene and reload.
     *
     * This is easier than trying to track what has changed.
     */
    private void DestroyAndRefreshLevels()
    {
        foreach (var level in _loadedLevels)
        {
            Destroy(level.gameObject);
        }
        _loadedLevels = new List<Transform>();

        var info = EditorLevelStorage.Instance.GetStoredLevelFiles();
        foreach (FileInfo f in info)
        {
            // Ignore the level reset template.
            if (f.Name == "level.json")
            {
                continue;
            }

            // Get the level code from the file name without the extension.
            string levelCode = f.Name.Remove(f.Name.Length - f.Extension.Length);
            SetupLevelPrefabByCode(levelCode);
        }

        if (_carousel != null)
        {
            _carousel.SetCount(_loadedLevels.Count);
        }
    }

    private void OnEnable()
    {
        EditorLevelStorage.OnLevelRefresh += DestroyAndRefreshLevels;
    }

    private void OnDisable()
    {
        EditorLevelStorage.OnLevelRefresh -= DestroyAndRefreshLevels;
    }
}
