using Editarrr.Level.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        private PlayerForceReceiver ForceReceiver { get; set; }

        private Vector3 ExternalForce { get; set; }
        private Vector3 KnockbackForce { get; set; }

        bool IsKnockback { get; set; }
        bool IsExternalForce { get; set; }

        private void AwakeExternalForce()
        {
            this.ForceReceiver = this.GetComponent<PlayerForceReceiver>();
            this.ForceReceiver.OnPositionRequest += this.ForceReceiver_OnPositionRequest;
            this.ForceReceiver.OnCancelMovementRequest += this.ForceReceiver_OnCancelMovementRequest;
            this.ForceReceiver.OnForceStarted += this.ForceReceiver_OnForceStarted;
            this.ForceReceiver.OnForceEnded += this.ForceReceiver_OnForceEnded;
        }

        private void ForceReceiver_OnForceStarted(Vector3 force)
        {
            if (this.IsExternalForce) return;

            this.IsExternalForce = true;
        }

        private void ForceReceiver_OnForceEnded()
        {
            this.IsExternalForce = false;
        }

        private void UpdateExternalForce()
        {
            if (this.ForceReceiver.ForcedMove.HasValue)
                this.KnockbackForce = this.ForceReceiver.ForcedMove.Value;
            else
                this.KnockbackForce = Vector3.zero;

            this.IsKnockback = false;

            if (this.KnockbackForce.x.Abs() > float.Epsilon)
            {
                this.HorizontalSpeed = this.HorizontalSpeed + this.KnockbackForce.x;
                this.IsKnockback = true;
            }

            if (this.KnockbackForce.y.Abs() > float.Epsilon)
            {
                this.VerticalSpeed = this.KnockbackForce.y;
                this.IsKnockback = true;
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }

        private bool ForceReceiver_OnPositionRequest(Vector3 position)
        {
            this.SetPosition(position);

            return true;
        }

        private void ForceReceiver_OnCancelMovementRequest()
        {
            this.HorizontalSpeed = 0;
            this.VerticalSpeed = 0;
        }
    }
}
