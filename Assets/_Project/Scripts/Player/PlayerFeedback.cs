using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Audio;

namespace Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerFeedback : MonoBehaviour
    {
        [SerializeField] MMFeedbacks _move, _jump, _softLanding, _hardLanding, _damage, _death;
        [Space(15)]
        [SerializeField] AudioSource _generalAudioSource;
        [SerializeField] AudioSource _moveAudioSource;
        [Space(15)]
        [SerializeField] AudioClip _spawnSFX;
        [Space(15)]
        [SerializeField] float _landingCamShakeExponent, _landingCamShakeMin, _landingCamShakeMax;
        [Space(15)]
        [SerializeField] float _damageCamShakeMultiplier, _damageCamShakeMin, _damageCamShakeMax;


        MMFeedbackSound _soundFeedback;
        MMFeedbackCameraShake _landingCameraShake;
        MMFeedbackCameraShake _damageCameraShake;
        MMSfxEvent.Delegate SoundDelegate => PlaySound;

        bool _playerIsFalling;
        float _fallTime = 0f;

        private void OnSpawn()
        {
            PlaySound(_spawnSFX, 1f);
        }

        private void OnMove(bool movingOnGround)
        {
            //if (isMoving)
            //    _move.PlayFeedbacks();
            //else
            //    _move.StopFeedbacks();

            if(movingOnGround)
            {
                if (!_moveAudioSource.isPlaying)
                    _moveAudioSource.Play();

                _move.PlayFeedbacks();
            }
            else
            {
                if (_moveAudioSource.isPlaying)
                    _moveAudioSource.Stop();

                _move.StopFeedbacks();
            }
        }

        private void OnJump()
        {
            _jump.PlayFeedbacks();
        }

        private void OnStartedFalling()
        {
            _playerIsFalling = true;
        }

        private void OnLand(float verticalSpeed)
        {
            float newAmplitude = 0f;

            if (verticalSpeed <= -30f)
            {
                //newAmplitude = Mathf.Pow(_fallTime, _landingCamShakeExponent);
                //newAmplitude = Mathf.Clamp(newAmplitude, _landingCamShakeMin, _landingCamShakeMax);
                //AdjustCameraShake(_landingCameraShake, newAmplitude);

                _fallTime = Mathf.Clamp(_fallTime, 0.5f, 2f);
                _hardLanding.transform.localScale = new Vector2(_fallTime, _fallTime);
                _hardLanding.PlayFeedbacks(_hardLanding.transform.position, _fallTime);
            }
            else
                _softLanding.PlayFeedbacks();

            _playerIsFalling = false;
            _fallTime = 0f;
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
            _landingCameraShake = (MMFeedbackCameraShake)_hardLanding.Feedbacks.Find(x => x.GetType() == typeof(MMFeedbackCameraShake));
            _damageCameraShake = (MMFeedbackCameraShake)_damage.Feedbacks.Find(x => x.GetType() == typeof(MMFeedbackCameraShake));
            OnSpawn();
        }

        private void Update()
        {
            if(_playerIsFalling)
            {
                _fallTime += Time.deltaTime;
            }
        }

        private void OnEnable()
        {
            PlayerController.OnPlayerMoved += OnMove;
            PlayerController.OnPlayerJumped += OnJump;
            PlayerController.OnPlayerStartedFalling += OnStartedFalling;
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
            PlayerController.OnPlayerStartedFalling -= OnStartedFalling;
            PlayerController.OnPlayerLanded -= OnLand;
            HealthSystem.OnHitPointsChanged -= OnDamage;
            HealthSystem.OnInvincibleStarted -= OnInvincible;
            HealthSystem.OnDeath -= OnDeath;
            MMSfxEvent.Unregister(PlaySound);
        }


    }
}
