using Editarrr.LevelEditor;
using System;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class LeverBlock : MonoBehaviour, IConfigurable
    {
        [field: SerializeField] private Collider2D Collider { get; set; }
        [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }


        [field: SerializeField] private int Channel { get; set; }
        [field: SerializeField] private bool Inverted { get; set; }

        bool State { get; set; }

        private void Start()
        {
            this.State = this.Inverted;

            this.SetState(this.State);
        }

        private void OnEnable()
        {
            Lever.OnLeverSignal += this.Lever_OnLeverSignal;
        }

        private void OnDisable()
        {
            Lever.OnLeverSignal -= this.Lever_OnLeverSignal;
        }

        private void Lever_OnLeverSignal(int channel)
        {
            if (channel == this.Channel)
            {
                this.Toggle();
            }
        }

        private void Toggle()
        {
            this.SetState(!this.State);
        }

        private void SetState(bool state)
        {
            this.State = state;
            this.Collider.enabled = this.State;
            this.SpriteRenderer.color = this.State ? Color.white : Color.white * .5f;
        }


        public void Configure(TileConfig config)
        {
            this.Configure(config as LeverBlockConfig);
        }

        private void Configure(LeverBlockConfig config)
        {
            if (config == null)
                return;

            this.Channel = config.Channel;
            this.Inverted = config.Inverted;
        }
    }
}
