using System;

namespace Editarrr.LevelEditor
{
    [Serializable]
    public enum TileType
    {
        Empty = 1,
        Wall = 2,
        Spike = 3,
        Player = 4,
        Goal = 5,
        Enemy_001 = 6,
        Enemy_002 = 7
    }
    // DO NOT change the Int values! This will mess with unity assets
}
