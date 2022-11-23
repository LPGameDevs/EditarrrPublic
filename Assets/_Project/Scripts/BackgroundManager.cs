using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform StaticBackground;
    public Transform BottomBackground;
    public Transform TopBackground;
    public Camera Camera;
    public float TopMin;
    public float BottomMax;

    private void Update()
    {
        Vector3 cameraPosition = Camera.transform.position;
        Vector3 currentStaticPosition = StaticBackground.position;
        StaticBackground.position = new Vector3(cameraPosition.x, currentStaticPosition.y, currentStaticPosition.z);

        Vector3 currentBottomPosition = BottomBackground.position;
        float newBottomY = Mathf.Min(cameraPosition.y, BottomMax);
        BottomBackground.position = new Vector3(currentBottomPosition.x, newBottomY, currentStaticPosition.z);

        Vector3 currentTopPosition = TopBackground.position;
        float newTopY = Mathf.Max(cameraPosition.y, TopMin);
        TopBackground.position = new Vector3(currentTopPosition.x, newTopY, currentStaticPosition.z);

    }
}
