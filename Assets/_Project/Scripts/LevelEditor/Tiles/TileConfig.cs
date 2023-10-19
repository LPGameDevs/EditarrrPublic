using System;
using System.Collections.Generic;

namespace Editarrr.LevelEditor
{
    /// To create a new TileConfig for a custom tile type:
    /// 1. Start by adding a new class that represents your tile configuration.
    ///    Make sure it inherits from the base class 'TileConfig'.
    ///    Fill in your configuration class with all the necessary properties and methods.
    /// 
    /// 2. Next, create a 'ConfigData' class that derives from 'EditorTileConfigData<[YourTileConfigHere]>'.
    ///    This class is responsible for storing your custom tile configuration data as well as the default values for a tile.
    /// 
    /// 3. Finally, generate the actual asset for your new 'ConfigData' class by:
    ///    - Navigating to the '_Project/ScriptableObjects/Data/Editor/Config' location.
    ///    - Creating a new asset of your 'ConfigData' type, providing settings for your tile.
    ///    
    /// 4. Now, navigate to the '_Project/ScriptableObjects/Data/Editor/Tiles' directory.
    ///    Find the tile for which you created the Config.
    ///    Link your Config asset to this specific Tile by referencing it.

    [System.Serializable]
    public abstract class TileConfig
    {
        // public abstract void CreateGUIElements(Func<object, UnityEngine.UIElements.VisualElement> getElement);
        public abstract void CreateGUIElements(GetElement getElement);

        public int[] CreateJSONData()
        {
            // Add something more?!?!
            int[] data = GetJSONData();

            return data;
        }

        protected abstract int[] GetJSONData();
    }

    public delegate UnityEngine.UIElements.VisualElement GetElement(string title, object obj);
}
