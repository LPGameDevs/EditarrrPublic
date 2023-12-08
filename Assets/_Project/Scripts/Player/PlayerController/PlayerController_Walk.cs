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


        private float ClampMoveSpeed { get; set; }
        private float HorizontalSpeed { get; set; }

        private void UpdateWalk()
        {
            float targetValue;
            float rate;
            float clampBonus;

            if (this.IsMoving)
            {
                targetValue = this.RawInputMove * (this.MaxMoveSpeed + this.CurrentSpeedBoost.Abs());
                rate = this.Acceleration * this.TimeScale * this.AccelerationBoost;
                clampBonus = Mathf.Sign(this.RawInputMove) * this.JumpApexInfluence * this.JumpApexBonus;
            }
            else
            {
                targetValue = 0;
                rate = this.DeAcceleration * this.TimeScale * this.DeAccelerationBoost;
                clampBonus = 0;
            }

            this.ClampMoveSpeed = this.MaxMoveSpeed.Max(targetValue.Abs());

            this.UpdateHorizontalSpeed(targetValue, rate, clampBonus);
        }

        private void UpdateHorizontalSpeed(float targetValue, float rate, float clampBonus)
        {
            // Debug.Log($"HZ: {this.HorizontalSpeed}, T: {targetValue}, R: {rate}, C: {clampBonus}");
            Debug.Log($"{this.HorizontalSpeed} >>>> {targetValue}");
            // Debug.Log($"T: {targetValue}");

            this.HorizontalSpeed = this.HorizontalSpeed.Lerp(targetValue, rate);

            float clampSum = this.ClampMoveSpeed + clampBonus.Abs();

            this.HorizontalSpeed = this.HorizontalSpeed.Clamp(-clampSum, clampSum);
        }

        //[Header("WALKING")][SerializeField] private float _acceleration = 90;
        //[SerializeField, Tooltip("Maximum move speed")] private float _maxMoveSpeed = 13;
        //[SerializeField] private float _deAcceleration = 60f;
        //[SerializeField] private float _apexBonus = 2;

        //private void CalculateWalk()
        //{
        //    if (_isMoving)
        //    {
        //        // Set horizontal move speed
        //        _currentHorizontalSpeed += _movementValue * _acceleration * Time.deltaTime;

        //        // clamped by max frame movement
        //        _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_maxMoveSpeed, _maxMoveSpeed);

        //        // Apply bonus at the apex of a jump
        //        var apexBonus = Mathf.Sign(_movementValue) * _apexBonus * _apexPoint;
        //        _currentHorizontalSpeed += apexBonus * Time.deltaTime;
        //    }
        //    else
        //    {
        //        // No input. Let's slow the character down
        //        _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
        //    }

        //    // Add external force to movement if one is being applied, pre-collision adjustment
        //    if (_forceReceiver.ForcedMove.HasValue)
        //    {
        //        _currentHorizontalSpeed = _currentHorizontalSpeed + _forceReceiver.ForcedMove.Value.x;

        //        if (_forceReceiver.ForcedMove.Value.x > 0 && _currentHorizontalSpeed > _forceReceiver.ForcedMove.Value.x)
        //            _currentHorizontalSpeed = _forceReceiver.ForcedMove.Value.x;
        //        else if (_forceReceiver.ForcedMove.Value.x < 0 && _currentHorizontalSpeed < -_forceReceiver.ForcedMove.Value.x)
        //            _currentHorizontalSpeed = _forceReceiver.ForcedMove.Value.x;
        //    }

        //    if (_currentHorizontalSpeed > 0 && _collisions.right || _currentHorizontalSpeed < 0 && _collisions.left)
        //    {
        //        // Don't walk through walls
        //        _currentHorizontalSpeed = 0;
        //    }
        //}

        //#endregion
    }
}
