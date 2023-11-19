using Editarrr.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editarrr.LevelEditor
{
    public class EditorLevelChannelLineManager : ManagerComponent
    {
        #region Fields and Properties

        [SerializeField] private TileBase _leverBlockTile;
        [SerializeField] private TileBase _leverTile;

        #endregion

        #region Methods

        public override void DoOnEnable()
        {
            EditorLevelManager.OnTileSet += DrawLines;
            EditorLevelManager.OnTileUnset += DrawLines;
        }

        public override void DoOnDisable()
        {
            EditorLevelManager.OnTileSet -= DrawLines;
            EditorLevelManager.OnTileUnset -= DrawLines;
        }

        private void DrawLines(EditorTileData data, TileType tileType, int inLevel)
        {

        }


        #endregion
    }
}
