using UnityEngine;

namespace Editarrr.LevelEditor
{
    public abstract class EditorTileConfigData : ScriptableObject
    {
        public abstract TileConfig CreateTileConfig();
    }


    public abstract class EditorTileConfigData<T> : EditorTileConfigData
        where T : TileConfig
    {
        public abstract T CreateConfig();

        public override TileConfig CreateTileConfig()
        {
            return this.CreateConfig();
        }
    }
}
