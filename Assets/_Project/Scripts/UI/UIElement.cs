using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editarrr.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        [field: SerializeField] public UIDocument Document { get; private set; }

        public Action OnUpdate { get; set; }

        public void Register(Action action)
        {
            this.OnUpdate += action;
        }


        protected virtual void Update()
        {
            this.OnUpdate?.Invoke();
        }
    }
}
