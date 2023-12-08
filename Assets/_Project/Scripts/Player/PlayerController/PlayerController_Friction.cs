using System;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        [field: SerializeField, Header("FRICTION")] private float AirControlRate { get; set; } = 1f;

        IGroundFriction Ground { get; set; }
        float AccelerationBoost { get; set; }
        float DeAccelerationBoost { get; set; }
        float MaxSpeedBoost { get; set; }
        float CurrentSpeedBoost { get; set; }

        float LastGroundGrip { get; set; }
        float LastGroundFriction { get; set; }
        bool FrictionBoostActive { get; set; }

        private void UpdateFriction()
        {
            bool isGrounded = this.Collisions.Down;

            if (this.Ground != null)
            {
                this.AccelerationBoost = this.LastGroundGrip = this.Ground.Grip;
                this.DeAccelerationBoost = this.LastGroundFriction = this.Ground.Friction;
                this.MaxSpeedBoost = this.Ground.MaxSpeedBoost;
                this.FrictionBoostActive = true;
            }
            else if (isGrounded)
            {
                this.AccelerationBoost = 1f;
                this.LastGroundGrip = 1f;

                this.DeAccelerationBoost = 1f;
                this.LastGroundFriction = 1f;

                this.MaxSpeedBoost = 0f;
                this.CurrentSpeedBoost = 0f;

                this.FrictionBoostActive = false;
            }
            else
            {
                // this.FrictionAcceleration = this.FrictionAcceleration.Lerp(1, this.AirTime / 4f);
                // this.FrictionDeAcceleration = this.FrictionDeAcceleration.Lerp(1, this.AirTime / 4f);
                // this.MaxSpeedBoost = this.MaxSpeedBoost.Lerp(0, this.AirTime);
                if (this.FrictionBoostActive)
                {
                    this.AccelerationBoost = this.LastGroundGrip * this.AirControlRate;
                    this.DeAccelerationBoost = this.LastGroundFriction * this.AirControlRate;
                }

                int direction = this.RawInputMove.SignInt();
                int currentSpeedBoostDirection = this.CurrentSpeedBoost.SignInt();

                if (direction != currentSpeedBoostDirection)
                {
                    this.CurrentSpeedBoost = 0;
                }

                this.CurrentSpeedBoost = this.CurrentSpeedBoost.Lerp(this.MaxSpeedBoost * direction, this.TimeScale);
            }
        }
    }
}
