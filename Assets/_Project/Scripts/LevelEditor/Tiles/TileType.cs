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
        Sharcus = 6,
        Patrick = 7,
        Clawdia = 8,
        SpikeDeath = 10,
        Key = 20,
        BoxBounce = 30,
        Geyser = 40,
        CrackedPlank = 50,
        MovingPlatform = 60,
        MovingPlatformDeath = 61,
        PowerSword = 70,
        PowerDoubleJump = 71,
        PowerWallJump = 72,
        Background = 100,
    }
    // DO NOT change the Int values! This will mess with unity assets
}
