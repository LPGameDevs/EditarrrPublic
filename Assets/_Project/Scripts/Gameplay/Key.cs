using Player;
using System;
using UnityEngine;

namespace Gameplay
{
    public class Key : MonoBehaviour, Editarrr.Misc.ISpecialTrigger
    {
        public static event Action<int> OnKeyCountChanged;

        [SerializeField] AudioClip pickupSound, bootySound;
        [SerializeField] Animator animator;
        [SerializeField] Collider2D collider;
        private bool _isCollected;

        const string PICKUP_TRIGGER_NAME = "Pickup";

        private void Start()
        {
            OnKeyCountChanged?.Invoke(+1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isCollected)
                return;

            PickUp();
        }

        public void FinishPickup()
        {
            if(transform.parent != null)
                transform.parent.gameObject.SetActive(false);
            else
                gameObject.SetActive(false);
        }

        public void Trigger(Transform transform)
        {
            if (!transform.TryGetComponent<PlayerController>(out PlayerController player))
                return;

            PickUp();
        }

        private void PickUp()
        {
            collider.enabled = false;
            _isCollected = true;
            Editarrr.Audio.AudioManager.Instance.PlayRandomizedAudioClip(pickupSound.name, 0.05f, 0);
            Editarrr.Audio.AudioManager.Instance.PlayRandomizedAudioClip(bootySound.name, 0.05f, 0.1f);
            animator.SetTrigger(PICKUP_TRIGGER_NAME);
            OnKeyCountChanged?.Invoke(-1);
        }
    }
}
