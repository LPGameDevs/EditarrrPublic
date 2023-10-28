using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] GameObject _displayPrefab;
    [SerializeField] Sprite[] _sprites;

    int _currentHP = 0, _maxHP = -1;

    private void OnEnable()
    {
        HealthSystem.OnHitPointsChanged += OnHitPointsChanged;
    }

    private void OnDisable()
    {
        HealthSystem.OnHitPointsChanged -= OnHitPointsChanged;
    }

    private void OnHitPointsChanged(object sender, HealthSystem.OnHealthChangedArgs args)
    {
        if (_maxHP == -1)
            _maxHP = (int)args.previousValue;

        _currentHP = (int)(args.value / _maxHP * 4);

        GameObject displayInstance = Instantiate(_displayPrefab, transform.position, Quaternion.identity);
        displayInstance.GetComponentInChildren<Image>().sprite = _sprites[_currentHP];
    }
}
