using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Editarrr.UI.LevelEditor
{
    public partial class LevelEditorScreen : UIElement
    {
        [field: SerializeField, Header("Components")] public TileSelectionComponent TileSelection { get; private set; }
        [field: SerializeField] public TileGroupSwapperComponent TileGroupSwapper { get; private set; }
        [field: SerializeField] public SelectionPreviewComponent SelectionPreview { get; private set; }

        private void Start()
        {
            this.TileSelection.Initialize(this, this.Document.rootVisualElement);
            this.TileGroupSwapper.Initialize(this, this.Document.rootVisualElement);
            this.SelectionPreview.Initialize(this, this.Document.rootVisualElement);
        }
    }
}
