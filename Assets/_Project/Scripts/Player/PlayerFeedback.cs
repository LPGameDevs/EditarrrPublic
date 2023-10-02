using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Audio;

namespace Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerFeedback : MonoBehaviour
    {
        [SerializeField] MMFeedbacks _move, _jump, _land, _damage, _death;
        [Space(15)]
        [SerializeField] AudioSource _generalAudioSource;
        [SerializeField] AudioSource _moveAudioSource;
        [Space(15)]
        [SerializeField] AudioClip spawnSFX;
        [Space(15)]
        [SerializeField] float _landingCamShakeExponent, _landingCamShakeMin, _landingCamShakeMax;
        [Space(15)]
        [SerializeField] float _damageCamShakeMultiplier, _damageCamShakeMin, _damageCamShakeMax;


        MMFeedbackSound _soundFeedback;
        MMFeedbackCameraShake _landingCameraShake;
        MMFeedbackCameraShake _damageCameraShake;
        MMSfxEvent.Delegate SoundDelegate => PlaySound;

        private void OnSpawn()
        {
            PlaySound(spawnSFX, 1f);
        }

        private void OnMove(bool movingOnGround)
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

        private void OnJump()
        {
            _jump.PlayFeedbacks();
        }

        private void OnLand(float timeInAir)
        {
            float newAmplitude = 0f;
            if (timeInAir >= 0.8f)
            {
                newAmplitude = Mathf.Pow(timeInAir, _landingCamShakeExponent);
                newAmplitude = Mathf.Clamp(newAmplitude, _landingCamShakeMin, _landingCamShakeMax);
            }

            AdjustCameraShake(_landingCameraShake, newAmplitude);
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
            float newAmplitude = (e.previousValue - e.value) * _damageCamShakeMultiplier;
            newAmplitude = Mathf.Clamp(newAmplitude, _damageCamShakeMin, _damageCamShakeMax);

            AdjustCameraShake(_damageCameraShake, newAmplitude);
            _damage.PlayFeedbacks();
        }

        private void OnDeath(object sender, EventArgs e)
        {
            _death.PlayFeedbacks();
        }

        private void PlaySound(AudioClip clipToPlay, float volume)
        {
            _generalAudioSource.PlayOneShot(clipToPlay, volume);
        }

        private void PlaySound(AudioClip clipToPlay, AudioMixerGroup audioGroup, float volume, float pitch)
        {
            _generalAudioSource.outputAudioMixerGroup = audioGroup;
            _generalAudioSource.pitch = pitch;
            _generalAudioSource.PlayOneShot(clipToPlay, volume);
        }

        private void AdjustCameraShake(MMFeedbackCameraShake shakeFeedback, float amplitudeValue) => shakeFeedback.CameraShakeProperties.Amplitude = amplitudeValue;

        private void Awake()
        {
            _landingCameraShake = (MMFeedbackCameraShake)_land.Feedbacks.Find(x => x.GetType() == typeof(MMFeedbackCameraShake));
            _damageCameraShake = (MMFeedbackCameraShake)_damage.Feedbacks.Find(x => x.GetType() == typeof(MMFeedbackCameraShake));
            OnSpawn();
        }

        private void OnEnable()
        {
            PlayerController.OnPlayerMoved += OnMove;
            PlayerController.OnPlayerJumped += OnJump;
            PlayerController.OnPlayerLanded += OnLand;
            HealthSystem.OnHitPointsChanged += OnDamage;
            HealthSystem.OnInvincibleStarted += OnInvincible;
            HealthSystem.OnDeath += OnDeath;
            MMSfxEvent.Register(PlaySound);
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerMoved -= OnMove;
            PlayerController.OnPlayerJumped -= OnJump;
            PlayerController.OnPlayerLanded -= OnLand;
            HealthSystem.OnHitPointsChanged -= OnDamage;
            HealthSystem.OnInvincibleStarted -= OnInvincible;
            HealthSystem.OnDeath -= OnDeath;
            MMSfxEvent.Unregister(PlaySound);
        }


    }
}
