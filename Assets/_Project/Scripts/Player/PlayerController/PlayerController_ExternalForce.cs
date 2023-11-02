using Editarrr.Level.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        private PlayerForceReceiver ForceReceiver { get; set; }

        private Vector3 ExternalForce { get; set; }
        private Vector3 KnockbackForce { get; set; }


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

            if (!this.ForceReceiver.ForcedMove.HasValue)
                return;

            var externalForce = this.ForceReceiver.ForcedMove.Value;

            if (externalForce.x != 0)
            {
                this.HorizontalSpeed = this.HorizontalSpeed + externalForce.x;

                if (externalForce.x > 0 && this.HorizontalSpeed > externalForce.x)
                    this.HorizontalSpeed = externalForce.x;
                else if (externalForce.x < 0 && this.HorizontalSpeed < -externalForce.x)
                    this.HorizontalSpeed = externalForce.x;
            }

            //    //Overwrite movement with external force if one is being applied, pre-collision adjustment
            //    if (_forceReceiver.ForcedMove.HasValue)
            //    {
            //        _currentVerticalSpeed = _forceReceiver.ForcedMove.Value.y;
            //    }

            if (externalForce.y != 0)
            {
                this.VerticalSpeed = externalForce.y;
            }
        }
    }
}
