using System;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        IGroundFriction Ground { get; set; }
        float FrictionAcceleration { get; set; }
        float FrictionDeAcceleration { get; set; }
        float MaxSpeedBoost { get; set; }

        private void UpdateFriction()
        {
            bool isGrounded = this.Collisions.Down;

            if (this.Ground != null)
            {
                this.FrictionAcceleration = this.Ground.Grip;
                this.FrictionDeAcceleration = this.Ground.Friction;
                this.MaxSpeedBoost = this.Ground.MaxSpeedBoost;
            }
            else if (isGrounded)
            {
                this.FrictionAcceleration = 1f;
                this.FrictionDeAcceleration = 1f;
                this.MaxSpeedBoost = 0;
            }
            else
            {
                this.FrictionAcceleration = this.FrictionAcceleration.Lerp(1, this.AirTime);
                this.FrictionDeAcceleration = this.FrictionDeAcceleration.Lerp(1, this.AirTime);
                this.MaxSpeedBoost = this.MaxSpeedBoost.Lerp(0, this.AirTime);
            }
        }
    }
}
