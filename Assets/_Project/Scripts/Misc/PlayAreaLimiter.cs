using System.Collections;
using System.Collections.Generic;
using Editarrr.LevelEditor;
using UnityEngine;
using Player;
using Systems;

namespace Editarrr.Level
{
    /// <summary>
    /// Stops players and the camera from moving beyond the play area, kills player and destroys objects below bottom limit.
    /// </summary>
    [RequireComponent(typeof(CompositeCollider2D), typeof(BoxCollider2D), typeof(BoxCollider2D))]
    internal class PlayAreaLimiter : MonoBehaviour
    {
        [SerializeField] CompositeCollider2D _areaCollider;
        [SerializeField] BoxCollider2D _areaColliderBase;
        [SerializeField] BoxCollider2D _fallDeathTrigger;
        [SerializeField] PolygonCollider2D _cameraConfinementCollider;
        [SerializeField] float additionalPlayAreaOffset = 2f, additionalFallDeathTriggerOffset = -1.5f;

        private void Awake()
        {
            EditorLevelSettings currentSettings = FindObjectOfType<LevelPlaySystem>().Manager.Settings;

            if (currentSettings == null)
            {
                Debug.Log("Current level state not found, play area limiter not initiated");
                _areaCollider.enabled = false;
                return;
            }

            _areaColliderBase.size = new Vector2(currentSettings.EditorLevelScaleX, currentSettings.EditorLevelScaleY + additionalPlayAreaOffset);
            _areaCollider.offset = new Vector2(currentSettings.EditorLevelScaleX * 0.5f, (currentSettings.EditorLevelScaleY * 0.5f) - 1f);

            _fallDeathTrigger.size = new Vector2(currentSettings.EditorLevelScaleX, _fallDeathTrigger.size.y);
            _fallDeathTrigger.offset = new Vector2(_fallDeathTrigger.size.x * 0.5f, -_areaColliderBase.offset.y + additionalFallDeathTriggerOffset);

            //Cinemachine's camera confiner requires a polygon collider, which needs to be set up point by point. CreatePrimitive doesn't work here.
            //_cameraConfinementCollider.CreatePrimitive(4, new Vector2(currentSettings.EditorLevelScaleX, currentSettings.EditorLevelScaleY));
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(0f, 0f);
            points[1] = new Vector2(0f, currentSettings.EditorLevelScaleY);
            points[2] = new Vector2(currentSettings.EditorLevelScaleX, currentSettings.EditorLevelScaleY);
            points[3] = new Vector2(currentSettings.EditorLevelScaleX, 0f);

            _cameraConfinementCollider.pathCount = 1;
            _cameraConfinementCollider.offset = Vector2.zero;
            _cameraConfinementCollider.points = points;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            HealthSystem healthSystem;
            if (collider.TryGetComponent<HealthSystem>(out healthSystem))
                healthSystem.TakeDamage(healthSystem.MaxHitPoints, 0f);
            else
                Destroy(collider.gameObject);
        }
    }
}
