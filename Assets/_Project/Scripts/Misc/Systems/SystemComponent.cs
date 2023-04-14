using Editarrr.Managers;
using UnityEngine;

namespace Editarrr.Systems
{
    public abstract class SystemComponent<T> : MonoBehaviour
        where T : ManagerComponent
    {
        [field: SerializeField, Header("Manager")] public T Manager { get; private set; }

        private void Awake()
        {
            this.PreAwake();
            this.Manager.DoAwake();
            this.OnAwake();
        }

        private void Start()
        {
            this.PreStart();
            this.Manager.DoStart();
            this.OnStart();
        }

        private void Update()
        {
            this.PreUpdate();
            this.Manager.DoUpdate();
            this.OnUpdate();
        }

        private void LateUpdate()
        {
            this.PreLateUpdate();
            this.Manager.DoLateUpdate();
            this.OnLateUpdate();
        }

        private void OnEnable()
        {
            this.PreOnEnable();
            this.Manager.DoOnEnable();
            this.OnOnEnable();
        }

        private void OnDisable()
        {
            this.PreOnDisable();
            this.Manager.DoOnDisable();
            this.OnOnDisable();
        }

        /// <summary>
        /// Is called, right before the Managers DoAwake is called.
        /// </summary>
        protected virtual void PreAwake() { }
        protected virtual void OnAwake() { }

        /// <summary>
        /// Is called, right before the Managers DoStart is called.
        /// </summary>
        protected virtual void PreStart() { }
        protected virtual void OnStart() { }

        /// <summary>
        /// Is called, right before the Managers DoUpdate is called.
        /// </summary>
        protected virtual void PreUpdate() { }
        protected virtual void OnUpdate() { }

        /// <summary>
        /// Is called, right before the Managers DoLateUpdate is called.
        /// </summary>
        protected virtual void PreLateUpdate() { }
        protected virtual void OnLateUpdate() { }

        protected virtual void PreOnEnable() { }
        protected virtual void OnOnEnable() { }

        protected virtual void PreOnDisable() { }
        protected virtual void OnOnDisable() { }
    }
}
