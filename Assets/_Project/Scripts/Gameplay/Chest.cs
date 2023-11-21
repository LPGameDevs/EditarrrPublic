using Editarrr.Misc;
using Player;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Gameplay
{
    public class Chest : MonoBehaviour, ISpecialTrigger
    {
        public static event Action OnChestOpened;

        [SerializeField] AudioClip winSound;

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

        public void Trigger(Transform transform)
        {
            if (_isWon || !_isOpen || !transform.TryGetComponent<PlayerController>(out PlayerController player))
            {
                return;
            }

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
            Key.OnKeyCountChanged += ManageKeys;
        }

        private void OnDisable()
        {
            Key.OnKeyCountChanged -= ManageKeys;
        }
    }
}
