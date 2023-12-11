using Editarrr.Level.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public partial class PlayerController : IMoveOnPlatform
    {
        public bool IsOnPlatform { get; private set; }
        List<IExternalMovementSource> PlatformQueue { get; set; } = new List<IExternalMovementSource>();
        IExternalMovementSource CurrentPlatform { get; set; }

        private void UpdatePlatform()
        {
            if (this.PlatformQueue.Count > 0)
            {
                this.CurrentPlatform = this.PlatformQueue[0];
            }
            else
            {
                this.CurrentPlatform = null;
            }

            this.IsOnPlatform = this.CurrentPlatform != null;

            if (this.IsOnPlatform)
            {
                this.ExternalForce += this.CurrentPlatform.Delta;
            }
            else
            {
                this.ExternalForce = Vector3.zero;
            }
        }

        public void EnterPlatform(IExternalMovementSource externalMovementSource)
        {
            // Debug.Log("Enter Platform");
            this.PlatformQueue.Add(externalMovementSource);
        }

        public void ExitPlatform(IExternalMovementSource externalMovementSource)
        {
            // Debug.Log("Exit Platform");
            this.PlatformQueue.Remove(externalMovementSource);

            if (this.CurrentPlatform == externalMovementSource)
            {
                // Debug.Log("Clear Parent");
                this.CurrentPlatform = null;
            }
        }

    }
}
