using Editarrr.Misc;
using System;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    [RequireComponent(typeof(Animator))]
    public class Geyser : MonoBehaviour, ISpecialTrigger
    {
        [field: SerializeField] private float Force { get; set; } = 100f;
        [field: SerializeField] private Transform LaunchOrigin { get; set; }

        [SerializeField] Animator geyserAnimator, fountainEffectAnimator;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _fountainEffectClip;

        const string ANIMATOR_TRIGGER_NAME = "Erupt";

        void ISpecialTrigger.Trigger(Transform transform)
        {
            if (!transform.TryGetComponent<IExternalForceReceiver>(out IExternalForceReceiver externalForceReceiver))
                return;

            this.Activate(externalForceReceiver);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<IExternalForceReceiver>(out IExternalForceReceiver externalForceReceiver))
                return;

            this.Activate(externalForceReceiver);
        }

        private void Activate(IExternalForceReceiver externalForceReceiver)
        {
            externalForceReceiver.CancelMovement();
            externalForceReceiver.SetPosition(this.LaunchOrigin.position);
            externalForceReceiver.ReceiveImpulse(this.Force, this.transform.up);
            geyserAnimator.SetTrigger(ANIMATOR_TRIGGER_NAME);
            _audioSource.PlayOneShot(_fountainEffectClip);
        }

        public void StartFountainEffect() => fountainEffectAnimator.SetTrigger(ANIMATOR_TRIGGER_NAME);
    }
}
