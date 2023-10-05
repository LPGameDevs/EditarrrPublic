using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElement : MonoBehaviour
{
    [field: SerializeField, Editarrr.Misc.Info("Set scrolling factors between 0 and 1 for each axis.\n0 keeps the element static and 1 maintains parity with the camera.")] public Vector2 ScrollingFactor { get; private set; }
    [field: SerializeField] public float RandomInitializationX { get; private set; }
    [field: SerializeField] public float RandomInitializationY { get; private set; }

    private void Awake()
    {
        float newPositionX = Random.Range(-RandomInitializationX, RandomInitializationX);
        float newPositionY = Random.Range(-RandomInitializationY, RandomInitializationY);

        transform.position = new Vector2(newPositionX, newPositionY);
    }
}