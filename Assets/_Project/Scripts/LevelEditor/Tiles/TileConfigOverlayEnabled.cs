using Editarrr.LevelEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    public abstract class TileConfigOverlayEnabled : TileConfig
    {
        public TileBase OverlayTile { get; protected set; }

        public static event Action<TileBase, Vector2> OnOverlayValueChanged;

        public override void CreateGUIElements(GetElement getElement)
        {
            //nothing to do here
        }

        public override void CreateGUIElements(GetElement getElement, Vector2 tilePosition)
        {
            //nothing to do here only in child overrides
        }

        public static void RaiseOverlayValueChanged(TileBase overlayTile, Vector2 tilePosition)
        {
            OnOverlayValueChanged?.Invoke(overlayTile, tilePosition);
        }
    }
}
