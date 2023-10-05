using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRow : MonoBehaviour
{
    public static event Action OnStartReplay;

    public TMP_Text Place, Time, Name;
    public Button Replay;

    private string _replayData;
    private string _code;

    public void SetRow(string code, string place, float time, string name, string replayData = "")
    {
        _replayData = replayData;
        _code = code;
        bool isReplay = replayData.Length > 0;

        Place.text = place;
        Time.text = time.ToString();
        Name.text = name;
        Replay.gameObject.SetActive(isReplay);
    }

    public void ButtonPressed()
    {
        OnStartReplay?.Invoke();
        string  ghostFile = Path.Combine(Application.persistentDataPath, "ghosts", _code + ".json");
        File.WriteAllText(ghostFile, _replayData);
        PlayerPrefs.SetString("EditorCode", _code);
        // SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.ReplayLevelSceneName);
    }

}
