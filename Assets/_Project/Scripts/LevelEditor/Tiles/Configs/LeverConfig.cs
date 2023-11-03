namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public class LeverConfig : TileConfig
    {
        public int Channel { get; private set; }


        public LeverConfig(int channel)
        {
            this.Channel = channel;
        }

        public LeverConfig(int[] data)
        {
            this.Channel = data[0];
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
        }

        public override string ToString()
        {
            return $"Lever Config >> C = {this.Channel}";
        }
    }
}
