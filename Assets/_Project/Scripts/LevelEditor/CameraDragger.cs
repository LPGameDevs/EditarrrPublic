using System;
using Editarrr.Input;
using Editarrr.LevelEditor;
using UnityEngine;

public class CameraDragger : MonoBehaviour
{
    public float ScrollSpeed = 2;
    public float DragSpeedX = 3.5f;
    public float DragSpeedY = 2;

    public int ZoomMax = 1;
    public int ZoomMin = 15;

    private Vector3 _maxDrag;
    private Vector3 _minDrag;

    private Vector3 _dragOrigin;
    private Vector3 _cameraStart;
    private Camera _camera;

    #region Input
    [field: SerializeField] private InputValue MousePosition { get; set; }
    [field: SerializeField] private InputValue MouseScroll { get; set; }
    [field: SerializeField] private InputValue MouseMiddleButton { get; set; }
    #endregion

    private void Start()
    {
        _camera = GetComponent<Camera>();

        EditorLevelSettings currentSettings = FindObjectOfType<EditorLevelSystem>().Manager.Settings;
        _maxDrag = new Vector3(currentSettings.EditorLevelScaleX / 2f, currentSettings.EditorLevelScaleY / 2f, 0);
        _minDrag = _maxDrag * -1f;
}

    void Update()
    {
        _camera.orthographicSize =
            Mathf.Clamp(_camera.orthographicSize - MouseScroll.Read<Vector2>().y * ScrollSpeed * Time.deltaTime * _camera.orthographicSize, ZoomMax, ZoomMin);

        Vector3 mousePosition = MousePosition.Read<Vector2>();

        if (MouseMiddleButton.WasPressed)
        {
            _dragOrigin = mousePosition;
            _cameraStart = transform.position;
            return;
        }

        if (!MouseMiddleButton.IsPressed) return;

        Vector3 pos = _camera.ScreenToViewportPoint(mousePosition - _dragOrigin);

        Vector3 posNormalised = new Vector3(pos.x * DragSpeedX, pos.y * DragSpeedY, 0);

        Vector3 move = _cameraStart - (posNormalised * _camera.orthographicSize);


        var vertExtent = _camera.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        move.x = Mathf.Clamp(move.x, _minDrag.x + horzExtent, _maxDrag.x - horzExtent);
        move.y = Mathf.Clamp(move.y, _minDrag.y + vertExtent, _maxDrag.y - vertExtent);
        move.z = transform.position.z;
        transform.position = move;
    }


    private void SetStartingPosition(Vector3 position)
    {
        position.z = -10;
        transform.position = position;
    }

    private void OnEnable()
    {
        EditorLevelManager.OnCameraTileSet += SetStartingPosition;
    }

    private void OnDisable()
    {
        EditorLevelManager.OnCameraTileSet -= SetStartingPosition;
    }
}
