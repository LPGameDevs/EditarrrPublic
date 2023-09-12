using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Editarrr.Level;
using Systems;

/// <summary>
/// Kills player upon exiting the bounds set in the current EditorLevelSettings.
/// Has a hidden BoxCollider2D attached.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PlayAreaLimiter : MonoBehaviour
{
    [SerializeField, HideInInspector]
    BoxCollider2D _areaTrigger;

    private void Reset()
    {
        if (_areaTrigger == null)
            _areaTrigger = GetComponent<BoxCollider2D>();

        _areaTrigger.isTrigger = true;
        _areaTrigger.enabled = false;
        _areaTrigger.hideFlags = HideFlags.HideInInspector;
    }

    private void Awake()
    {
        LevelState currentLevelState = FindObjectOfType<LevelPlaySystem>().Manager.LevelManager.LevelState;

        if(currentLevelState == null)
        {
            Debug.Log("Current level state not found, play area limiter not initiated");
            _areaTrigger.enabled = false;
            return;
        }

        _areaTrigger.size = new Vector2(currentLevelState.ScaleX, currentLevelState.ScaleY);
        _areaTrigger.offset = new Vector2(_areaTrigger.size.x * 0.5f, _areaTrigger.size.y * 0.5f);
        _areaTrigger.enabled = true;

        Debug.Log($"Play area initialized with x = {_areaTrigger.size.x} and y = {_areaTrigger.size.y}" );
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HealthSystem healthSystem;
        if (collision.TryGetComponent<HealthSystem>(out healthSystem))
            healthSystem.TakeDamage(healthSystem.MaxHitPoints);
    }
}
