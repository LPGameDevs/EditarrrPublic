using System;
using UnityEngine;

namespace Player
{
    public class HealthSystem : MonoBehaviour
    {

        private int _hitPoints = 0;
        [SerializeField, Tooltip("Maximum hit points")] private int _maxHitPoints = 20;
        [SerializeField, Tooltip("Initial hit points, limited to max hit points")] private int _startingHitPoints = 20;

        private void Awake()
        {
            _hitPoints = Mathf.Max(_maxHitPoints, _startingHitPoints);
        }

        public event EventHandler OnDeath;
        public event EventHandler OnHitPointsChanged;

        public void TakeDamage(int amount)
        {
            _hitPoints -= amount;
            OnHitPointsChanged?.Invoke(this, EventArgs.Empty);
            _hitPoints = Mathf.Max(_hitPoints, 0);
            if (_hitPoints == 0)
            {
                Die();
            }
        }

        public void SetHitPoints(int amount)
        {
            _hitPoints = Mathf.Clamp(amount, 0, _maxHitPoints);
            OnHitPointsChanged?.Invoke(this, EventArgs.Empty);
        }

        public float GetHealthNormalised()
        {
            return (float)(_hitPoints) / _maxHitPoints;
        }

        private void Die()
        {
            Debug.Log(gameObject.name + "has died");
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}
