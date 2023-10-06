using System;
using Singletons;
using UnityEngine;

namespace Player
{
    public class HealthSystem : MonoBehaviour
    {
        private int _hitPoints = 0;
        [SerializeField, Tooltip("Maximum hit points")] private int _maxHitPoints = 20;
        [SerializeField, Tooltip("Initial hit points, limited to max hit points")] private int _startingHitPoints = 20;
        [SerializeField, Tooltip("If entity becomes invincible for a short time after taking damage")] private bool _hasDamageCooldown = true;
        [SerializeField, Range(0, 3f), Tooltip("How long entity is invincible after taking damage")] private float _baseDamageCooldown = 0.3f;
        private float _damageCooldownTimeRemaining = 0;

        public int MaxHitPoints { get => _maxHitPoints; }

        private void Awake()
        {
            _hitPoints = Mathf.Min(_maxHitPoints, _startingHitPoints);
        }

        public class OnHealthChangedArgs : OnValueChangedArgs<float> { public float stunDuration; };

        public static event EventHandler OnDeath;
        public static event EventHandler<OnValueChangedArgs<float>> OnInvincibleStarted;
        public static event EventHandler OnInvincibleEnded;
        public static event EventHandler<OnHealthChangedArgs> OnHitPointsChanged;

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

        public void TakeDamage(int damage, float stunDuration, float damageCooldown)
        {
            if (IsInvincible() || _hitPoints <= 0 || damage <= 0)
            {
                return;
            }

            int prevHitPoints = _hitPoints;
            _hitPoints -= damage;
            _hitPoints = Mathf.Max(_hitPoints, 0);

            if (_hitPoints < 1)
            {
                Die();
            }
            else
            {
                OnHitPointsChanged?.Invoke(this,
                    new OnHealthChangedArgs
                    {
                        previousValue = prevHitPoints,
                        value = _hitPoints,
                        stunDuration = stunDuration
                    });
            }



            if (_hasDamageCooldown)
            {
                _damageCooldownTimeRemaining = damageCooldown + _baseDamageCooldown; //Remains shortly after regaining character control, based on time spent disabled
                OnValueChangedArgs<float> invincibleArgs = new OnValueChangedArgs<float>();
                invincibleArgs.value = _damageCooldownTimeRemaining;
                OnInvincibleStarted?.Invoke(this, invincibleArgs);
            }
        }


        public void SetHitPoints(int amount)
        {
            int prevHitPoints = _hitPoints;
            _hitPoints = Mathf.Clamp(amount, 0, _maxHitPoints);
            OnHitPointsChanged?.Invoke(this, new OnHealthChangedArgs { previousValue = prevHitPoints, value = _hitPoints });
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
