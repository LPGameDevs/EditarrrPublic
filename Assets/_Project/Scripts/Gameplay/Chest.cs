using System;
using UnityEngine;

namespace Gameplay
{
    public class Chest : MonoBehaviour
    {
        public static event Action OnChestOpened;

        [SerializeField] AudioClip winSound;
        private bool _isOpen;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isOpen)
            {
                return;
            }

            // The Chest should be on the Chest layer and the Player should be on the player layer.
            // The Chest should ONLY be allowed to collide with the Player and so no further checks
            // are required.
            // @todo Check this by (for example) letting an Enemy run into the Chest.
            _isOpen = true;
            Editarrr.Audio.AudioManager.Instance.PlayAudioClip(winSound);
            OnChestOpened?.Invoke();
        }
    }
}
