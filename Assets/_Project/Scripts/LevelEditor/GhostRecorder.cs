using System;
using System.Collections.Generic;
using System.IO;
using Editarrr.Level;
using Gameplay;
using UnityEngine;

namespace LevelEditor
{
  public class GhostRecorder : MonoBehaviour
  {
    private string _ghostFile;
    private List<GameObject> _trackables = new List<GameObject>();
    private List<GameObject> _ghostables = new List<GameObject>();
    private List<Vector3> _recordedTransforms = new List<Vector3>();
    private List<Vector3> _playbackTransforms = new List<Vector3>();
    private int _playbackHeadIndex;
    public bool IsPlayingBack = false;

    private bool _playerFound;
    private float _playerSeekDelay = 0.5f;
    private float _playerSeekTimer = 0;

    private LevelManager _levelManager;
    private string _code;

    public void SetLevelManager(LevelManager levelManager)
    {
      _levelManager = levelManager;
    }

    public void SetLevelCode(string code)
    {
      _code = code;
    }

    protected virtual void Start()
    {

      // check directory exists.
      if (!Directory.Exists(Application.persistentDataPath + "/ghosts"))
      {
        Directory.CreateDirectory(Application.persistentDataPath + "/ghosts");
      }
      _ghostFile = Path.Combine(Application.persistentDataPath, "ghosts", _code + ".json");

      if (IsPlayingBack)
      {
        OnResetGame();
      }
    }

    protected virtual void Update()
    {
      if (!_playerFound)
      {
        HandlePlayerSeek();
        return;
      }

      if (_ghostables.Count == 0)
      {
        return;
      }

      // UnityEngine.Camera main = UnityEngine.Camera.main;
      // main.transparencySortAxis = this.transform.up;
      // main.transparencySortMode = TransparencySortMode.CustomAxis;
      if (IsPlayingBack && _playbackTransforms.Count > 0)
      {
        for (int index = 0; index < _ghostables.Count; ++index)
        {
          _ghostables[index].transform.position = _playbackTransforms[_playbackHeadIndex];
        }
      }
    }

    protected virtual void FixedUpdate()
    {
      if (!_playerFound)
      {
        return;
      }

      if (!IsPlayingBack)
      {
        for (int index = 0; index < _trackables.Count; ++index)
          _recordedTransforms.Add(_trackables[index].transform.position);
      }
      if (!IsPlayingBack || _playbackTransforms.Count <= 0)
        return;
      for (int index = 0; index < _ghostables.Count; ++index)
      {
        _ghostables[index].transform.position = _playbackTransforms[_playbackHeadIndex];
      }
      if (_playbackHeadIndex >= _playbackTransforms.Count - 1)
        return;
      ++_playbackHeadIndex;
    }

    private void HandlePlayerSeek()
    {
      _playerSeekTimer += Time.deltaTime;
      if (_playerSeekTimer < _playerSeekDelay)
      {
        return;
      }

      var player = GameObject.FindWithTag("Player");
      if (player)
      {
        _playerFound = true;
        _trackables.Add(player);
        _ghostables.Add(player);
      }
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
      {
        return;
      }
      SaveGhostData(_playbackTransforms);
    }

    public void OnResetGame()
    {
      _recordedTransforms.Clear();
      IsPlayingBack = false;
      _playbackHeadIndex = 0;
      if (_ghostFile.Length > 0)
      {
        LoadGhostData(ref _playbackTransforms);
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

    private void SaveGhostData(List<Vector3> data)
    {
      try
      {
        GhostSave saveData = new GhostSave();

        for (int i = 0; i < data.Count; i++)
        {
          saveData.positions.Add(data[i]);
        }

        string dataString = JsonUtility.ToJson(saveData);
        File.WriteAllText(_ghostFile, dataString);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) string.Format("Error saving ghost data: {0}", (object) ex));
      }
    }

    private void LoadGhostData(ref List<Vector3> data)
    {
      try
      {
        data.Clear();
        if (!File.Exists(_ghostFile))
        {
          return;
        }

        string rawData = File.ReadAllText(_ghostFile);
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
      Chest.OnChestOpened += SaveGhostRun;
    }

    protected void OnDisable()
    {
      Chest.OnChestOpened -= SaveGhostRun;
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
