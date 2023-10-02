using Editarrr.Input;
using Editarrr.Managers;
using Editarrr.Misc;
using Editarrr.UI.LevelEditor;
using System;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Selection Manager", menuName = "Managers/Editor/new Editor Tile Selection Manager")]
    public class EditorTileSelectionManager : ManagerComponent
    {
        private const string Documentation = "This manager is responsible for helping the user select tiles.\r\n"
            + "It responds to Input to provide shortcuts for editor tile selection.\r\n";
        public static Action<EditorTileGroupData> ActiveGroupChanged { get; set; }
        public static Action<EditorTileData> ActiveElementChanged { get; set; }
        public static Action<Rotation> RotationChanged { get; set; }

        [field: SerializeField, Info(Documentation)] private EditorTileGroupDataPool GroupPool { get; set; }
        public EditorTileGroupData ActiveGroup { get; private set; }

        [field: SerializeField, Header("Elements")] public EditorTileData DefaultElement { get; private set; }
        public EditorTileData ActiveElement { get; private set; }

        #region Input
        [field: SerializeField, Header("Input")] private InputValue Rotate { get; set; }
        [field: SerializeField] private InputValue SelectTile { get; set; }
        #endregion

        public int ActiveGroupIndex { get; private set; }
        public int ActiveElementIndex { get; private set; }
        public Rotation Rotation { get; private set; }
        public bool IsUIHover { get; private set; }
        public static TileSelect OnTileSelect { get; set; }
        public delegate void TileSelect();


        public override void DoAwake()
        {
            this.ClearEvents();
            this.SetActiveGroupIndex(0);

            // @todo This feature allows us to always use the same tile at the start
            // of level editing. If we want to track whatever the most recent tile was
            // just uncomment this line.
            ActiveElement = DefaultElement;
        }

        public override void DoStart()
        {
            // Register UI Events
            LevelEditorScreen.OnPointerEnter += this.LevelEditorScreen_OnPointerEnter;
            LevelEditorScreen.OnPointerLeave += this.LevelEditorScreen_OnPointerLeave;
        }

        public override void DoUpdate()
        {
            if (this.Rotate.WasPressed)
                this.NextRotation();

            if (this.SelectTile.WasPressed)
            {
                int index = (int)this.SelectTile.Read<float>();

                index = (index - 1).Loop(10);

                this.SetActiveElementIndex(index);
                OnTileSelect?.Invoke();
            }
        }

        private void LevelEditorScreen_OnPointerEnter()
        {
            this.IsUIHover = true;
        }

        private void LevelEditorScreen_OnPointerLeave()
        {
            this.IsUIHover = false;
        }

        private void ClearEvents()
        {
            EditorTileSelectionManager.ActiveGroupChanged = null;
            EditorTileSelectionManager.ActiveElementChanged = null;
            EditorTileSelectionManager.RotationChanged = null;
        }


        public void SetActiveGroupIndex(int index)
        {
            this.ActiveGroupIndex = index.Loop(this.GroupPool.GroupData.Length);
            this.ActiveGroup = this.GroupPool.Get(this.ActiveGroupIndex);

            EditorTileSelectionManager.ActiveGroupChanged?.Invoke(this.ActiveGroup);
        }

        public void SetActiveElementIndex(int index)
        {
            this.ActiveElementIndex = index.Loop(this.ActiveGroup.GroupElements.Length);

            this.ActiveElement = this.ActiveGroup.GroupElements[this.ActiveElementIndex];

            EditorTileSelectionManager.ActiveElementChanged?.Invoke(this.ActiveElement);

            this.SetRotation(Rotation.North);
        }


        public void NextGroup()
        {
            this.SetActiveGroupIndex(this.ActiveGroupIndex + 1);
        }

        public void NextRotation()
        {
            this.SetRotation((Rotation)(((int)this.Rotation - 1).Loop(4)));
        }

        private void SetRotation(Rotation rotation)
        {
            this.Rotation = rotation;
            EditorTileSelectionManager.RotationChanged?.Invoke(this.Rotation);
        }
    }
}
