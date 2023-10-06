using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform StaticBackground;
    public Transform BottomBackground;
    public Transform TopBackground;

    [SerializeField] List<BackgroundElement> elements;
    [SerializeField] Camera Camera;
    [SerializeField] float TopMin;
    [SerializeField] float BottomMax;

    private Vector2 _currentCameraPosition = Vector2.zero, _previousCameraPosition = Vector2.zero;

    /**
     * This function moves different background layers to match the camera's
     * position.
     */
    private void Update()
    {
        _previousCameraPosition = _currentCameraPosition;
        _currentCameraPosition = Camera.transform.position;

        MoveBackgroundElements();

        Vector3 currentBottomPosition = BottomBackground.position;
        float newBottomY = Mathf.Min(_currentCameraPosition.y, BottomMax);
        BottomBackground.position = new Vector3(currentBottomPosition.x, newBottomY, currentBottomPosition.z);
    }

    private void MoveBackgroundElements()
    {
        foreach(BackgroundElement element in elements)
        {
            Vector2 delta = _currentCameraPosition - _previousCameraPosition;
            element.transform.position += new Vector3(delta.x * element.ScrollingFactor.x, delta.y * element.ScrollingFactor.y);
        }
    }
}

