using System;
using UnityEngine;

namespace Gameplay
{
    public class Key : MonoBehaviour
    {
        public static event Action OnKeyPickup;

        [SerializeField] AudioClip pickupSound;
        private bool _isCollected;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isCollected)
            {
                return;
            }

            // The Chest should be on the Chest layer and the Player should be on the player layer.
            // The Chest should ONLY be allowed to collide with the Player and so no further checks
            // are required.
            // @todo Check this by (for example) letting an Enemy run into the Chest.
            _isCollected = true;
            Editarrr.Audio.AudioManager.Instance.PlayAudioClip(pickupSound);
            this.gameObject.SetActive(false);
            OnKeyPickup?.Invoke();
        }
    }
}
