using System;

namespace LevelEditor
{
        public class EditorItemManager : UnitySingleton<EditorItemManager>, ITileManager
        {
                public static event Action<IFrameSelectable> OnTileSelected;

                public EditorTileSO[] EditorTiles;

                protected int _currentTrapIndex;

                private void Start()
                {
                        InitializeTraps();
                }

                protected void InitializeTraps()
                {
                        if (EditorTiles.Length == 0)
                        {
                                return;
                        }

                        foreach (var tile in EditorTiles)
                        {
                                tile.currentItemCount = tile.initialItemCount;
                        }

                        SetCurrentTrap(0);
                }

                public bool HasMultipleTraps()
                {
                        return EditorTiles.Length > 1;
                }

                public void NextTrap()
                {
                        SetCurrentTrap(_currentTrapIndex + 1);
                }

                public void PreviousTrap()
                {
                        SetCurrentTrap(_currentTrapIndex - 1);
                }

                public EditorTileSO GetCurrentTrap()
                {
                        return EditorTiles[_currentTrapIndex];
                }

                protected void SetCurrentTrap(int index)
                {
                        _currentTrapIndex = (index + EditorTiles.Length) % EditorTiles.Length;
                        OnTileSelected?.Invoke(EditorTiles[_currentTrapIndex]);
                }

                private void OnEnable()
                {
                        // @todo listen for level start event.
                }

                private void OnDisable()
                {
                        // @todo listen for level start event.
                }

                public void ResetTraps()
                {
                        InitializeTraps();
                }

                public void OnLevelStart()
                {
                        InitializeTraps();
                }


        }
        public interface ITileManager
        {

                public void NextTrap();

                public void PreviousTrap();


                public bool HasMultipleTraps();

        }
}
