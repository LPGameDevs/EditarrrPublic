using System;
using UnityEngine;

namespace Player
{

    public partial class PlayerController
    {
        [field: SerializeField, Header("WALK")] private float Acceleration { get; set; } = 10f;
        [field: SerializeField] private float DeAcceleration { get; set; } = 7f;
        [field: SerializeField] private float MaxMoveSpeed { get; set; } = 13f;
        [field: SerializeField] private float JumpApexBonus { get; set; } = 2f;


        private float CurrentMaxMoveSpeed { get; set; }
        private float HorizontalSpeed { get; set; }

        private void UpdateWalk()
        {
            this.CurrentMaxMoveSpeed = this.MaxMoveSpeed + this.CurrentSpeedBoost.Abs();

            if (this.IsExternalForce)
            {
                this.ClampHorizontalSpeed(0);
                return;
            }

            float targetValue = 0;
            float rate = 0;
            float clampBonus = 0;

            if (this.IsMoving)
            {
                targetValue = this.RawInputMove * this.CurrentMaxMoveSpeed;
                rate = this.Acceleration * this.TimeScale * this.AccelerationBoost;
                clampBonus = Mathf.Sign(this.RawInputMove) * this.JumpApexInfluence * this.JumpApexBonus;
            }
            else
            {
                rate = this.DeAcceleration * this.TimeScale * this.DeAccelerationBoost;
                // this.ClampMoveSpeed = this.MaxMoveSpeed.Max(this.MaxMoveSpeed + Mathf.Abs(this.CurrentSpeedBoost));
            }

            this.CurrentMaxMoveSpeed = this.CurrentMaxMoveSpeed.Max(targetValue);

            this.UpdateHorizontalSpeed(targetValue, rate);
            this.ClampHorizontalSpeed(clampBonus);
        }

        private void UpdateHorizontalSpeed(float targetValue, float rate)
        {
            float t1 = this.HorizontalSpeed, t2 = targetValue;

            this.HorizontalSpeed = this.HorizontalSpeed.Lerp(targetValue, rate);

            // Debug.Log($"{t1} >> {t2} :: {this.HorizontalSpeed}");
        }

        private void ClampHorizontalSpeed(float clampBonus)
        {
            float clampSum = this.CurrentMaxMoveSpeed + clampBonus.Abs();

            this.HorizontalSpeed = this.HorizontalSpeed.Clamp(-clampSum, clampSum);
        }
    }
}
