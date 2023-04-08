using UnityEngine;

namespace Editarrr.Managers
{
    public abstract class ManagerComponent : ScriptableObject
    {
        public virtual void DoAwake() { }
        public virtual void DoStart() { }
        public virtual void DoUpdate() { }
        public virtual void DoLateUpdate() { }
    }
}
