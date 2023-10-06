using System;
using UnityEngine;

namespace Gameplay
{
    public class Chest : MonoBehaviour
    {
        public static event Action OnChestOpened;

        [SerializeField] AudioClip winSound;

        private Animator _animator;

        private bool _isWon;
        private bool _isOpen;
        private static readonly int Open = Animator.StringToHash("Open");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isWon || !_isOpen)
            {
                return;
            }

            // The Chest should be on the Chest layer and the Player should be on the player layer.
            // The Chest should ONLY be allowed to collide with the Player and so no further checks
            // are required.
            // @todo Check this by (for example) letting an Enemy run into the Chest.
            _isWon = true;
            Editarrr.Audio.AudioManager.Instance.PlayAudioClip(winSound);
            OnChestOpened?.Invoke();
        }

        public void SetOpen()
        {
            if (_isOpen)
            {
                return;
            }

            _animator.SetTrigger(Open);
            _isOpen = true;
        }

        private void OnEnable()
        {
            Key.OnKeyPickup += SetOpen;
        }

        private void OnDisable()
        {
            Key.OnKeyPickup -= SetOpen;
        }
    }
}
