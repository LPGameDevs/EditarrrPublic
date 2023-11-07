using Player;
using Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievements/Threshold Achievement")]
public class ThresholdAchievement : ScriptableObject
{
    #region Fields and Properties

    [SerializeField] private List<GameAchievement> _achievements = new();
    [SerializeField] private List<int> _thresholds = new();

    public Dictionary<GameAchievement, int> AchievementDictionary = new();
    
    [field: SerializeField] public string SavePrefString { get; private set; }

    #endregion

    #region Methods

    private void OnValidate()
    {
        InitializeDictionary();
    }

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        if (_achievements.Count == _thresholds.Count)
        {
            _achievements.Sort();
            _thresholds.Sort();

            AchievementDictionary.Clear();

            for (int i = 0; i < _achievements.Count; i++)
                AchievementDictionary[_achievements[i]] = _thresholds[i];
        }
    }

    #endregion
}
