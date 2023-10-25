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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only triggers for player, We can add other conditions though (i.e. Enemies etc.)
            if (!collision.transform.TryGetComponent<PlayerController>(out PlayerController playerController))
                return;

            Debug.Log("Player At Lever");
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
