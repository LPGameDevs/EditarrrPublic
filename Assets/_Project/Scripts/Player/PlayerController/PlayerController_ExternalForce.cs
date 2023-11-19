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


        private void AwakeExternalForce()
        {
            this.ForceReceiver = this.GetComponent<PlayerForceReceiver>();
            this.ForceReceiver.OnPositionRequest += this.ForceReceiver_OnPositionRequest;
            this.ForceReceiver.OnCancelMovementRequest += this.ForceReceiver_OnCancelMovementRequest;
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

                if (this.KnockbackForce.x > 0 && this.HorizontalSpeed > this.KnockbackForce.x)
                    this.HorizontalSpeed = this.KnockbackForce.x;
                else if (this.KnockbackForce.x < 0 && this.HorizontalSpeed < -this.KnockbackForce.x)
                    this.HorizontalSpeed = this.KnockbackForce.x;
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
