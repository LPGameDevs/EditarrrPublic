using System;
using System.Collections.Generic;

namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public class MovingPlatformConfig : TileConfig
    {
        public int Distance { get; private set; }
        public bool MoveRight { get; private set; }


        public MovingPlatformConfig(int distance, bool moveRight)
        {
            this.Distance = distance;
            this.MoveRight = moveRight;
        }

        public MovingPlatformConfig(int[] data)
        {
            this.Distance = data[0];
            this.MoveRight = data[1] == 1 ? true : false;
        }

        protected override int[] GetJSONData()
        {
            int[] toReturn = new int[]
            {
                this.Distance,
                this.MoveRight ? 1 : 0
            };

            return toReturn;
        }

        // public override void CreateGUIElements(Func<object, UnityEngine.UIElements.VisualElement> getElement)
        public override void CreateGUIElements(GetElement getElement)
        {
            var directionElement = getElement("Start moving right", this.MoveRight);
            directionElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<bool>>(this.SetDirection_Callback);

            var distanceElement = getElement("Distance", this.Distance);
            distanceElement.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(this.SetDistance_Callback);
        }

        private void SetDirection_Callback(UnityEngine.UIElements.ChangeEvent<bool> evt)
        {
            this.MoveRight = evt.newValue;
        }
        
        private void SetDistance_Callback(UnityEngine.UIElements.ChangeEvent<string> evt)
        {
            if (!int.TryParse(evt.newValue, out int value))
                return;

            this.Distance = value;
        }


        public override TileConfig Clone()
        {
            return new MovingPlatformConfig(this.Distance, this.MoveRight);
        }

        public override string ToString()
        {
            return $"Moving Platform Config >> D = {this.Distance}, DIR = {this.MoveRight}";
        }
    }
}
