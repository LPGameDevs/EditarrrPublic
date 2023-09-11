using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Audio;

namespace Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerFeedback : MonoBehaviour
    {
        [SerializeField] private MMFeedbacks _move, _jump, _land, _damage, _death;
        [SerializeField] private AudioSource _generalAudioSource, _moveAudioSource;

        float _moveSfxDuration, _moveSfxTimer;
        MMFeedbackSound soundFeedback;
        MMSfxEvent.Delegate SoundDelegate => PlaySound;

        public void OnMove(bool movingOnGround)
        {
            //if (isMoving)
            //    _move.PlayFeedbacks();
            //else
            //    _move.StopFeedbacks();

            if (!_moveAudioSource.isPlaying && movingOnGround)
                _moveAudioSource.Play();
            else if (_moveAudioSource.isPlaying && !movingOnGround)
                _moveAudioSource.Stop();
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
            PlayerController.OnPlayerMoved += OnMove;
            PlayerController.OnPlayerJumped += OnJump;
            PlayerController.OnPlayerLanded += OnLand;
            HealthSystem.OnHitPointsChanged += OnDamage;
            HealthSystem.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerMoved -= OnMove;
            PlayerController.OnPlayerJumped -= OnJump;
            PlayerController.OnPlayerLanded -= OnLand;
            HealthSystem.OnHitPointsChanged -= OnDamage;
            HealthSystem.OnDeath -= OnDeath;
        }

        private void PlaySound(AudioClip clipToPlay, AudioMixerGroup audioGroup, float volume, float pitch)
        {
            _generalAudioSource.outputAudioMixerGroup = audioGroup;
            _generalAudioSource.pitch = pitch;
            _generalAudioSource.PlayOneShot(clipToPlay, volume);
        }
    }
}
