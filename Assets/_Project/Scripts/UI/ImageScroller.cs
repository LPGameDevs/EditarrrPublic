using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    [SerializeField] RectTransform scrollTarget, sizeReference;
    [SerializeField] float scrollSpeed;
    Vector2 startingPosition;

    void Awake()
    {
        startingPosition = scrollTarget.anchoredPosition;
    }


    void Update()
    {
        if (scrollTarget.anchoredPosition.x > -sizeReference.rect.width)
            transform.Translate(scrollSpeed, 0, 0);
        else
            scrollTarget.anchoredPosition = startingPosition;
    }
}
