using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    public interface IOverlayTile
    {
        public TileBase OverlayTile { get; protected set; }
    }
}
