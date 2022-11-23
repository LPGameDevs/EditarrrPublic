using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LevelEditor
{
  public class GhostRecorder : MonoBehaviour
  {
    private string _ghostFile;
    private GameObject[] _trackables = new GameObject[1];
    private GameObject[] _ghostables = new GameObject[1];
    private List<Vector3> _recordedTransforms = new List<Vector3>();
    private List<Vector3> _playbackTransforms = new List<Vector3>();
    private int _playbackHeadIndex;
    public bool IsPlayingBack = false;

    protected virtual void Start()
    {
      string code = PlayerPrefs.GetString("EditorCode");

      // check directory exists.
      if (!Directory.Exists(Application.persistentDataPath + "/ghosts"))
      {
        Directory.CreateDirectory(Application.persistentDataPath + "/ghosts");
      }
      _ghostFile = Path.Combine(Application.persistentDataPath, "ghosts", code + ".json");
      _trackables[0] = GameObject.Find("Player");
      _ghostables[0] = GameObject.Find("Player");

      if (IsPlayingBack)
      {
        OnResetGame();
      }
    }

    protected virtual void Update()
    {
      // UnityEngine.Camera main = UnityEngine.Camera.main;
      // main.transparencySortAxis = this.transform.up;
      // main.transparencySortMode = TransparencySortMode.CustomAxis;
      if (IsPlayingBack && _playbackTransforms.Count > 0)
      {
        for (int index = 0; index < _ghostables.Length; ++index)
        {
          _ghostables[index].transform.position = _playbackTransforms[_playbackHeadIndex];
        }
      }
    }

    protected virtual void FixedUpdate()
    {
      if (!IsPlayingBack)
      {
        for (int index = 0; index < _trackables.Length; ++index)
          _recordedTransforms.Add(_trackables[index].transform.position);
      }
      if (!IsPlayingBack || _playbackTransforms.Count <= 0)
        return;
      for (int index = 0; index < _ghostables.Length; ++index)
      {
        _ghostables[index].transform.position = _playbackTransforms[_playbackHeadIndex];
      }
      if (_playbackHeadIndex >= _playbackTransforms.Count - 1)
        return;
      ++_playbackHeadIndex;
    }

    public void SaveGhostRun()
    {
      if (IsPlayingBack)
      {
        return;
      }

      _playbackHeadIndex = 0;
      _playbackTransforms = _recordedTransforms;
      if (string.IsNullOrEmpty(_ghostFile))
        return;
      SaveGhostData(_ghostFile, _playbackTransforms);
    }

    public void OnResetGame()
    {
      _recordedTransforms.Clear();
      IsPlayingBack = false;
      _playbackHeadIndex = 0;
      if (_ghostFile.Length > 0)
      {
        LoadGhostData(_ghostFile, ref _playbackTransforms);
        IsPlayingBack = _playbackTransforms.Count > 0;
      }
      else
      {
        _playbackTransforms.Clear();
      }
    }

    public void ClearGhostData()
    {
      _recordedTransforms.Clear();
      _playbackTransforms.Clear();
      IsPlayingBack = false;
      _playbackHeadIndex = 0;
    }

    private void SaveGhostData(string ghostFile, List<Vector3> data)
    {
      try
      {
        Directory.CreateDirectory(Path.GetDirectoryName(ghostFile));

        GhostSave saveData = new GhostSave();

        for (int i = 0; i < data.Count; i++)
        {
          saveData.positions.Add(data[i]);
        }

        string dataString = JsonUtility.ToJson(saveData);
        File.WriteAllText(ghostFile, dataString);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) string.Format("Error saving ghost data: {0}", (object) ex));
      }
    }

    private void LoadGhostData(string ghostFile, ref List<Vector3> data)
    {
      try
      {
        data.Clear();
        if (!File.Exists(ghostFile))
        {
          return;
        }

        string rawData = File.ReadAllText(ghostFile);
        GhostSave ghostData = JsonUtility.FromJson<GhostSave>(rawData);

        if (ghostData.positions.Count < 1)
        {
          throw new Exception("No data found for ghost run.");
        }

        for (int index = 0; index < ghostData.positions.Count; ++index)
        {
          data.Add(ghostData.positions[index]);
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) string.Format("Error loading ghost data: {0}", (object) ex));
      }
    }

    protected void OnEnable()
    {
      LevelWin.OnLevelWin += SaveGhostRun;
    }

    protected void OnDisable()
    {
      LevelWin.OnLevelWin -= SaveGhostRun;
    }

    private struct TransformData
    {
      public Vector3 position;

    }

    [Serializable]
    public class GhostSave
    {
      public List<Vector3> positions = new List<Vector3>();
    }

  }
}
