using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElement : MonoBehaviour
{
    [field: SerializeField, Editarrr.Misc.Info("Set scrolling factors between 0 and 1 for each axis.\n0 keeps the element static and 1 maintains parity with the camera.")] public Vector2 ScrollingFactor { get; private set; }
    [field: SerializeField] public float RandomInitializationX { get; private set; }
    [field: SerializeField] public float RandomInitializationY { get; private set; }

    Vector3 TargetPosition { get; set; }

    private void Awake()
    {
        float newPositionX = Random.Range(-RandomInitializationX, RandomInitializationX);
        float newPositionY = Random.Range(-RandomInitializationY, RandomInitializationY);

        this.TargetPosition = transform.position = new Vector2(newPositionX, newPositionY);
    }

    public void Change(Vector3 delta)
    {
        this.TargetPosition += new Vector3(delta.x * this.ScrollingFactor.x, delta.y * this.ScrollingFactor.y);
        // element.transform.position += new Vector3(delta.x * element.ScrollingFactor.x, delta.y * element.ScrollingFactor.y);
    }

    private void LateUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.TargetPosition, 1 - .2f * Time.deltaTime);
    }
}