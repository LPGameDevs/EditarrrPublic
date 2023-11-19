
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public class LeverBlockConfig : TileConfigOverlayEnabled
    {
        public int Channel { get; private set; }
        public bool Inverted { get; private set; }
        private Vector2 _tilePosition = new();

        public LeverBlockConfig(int channel, bool inverted)
        {
            this.Channel = Mathf.Clamp(channel, 0, 9);
            this.Inverted = inverted;
            this.OverlayTile = LeverBlockConfigData.LeverOverlayTiles[this.Channel];
        }

        public LeverBlockConfig(int[] data)
        {
            this.Channel = Mathf.Clamp(data[0], 0, 9);
            this.Inverted = data[1] == 1;
            this.OverlayTile = LeverBlockConfigData.LeverOverlayTiles[this.Channel];
        }

        protected override int[] GetJSONData()
        {
            int[] toReturn = new int[]
            {
                this.Channel,
                this.Inverted ? 1 : 0
            };

            return toReturn;
        }

        public override void CreateGUIElements(GetElement getElement, Vector2 tilePosition)
        {
            var channelElement = getElement("Channel", this.Channel);
            _tilePosition = tilePosition;
            channelElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(this.SetChannel_Callback);
            channelElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(this.OverlayValueChanged);

            var invertedElement = getElement("Start as active", this.Inverted);
            invertedElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<bool>>(this.SetInverted_Callback);
        }

        private void SetChannel_Callback(UnityEngine.UIElements.ChangeEvent<string> evt)
        {
            if (!int.TryParse(evt.newValue, out int value))
                return;

            this.Channel = Mathf.Clamp(value, 0, 9);
            this.OverlayTile = LeverBlockConfigData.LeverOverlayTiles[this.Channel];
        }

        private void SetInverted_Callback(UnityEngine.UIElements.ChangeEvent<bool> evt)
        {
            this.Inverted = evt.newValue;
        }

        public override TileConfig Clone()
        {
            return new LeverBlockConfig(this.Channel, this.Inverted);
        }

        public override string ToString()
        {
            return $"Lever Block Config >> C = {this.Channel}, I = {this.Inverted}";
        }

        private void OverlayValueChanged(UnityEngine.UIElements.ChangeEvent<string> evt)
        {
            TileConfigOverlayEnabled.RaiseOverlayValueChanged(this.OverlayTile, _tilePosition);
        }
    }
}
