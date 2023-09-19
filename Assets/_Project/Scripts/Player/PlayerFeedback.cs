using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Player
{
    public class PlayerFeedback : MonoBehaviour
    {
        [SerializeField] private MMFeedbacks _move, _jump, _land, _damage, _death;

        private PlayerController _player;

        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            if (_player.IsMoving)
            {
                OnMove();
                return;
            }
        }

        public void OnMove()
        {
            _move.PlayFeedbacks();
        }

        public void OnJump()
        {
            _jump.PlayFeedbacks();
        }

        private void OnLand()
        {
            _land.PlayFeedbacks();
        }

        private void OnInvincible(object sender, OnValueChangedArgs<float> e)
        {
            MMFeedbackFlicker invulnFlicker;
            invulnFlicker = (MMFeedbackFlicker)_damage.Feedbacks.FindLast(x => x.GetType() == typeof(MMFeedbackFlicker));
            invulnFlicker.FlickerDuration = e.value - invulnFlicker.Timing.InitialDelay;
        }

        private void OnDamage(object sender, OnValueChangedArgs<float> e)
        {
            MMFeedbackCameraShake cameraShake;
            cameraShake = (MMFeedbackCameraShake)_damage.Feedbacks.Find(x => x.GetType() == typeof(MMFeedbackCameraShake));
            cameraShake.CameraShakeProperties.Amplitude = e.previousValue - e.value;

            _damage.PlayFeedbacks();
        }

        private void OnDeath(object sender, EventArgs e)
        {
            _death.PlayFeedbacks();
        }

        private void OnEnable()
        {
            PlayerController.OnPlayerJumped += OnJump;
            PlayerController.OnPlayerLanded += OnLand;
            HealthSystem.OnHitPointsChanged += OnDamage;
            HealthSystem.OnInvincibleStarted += OnInvincible;
            HealthSystem.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerJumped -= OnJump;
            PlayerController.OnPlayerLanded -= OnLand;
            HealthSystem.OnHitPointsChanged -= OnDamage;
            HealthSystem.OnInvincibleStarted -= OnInvincible;
            HealthSystem.OnDeath -= OnDeath;
        }
    }
}
