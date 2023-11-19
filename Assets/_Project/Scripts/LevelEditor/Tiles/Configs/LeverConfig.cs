using UnityEngine;

namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public class LeverConfig : TileConfigOverlayEnabled
    {
        public int Channel { get; private set; }
        private Vector2 _tilePosition = new();
        
        public LeverConfig(int channel)
        {
            this.Channel = Mathf.Clamp(channel, 0, 9);
            this.OverlayTile = LeverBlockConfigData.LeverOverlayTiles[this.Channel];
        }

        public LeverConfig(int[] data)
        {
            this.Channel = Mathf.Clamp(data[0], 0, 9);
            this.OverlayTile = LeverBlockConfigData.LeverOverlayTiles[this.Channel];
        }

        protected override int[] GetJSONData()
        {
            int[] toReturn = new int[]
            {
                this.Channel
            };

            return toReturn;
        }

        public override void CreateGUIElements(GetElement getElement, Vector2 tilePosition)
        {
            var channelElement = getElement("Channel", this.Channel);
            _tilePosition = tilePosition;
            channelElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(this.SetChannel_Callback);
            channelElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(this.OverlayValueChanged);
        }

        private void SetChannel_Callback(UnityEngine.UIElements.ChangeEvent<string> evt)
        {
            if (!int.TryParse(evt.newValue, out int value))
                return;

            this.Channel = value;
            this.OverlayTile = LeverBlockConfigData.LeverOverlayTiles[this.Channel];
        }

        public override TileConfig Clone()
        {
            return new LeverConfig(this.Channel);
        }

        public override string ToString()
        {
            return $"Lever Config >> C = {this.Channel}";
        }

        private void OverlayValueChanged(UnityEngine.UIElements.ChangeEvent<string> evt)
        {
            TileConfigOverlayEnabled.RaiseOverlayValueChanged(this.OverlayTile, _tilePosition);
        }
    }
}
