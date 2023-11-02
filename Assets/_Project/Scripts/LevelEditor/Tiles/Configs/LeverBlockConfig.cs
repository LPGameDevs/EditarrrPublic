namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public class LeverBlockConfig : TileConfig
    {
        public int Channel { get; private set; }
        public bool Inverted { get; private set; }


        public LeverBlockConfig(int channel, bool inverted)
        {
            this.Channel = channel;
            this.Inverted = inverted;
        }

        public LeverBlockConfig(int[] data)
        {
            this.Channel = data[0];
            this.Inverted = data[1] == 1 ? true : false;
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

        public override void CreateGUIElements(GetElement getElement)
        {
            var channelElement = getElement("Channel", this.Channel);
            channelElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(this.SetChannel_Callback);

            var invertedElement = getElement("Start as inactive", this.Inverted);
            invertedElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<bool>>(this.SetInverted_Callback);

        }

        private void SetChannel_Callback(UnityEngine.UIElements.ChangeEvent<string> evt)
        {
            if (!int.TryParse(evt.newValue, out int value))
                return;

            this.Channel = value;
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
    }
}
