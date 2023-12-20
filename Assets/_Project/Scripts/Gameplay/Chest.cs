using Editarrr.Misc;
using Player;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Gameplay
{
    public class Chest : MonoBehaviour, ISpecialTrigger
    {
        public static event Action OnChestReached;

        [SerializeField] AudioClip _winSound;
        [SerializeField] Collider2D _collider;

        private Animator _animator;

        private int _remainingKeys;
        private bool _isWon;
        private bool _isOpen;
        private static readonly int Open = Animator.StringToHash("Open");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            Invoke(nameof(InitializeState), 0.5f);
        }

        private void InitializeState()
        {
            ManageKeys();
        }

        private void ManageKeys(int countChange = 0)
        {
            _remainingKeys += countChange;

            if (_remainingKeys == 0)
                SetOpen();
        }

        public void SetOpen()
        {
            if (_isOpen)
            {
                return;
            }

            _collider.enabled = true;
            _animator.SetTrigger(Open);
            _isOpen = true;
        }


        public void Trigger(Transform transform)
        {
            if (_isWon || !_isOpen)
            {
                return;
            }

            _isWon = true;
            Editarrr.Audio.AudioManager.Instance.PlayAudioClip(_winSound);
            OnChestReached?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Trigger(collision.transform);
        }

        private void OnEnable()
        {
            Key.OnKeyCountChanged += ManageKeys;
        }

        private void OnDisable()
        {
            Key.OnKeyCountChanged -= ManageKeys;
        }
    }
}
