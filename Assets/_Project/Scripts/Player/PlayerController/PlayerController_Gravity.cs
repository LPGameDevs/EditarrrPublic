using System;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        [field: SerializeField, Header("GRAVITY")] private float FallClamp { get; set; } = -40f;
        [field: SerializeField] private float JumpCanceledGravityModifier { get; set; } = 3f;

        private float FallSpeed { get; set; }


        private void UpdateGravity()
        {
            if (this.Collisions.Down)
            {
                this.VerticalSpeed = this.VerticalSpeed < 0 ? 0 : this.VerticalSpeed;
            }
            else
            {
                var fallSpeed = this.JumpCanceled && this.VerticalSpeed > 0 ? this.FallSpeed * this.JumpCanceledGravityModifier : this.FallSpeed;

                float currentVerticalSpeed = this.VerticalSpeed;
                this.VerticalSpeed = (this.VerticalSpeed - (fallSpeed * Time.deltaTime)).ClampMin(this.FallClamp);

                if (currentVerticalSpeed >= 0f && this.VerticalSpeed < 0f)
                {
                    OnPlayerStartedFalling?.Invoke();
                }
            }
        }

        //[Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
        //[SerializeField] private float _minFallSpeed = 80f;
        //[SerializeField] private float _maxFallSpeed = 120f;
        //private float _fallSpeed;

        //private void CalculateGravity()
        //{
        //    if (_collisions.down || _forceReceiver.ForcedMove != null)
        //    {
        //        // Move out of the ground
        //        if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
        //    }
        //    else
        //    {
        //        // Add downward force while ascending if we ended the jump early
        //        var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

        //        // Fall
        //        float bufferedVerticalSpeed = _currentVerticalSpeed;
        //        _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

        //        // Clamp
        //        if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;

        //        if (_currentVerticalSpeed < 0f && bufferedVerticalSpeed >= 0f)
        //            OnPlayerStartedFalling?.Invoke();
        //    }
        //}
    }
}
