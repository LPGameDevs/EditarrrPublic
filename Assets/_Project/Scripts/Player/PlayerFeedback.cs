using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Player
{
    public class PlayerFeedback : MonoBehaviour
    {
        [SerializeField] private MMFeedbacks _move, _jump, _land, _damage, _death;

        private PlayerController _player;
        private HealthSystem _health;

        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
            _health = GetComponentInParent<HealthSystem>();
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

        private void OnDamage(object sender, EventArgs e)
        {
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
            _health.OnHitPointsChanged += OnDamage;
            _health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerJumped -= OnJump;
            PlayerController.OnPlayerLanded -= OnLand;
            _health.OnHitPointsChanged -= OnDamage;
            _health.OnDeath -= OnDeath;
        }
    }
}
