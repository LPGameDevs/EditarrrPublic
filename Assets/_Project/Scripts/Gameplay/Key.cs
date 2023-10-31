using System;
using UnityEngine;

namespace Gameplay
{
    public class Key : MonoBehaviour
    {
        public static event Action<int> OnKeyCountChanged;

        [SerializeField] AudioClip pickupSound, bootySound;
        [SerializeField] Animator animator;
        private bool _isCollected;

        const string PICKUP_TRIGGER_NAME = "Pickup";

        private void Awake()
        {
            OnKeyCountChanged?.Invoke(+1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isCollected)
            {
                return;
            }

            _isCollected = true;
            Editarrr.Audio.AudioManager.Instance.PlayRandomizedAudioClip(pickupSound.name, 0.05f, 0);
            Editarrr.Audio.AudioManager.Instance.PlayRandomizedAudioClip(bootySound.name, 0.05f, 0.1f);
            animator.SetTrigger(PICKUP_TRIGGER_NAME);
            OnKeyCountChanged?.Invoke(-1);
        }

        public void FinishPickup()
        {
            if(transform.parent != null)
                transform.parent.gameObject.SetActive(false);
            else
                gameObject.SetActive(false);
        }
    }
}
