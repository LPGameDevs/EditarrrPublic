using UnityEngine.UIElements;

namespace Editarrr.UI
{
    public abstract class UIComponent
    {
        public abstract void Initialize(UIElement root, VisualElement visualElement);
    }
}
