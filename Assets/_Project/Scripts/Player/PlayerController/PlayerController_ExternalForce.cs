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
        }

        private void UpdateExternalForce()
        {
            //    if (_forceReceiver.ForcedMove.HasValue)
            //    {
            //        _currentHorizontalSpeed = _currentHorizontalSpeed + _forceReceiver.ForcedMove.Value.x;

            //        if (_forceReceiver.ForcedMove.Value.x > 0 && _currentHorizontalSpeed > _forceReceiver.ForcedMove.Value.x)
            //            _currentHorizontalSpeed = _forceReceiver.ForcedMove.Value.x;
            //        else if (_forceReceiver.ForcedMove.Value.x < 0 && _currentHorizontalSpeed < -_forceReceiver.ForcedMove.Value.x)
            //            _currentHorizontalSpeed = _forceReceiver.ForcedMove.Value.x;
            //    }

            if (this.ForceReceiver.ForcedMove.HasValue)
                this.KnockbackForce = this.ForceReceiver.ForcedMove.Value;
            else
                this.KnockbackForce = Vector3.zero;

            // KnockbackForce > other external forces
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
            //else if (this.ExternalForce.x != 0)
            //{
            //    this.HorizontalSpeed += this.ExternalForce.x;
            //}

            //    //Overwrite movement with external force if one is being applied, pre-collision adjustment
            //    if (_forceReceiver.ForcedMove.HasValue)
            //    {
            //        _currentVerticalSpeed = _forceReceiver.ForcedMove.Value.y;
            //    }

            if (this.KnockbackForce.y.Abs() > float.Epsilon)
            {
                this.VerticalSpeed = this.KnockbackForce.y;
                this.IsKnockback = true;
            }
            //else if (this.ExternalForce.x != 0)
            //{
            //    this.VerticalSpeed += this.ExternalForce.y;
            //}
        }
    }
}
