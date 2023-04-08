using Editarrr.Managers;
using Editarrr.Misc;
using System;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "Editor Tile Selection Manager", menuName = "Managers/Editor/new Editor Tile Selection Manager")]
    public class EditorTileSelectionManager : ManagerComponent
    {
        public static Action<EditorTileGroupData> ActiveGroupChanged { get; set; }
        public static Action<EditorTileData> ActiveElementChanged { get; set; }

        [field: SerializeField] private EditorTileGroupDataPool GroupPool { get; set; }
        [field: SerializeField] public EditorTileGroupData ActiveGroup { get; private set; }

        [field: SerializeField] public EditorTileData ActiveElement { get; private set; }

        public int ActiveGroupIndex { get; private set; }
        public int ActiveElementIndex { get; private set; }
        public Rotation Rotation { get; private set; }

        public override void DoAwake()
        {
            this.ClearEvents();
            this.SetActiveGroupIndex(0);
        }

        private void ClearEvents()
        {
            EditorTileSelectionManager.ActiveGroupChanged = null;
            EditorTileSelectionManager.ActiveElementChanged = null;
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
        }


        public void NextGroup()
        {
            this.SetActiveGroupIndex(this.ActiveGroupIndex + 1);
        }

        public void NextRotation()
        {
            this.Rotation = (Rotation)(((int)this.Rotation + 1) % 4);
        }
    }
}
