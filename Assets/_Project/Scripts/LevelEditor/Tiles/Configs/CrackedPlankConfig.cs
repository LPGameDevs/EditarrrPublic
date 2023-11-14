namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public class CrackedPlankConfig : TileConfig
    {
        public bool CanRespawn { get; private set; }


        public CrackedPlankConfig(bool canRespawn)
        {
            this.CanRespawn = canRespawn;
        }

        public CrackedPlankConfig(int[] data)
        {
            this.CanRespawn = data[0] == 1;
        }

        protected override int[] GetJSONData()
        {
            int[] toReturn = new int[]
            {
                this.CanRespawn ? 1 : 0
            };

            return toReturn;
        }

        public override void CreateGUIElements(GetElement getElement)
        {
            var channelElement = getElement("Can Respawn", this.CanRespawn);
            channelElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<bool>>(this.SetCanRespawn_Callback);
        }

        private void SetCanRespawn_Callback(UnityEngine.UIElements.ChangeEvent<bool> evt)
        {
            this.CanRespawn = evt.newValue;
        }


        public override TileConfig Clone()
        {
            return new CrackedPlankConfig(this.CanRespawn);
        }

        public override string ToString()
        {
            return $"Cracked Plank Config >> CR = {this.CanRespawn}";
        }
    }
}
