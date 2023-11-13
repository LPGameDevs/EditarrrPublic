using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        [field: SerializeField, Header("JUMP")] private float JumpForce { get; set; } = 4f;
        [field: SerializeField] private float CoyoteTime { get; set; } = 0.1f;
        [field: SerializeField] private float JumpBuffer { get; set; } = 0.1f;
        [field: SerializeField] private float FallSpeedMin { get; set; } = 80f;
        [field: SerializeField] private float FallSpeedMax { get; set; } = 120f;
        [field: SerializeField] private float JumpApexThreshold { get; set; } = 10f;

        private float JumpApexInfluence { get; set; } // Ranges from 0 to 1 (At Threshold)

        private float VerticalSpeed { get; set; }

        private bool CanJump { get; set; }
        private bool CoyoteTimeActive { get; set; }
        private bool CoyoteJumpFlag { get; set; }
        private bool JumpBufferActive { get; set; }

        private bool JumpCanceled { get; set; }

        private float AirTime { get; set; }


        private void UpdateJump()
        {
            this.UpdateJumpValues();

            if ((this.InputJumpPressed && this.CanJump) || this.JumpBufferActive)
            {                
                this.VerticalSpeed = this.JumpForce;
                this.JumpCanceled = false;
                this.CoyoteJumpFlag = false;
                this.InputJumpTime = float.MinValue;

                OnPlayerJumped?.Invoke();
                Debug.Log($"Jump == VS: {this.VerticalSpeed}, CT: {this.CoyoteTimeActive}, JB: {this.JumpBufferActive}");
            }

            if (!this.Collisions.Down && this.InputJumpReleased && !this.JumpCanceled && this.Velocity.y > 0)
            {
                this.JumpCanceled = true;
            }
        }

        private void UpdateJumpValues()
        {
            if (this.IsLanding)
            {
                this.CoyoteJumpFlag = true;
                this.AirTime = 0;
            }
            else if (!this.Collisions.Down)
            {
                this.AirTime += this.TimeScale;
            }

            this.CoyoteTimeActive = this.CoyoteJumpFlag && !this.Collisions.Down && this.LastGroundedTime + this.CoyoteTime > Time.time;
            this.CanJump = this.Collisions.Down || this.CoyoteTimeActive;

            this.JumpBufferActive = this.CanJump && this.InputJumpTime + this.JumpBuffer >= Time.time;
        }


        private void UpdateJumpApex()
        {
            if (!this.Collisions.Down)
            {
                this.JumpApexInfluence = Mathf.InverseLerp(this.JumpApexThreshold, 0, Mathf.Abs(this.Velocity.y));
                this.FallSpeed = Mathf.Lerp(this.FallSpeedMin, this.FallSpeedMax, this.JumpApexInfluence);

                //float target = Mathf.Lerp(this.FallSpeedMin, this.FallSpeedMax, this.JumpApexInfluence * (Time.deltaTime * .25f));

                //float tI = Mathf.InverseLerp(this.JumpApexThreshold, 0, Mathf.Abs(this.Velocity.y));
                //this.JumpApexInfluence = Mathf.Lerp(this.JumpApexInfluence, tI, Time.deltaTime * Time.deltaTime);


                //float t = Mathf.Lerp(this.FallSpeedMin, this.FallSpeedMax, this.JumpApexInfluence);
                //float target = t;
                ////float target = Mathf.Lerp(this.FallSpeedMin, this.FallSpeedMax, this.JumpApexInfluence * (Time.deltaTime * .25f));
                //this.FallSpeed = target; // Mathf.Lerp(this.FallSpeedMin, this.FallSpeedMax, this.JumpApexInfluence);
            }
            else
            {
                this.JumpApexInfluence = 0;
            }
        }

        //[Header("JUMPING")] [SerializeField] private float _jumpHeight = 30;
        //[SerializeField] private float _jumpApexThreshold = 10f;
        //[SerializeField] private float _coyoteTimeThreshold = 0.1f;
        //[SerializeField] private float _jumpBuffer = 0.1f;
        //[SerializeField] private float _jumpEndEarlyGravityModifier = 3;

        //private bool _coyoteUsable;
        //private bool _endedJumpEarly = true;
        //private float _apexPoint; // Becomes 1 at the apex of a jump
        //private float _lastJumpPressed;
        //private bool CanUseCoyote => _coyoteUsable && !_collisions.down && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        //private bool HasBufferedJump => _collisions.down && _lastJumpPressed + _jumpBuffer > Time.time;

        //private void CalculateJumpApex()
        //{
        //    if (!_collisions.down)
        //    {
        //        // Gets stronger the closer to the top of the jump
        //        _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
        //        _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
        //    }
        //    else
        //    {
        //        _apexPoint = 0;
        //    }
        //}

        //private void CalculateJump()
        //{
        //    // Jump if: grounded or within coyote threshold || sufficient jump buffer
        //    if (_jumpStartThisFrame && CanUseCoyote || HasBufferedJump)
        //    {
        //        _currentVerticalSpeed = _jumpHeight;
        //        _endedJumpEarly = false;
        //        _coyoteUsable = false;
        //        _timeLeftGrounded = float.MinValue;
        //        OnPlayerJumped?.Invoke();
        //    }

        //    // End the jump early if button released
        //    if (!_collisions.down && _jumpReleaseThisFrame && !_endedJumpEarly && _velocity.y > 0)
        //    {
        //        // _currentVerticalSpeed = 0;
        //        _endedJumpEarly = true;
        //    }

        //    //Overwrite movement with external force if one is being applied, pre-collision adjustment
        //    if (_forceReceiver.ForcedMove.HasValue)
        //    {
        //        _currentVerticalSpeed = _forceReceiver.ForcedMove.Value.y;
        //    }

        //    if (_collisions.up)
        //    {
        //        if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
        //    }
        //}
    }
}
