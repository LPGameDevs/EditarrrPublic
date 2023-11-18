using System;
using UnityEngine;

namespace Editarrr.Misc
{
    public class ProxyCollider : MonoBehaviour
    {
        [field: SerializeField] public Transform Proxy { get; private set; }

        public Action<Collider> OnTriggerEnterEvent { get; set; }
        public Action<Collider> OnTriggerStayEvent { get; set; }
        public Action<Collider> OnTriggerExitEvent { get; set; }

        public Action<Collision> OnCollisionEnterEvent { get; set; }
        public Action<Collision> OnCollisionStayEvent { get; set; }
        public Action<Collision> OnCollisionExitEvent { get; set; }


        public Action<Collider2D> OnTriggerEnter2DEvent { get; set; }
        public Action<Collider2D> OnTriggerStay2DEvent { get; set; }
        public Action<Collider2D> OnTriggerExit2DEvent { get; set; }

        public Action<Collision2D> OnCollisionEnter2DEvent { get; set; }
        public Action<Collision2D> OnCollisionStay2DEvent { get; set; }
        public Action<Collision2D> OnCollisionExit2DEvent { get; set; }


        public void SetProxy(Transform transform)
        {
            this.Proxy = transform;

            this.OnTriggerEnterEvent = null;
            this.OnTriggerStayEvent = null;
            this.OnTriggerExitEvent = null;

            this.OnCollisionEnterEvent = null;
            this.OnCollisionStayEvent = null;
            this.OnCollisionExitEvent = null;


            this.OnTriggerEnter2DEvent = null;
            this.OnTriggerStay2DEvent = null;
            this.OnTriggerExit2DEvent = null;

            this.OnCollisionEnter2DEvent = null;
            this.OnCollisionStay2DEvent = null;
            this.OnCollisionExit2DEvent = null;
        }

        #region 2D Trigger
        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.OnTriggerEnter2DEvent?.Invoke(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            this.OnTriggerStay2DEvent?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.OnTriggerExit2DEvent?.Invoke(collision);
        }
        #endregion

        #region 2D Collision
        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.OnCollisionEnter2DEvent?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            this.OnCollisionStay2DEvent?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            this.OnCollisionExit2DEvent?.Invoke(collision);
        }
        #endregion

        #region Trigger
        private void OnTriggerEnter(Collider other)
        {
            this.OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            this.OnTriggerStayEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            this.OnTriggerExitEvent?.Invoke(other);

        }
        #endregion

        #region Collision
        private void OnCollisionEnter(Collision other)
        {
            this.OnCollisionEnterEvent?.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            this.OnCollisionStayEvent?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            this.OnCollisionExitEvent?.Invoke(other);

        }
        #endregion

    }
}
