using Editarrr.Audio;
using Singletons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CursorHandler : UnityPersistentSingleton<CursorHandler>
{
    [SerializeField] Texture2D _defaultTexture, _pressedTexture;
    [SerializeField] Texture2D _uiHoverTexture, _uiPressedTexture;
    [SerializeField] GameObject _clickEffectPrefab;

    bool _overrideActive;
    private CursorMode _mode;
    private Camera _camera;

    private void Start()
    {
        Initialize();
//        _mode = CursorMode.Auto;

//#if UNITY_EDITOR
        _mode = CursorMode.ForceSoftware;
//#endif

        Cursor.SetCursor(_defaultTexture, Vector2.zero, _mode);
    }

    private void Initialize(string sceneName = null)
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!_overrideActive)
                ChangeCursorImage(_defaultTexture);
            else
                ChangeCursorImage(_uiHoverTexture);
        }

    }

    private void ChangeCursorImage(Texture2D newTexture)
    {
        Cursor.SetCursor(newTexture, Vector2.zero, _mode);
    }

    private void Click()
    {
        if (!_overrideActive)
            ChangeCursorImage(_pressedTexture);
        else
            ChangeCursorImage(_uiPressedTexture);

        if (_camera == null)
            Initialize();

        Vector3 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition); /*- (uiPressedTexture.Size() * 0.5f)*/
        GameObject clickEffect = Instantiate(_clickEffectPrefab, clickPosition, Quaternion.identity);
        Destroy(clickEffect, 0.2f);
        AudioManager.Instance.PlayRandomizedAudioClip(AudioManager.MEDIUM_CLICK_CLIP_NAME, 0.2f, 0.2f);
    }

    public void SetTextureOverride(bool hoveringOverUI)
    {
        _overrideActive = hoveringOverUI;
        if (!_overrideActive)
            ChangeCursorImage(_defaultTexture);
        else
            ChangeCursorImage(_uiHoverTexture);
    }
}
