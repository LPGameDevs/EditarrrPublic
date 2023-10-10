using System;
using TMPro;
using UnityEngine;

public class GameVersion : MonoBehaviour
{
    private TextMeshProUGUI _version;

    private void Awake()
    {
        _version = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _version.text = "v" + Application.version;
    }
}
