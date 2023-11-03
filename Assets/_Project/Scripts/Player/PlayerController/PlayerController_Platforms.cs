using Editarrr.Level.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public partial class PlayerController : IMoveOnPlatform
    {
        public bool IsOnPlatform { get; private set; }
        List<Transform> PlatformQueue { get; set; } = new List<Transform>();
        Transform CurrentPlatform { get; set; }

        private void CalculatePlatform()
        {
            if (this.PlatformQueue.Count > 0)
            {
                this.CurrentPlatform = this.transform.parent = this.PlatformQueue[0];
            }
            else
            {
                this.CurrentPlatform = this.transform.parent = null;
            }

            this.IsOnPlatform = this.CurrentPlatform != null;
        }

        public void EnterPlatform(Transform transform)
        {
            Debug.Log("Enter Platform");
            this.PlatformQueue.Add(transform);
        }

        public void ExitPlatform(Transform transform)
        {
            Debug.Log("Exit Platform");
            this.PlatformQueue.Remove(transform);

            if (this.CurrentPlatform == transform)
            {
                Debug.Log("Clear Parent");
                this.CurrentPlatform = null;
            }
        }

    }
}
