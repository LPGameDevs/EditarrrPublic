using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class LevelWin : MonoBehaviour
{
    public static event Action OnLevelWin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!CheckConditions(other))
        {
            return;
        }

        OnLevelWin?.Invoke();
        MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 0f, true);
    }

    private bool CheckConditions(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
}
