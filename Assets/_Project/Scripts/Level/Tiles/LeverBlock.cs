using Editarrr.LevelEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class LeverBlock : MonoBehaviour, IConfigurable
    {
        [field: SerializeField] private Collider2D Collider { get; set; }
        [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }


        [field: SerializeField] private int Channel { get; set; }
        [field: SerializeField] private bool Inverted { get; set; }

        private readonly List<string> _channelColors = new() 
        { 
            "#ffffff", "#639d6d", "#eed878", "#f18770", "#e39bba", "#505db3", "#de9970"
        };

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

            SetChannelColor();
        }

        private void SetChannelColor()
        {
            var index = this.Channel;
            
            if (index >= _channelColors.Count)
                index -= _channelColors.Count;

            var colorString = _channelColors[index];

            ColorUtility.TryParseHtmlString(colorString, out Color outlineColor);

            SpriteRenderer.material.color = outlineColor;
        }
    }
}
