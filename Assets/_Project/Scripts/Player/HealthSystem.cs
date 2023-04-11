using System;
using UnityEngine;

namespace Player
{
    public class HealthSystem : MonoBehaviour
    {

        private int _hitPoints = 0;
        [SerializeField, Tooltip("Maximum hit points")] private int _maxHitPoints = 20;
        [SerializeField, Tooltip("Initial hit points, limited to max hit points")] private int _startingHitPoints = 20;
        [SerializeField, Tooltip("If entity becomes invincible for a short time after taking damage")] private bool _hasDamageCooldown = true;
        [SerializeField, Range(0, 3f), Tooltip("How long entity is invincible after taking damage")] private float _damageCooldown = 0.3f;
        private float _damageCooldownTimeRemaining = 0;

        private void Awake()
        {
            _hitPoints = Mathf.Min(_maxHitPoints, _startingHitPoints);
        }

        public event EventHandler OnDeath;
        public event EventHandler OnInvincibleStarted;
        public event EventHandler OnInvincibleEnd;
        public event EventHandler OnHitPointsChanged;

        private void Update()
        {
            if (IsInvincible())
            {
                _damageCooldownTimeRemaining -= Time.deltaTime;
                if (_damageCooldownTimeRemaining <= 0)
                {
                    OnInvincibleEnded?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void TakeDamage(int amount)
        {
            if (IsInvincible())
            {
                return;
            }

            _hitPoints -= amount;
            _hitPoints = Mathf.Max(_hitPoints, 0);

            OnHitPointsChanged?.Invoke(this, _hitPoints);

            if (_hitPoints == 0)
            {
                Die();
            }
            else if (_hasDamageCooldown)
            {
                _damageCooldownTimeRemaining = _damageCooldown;
                OnInvincibleStarted?.Invoke(this.EventArgs.Empty);
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

        public bool IsDamageable()
        {
            return !_hasDamageCooldown || (_hasDamageCooldown && _damageCooldownTimeRemaining <= 0);
        }

        public bool IsInvincible()
        {
            return _hasDamageCooldown && _damageCooldownTimeRemaining > 0;
        }

        private void Die()
        {
            Debug.Log(gameObject.name + "has died");
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}
