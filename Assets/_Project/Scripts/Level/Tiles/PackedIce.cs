using Editarrr.LevelEditor;
using Player;
using System;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class PackedIce : MonoBehaviour, IGroundFriction
    {
        [field: SerializeField] public float Friction { get; private set; } = 1f;
    }
}
