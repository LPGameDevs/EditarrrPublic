using System;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public class LeverConfig : TileConfig, IChannelUser
    {
        public Action<int> OnChannelChanged { get; set; }

        public int Channel { get; private set; }
        
        public LeverConfig(int channel)
        {
            this.Channel = Mathf.Clamp(channel, 0, 9);
        }

        public LeverConfig(int[] data)
        {
            this.Channel = Mathf.Clamp(data[0], 0, 9);
        }

        protected override int[] GetJSONData()
        {
            int[] toReturn = new int[]
            {
                this.Channel
            };

            return toReturn;
        }

        public override void CreateGUIElements(GetElement getElement)
        {
            var channelElement = getElement("Channel", this.Channel);
            channelElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(this.SetChannel_Callback);
        }

        private void SetChannel_Callback(UnityEngine.UIElements.ChangeEvent<string> evt)
        {
            if (!int.TryParse(evt.newValue, out int value))
                return;

            this.Channel = value;
            this.OnChannelChanged?.Invoke(this.Channel);
        }

        public override TileConfig Clone()
        {
            return new LeverConfig(this.Channel);
        }

        public override string ToString()
        {
            return $"Lever Config >> C = {this.Channel}";
        }
    }
}
