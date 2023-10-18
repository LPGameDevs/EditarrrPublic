using System;
using System.Collections.Generic;

namespace Editarrr.LevelEditor
{
    [System.Serializable]
    public abstract class TileConfig
    {
        public abstract void CreateGUIElements(Func<object, UnityEngine.UIElements.VisualElement> getElement);

        public int[] CreateJSONData()
        {
            // Add something more?!?!
            int[] data = GetJSONData();

            return data;
        }

        protected abstract int[] GetJSONData();
    }
}
