using Editarrr.Audio;
using Editarrr.LevelEditor;
using Player;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class Lever : MonoBehaviour, IConfigurable
    {
        public static LeverSignal OnLeverSignal { get; set; }
        public delegate void LeverSignal(int channel);

        [field: SerializeField] private int Channel { get; set; }
        [field: SerializeField] private bool IsActivated { get; set; }

        [SerializeField] Animator _animator;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _activateClip, _deactivateClip;

        const string ANIMATOR_ACTIVATE_NAME = "Activate";
        const string ANIMATOR_DEACTIVATE_NAME = "Deactivate";

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only triggers for player, We can add other conditions though (i.e. Enemies etc.)
            if (!collision.transform.TryGetComponent<PlayerController>(out PlayerController playerController))
                return;

            if(IsActivated)
            {
                _audioSource.PlayOneShot(_deactivateClip);
                _animator.SetTrigger(ANIMATOR_DEACTIVATE_NAME);
            }
            else
            {
                _audioSource.PlayOneShot(_activateClip);
                _animator.SetTrigger(ANIMATOR_ACTIVATE_NAME);
            }

            IsActivated = !IsActivated;
            Lever.OnLeverSignal?.Invoke(this.Channel);
        }


        public void Configure(TileConfig config)
        {
            this.Configure(config as LeverConfig);
        }

        private void Configure(LeverConfig config)
        {
            if (config == null)
                return;

            this.Channel = config.Channel;
        }
    }
}
