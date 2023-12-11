using Editarrr.LevelEditor;
using Player;
using System;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class PackedIce : MonoBehaviour, IGroundFriction
    {
        [field: SerializeField] public float Friction { get; private set; } = 0.01f;
        [field: SerializeField] public float Grip { get; private set; } = 0.1f;
        [field: SerializeField] public float MaxSpeedBoost { get; private set; } = 5f;
        
    }
}
