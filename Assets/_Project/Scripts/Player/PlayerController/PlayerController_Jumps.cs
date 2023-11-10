using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        [Header("JUMPING")] [SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;

        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed;
        private bool _twitchJumpRequested = false;
        private bool _twitchBouncy = false;
        private bool CanUseCoyote => _coyoteUsable && !_collisions.down && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _collisions.down && _lastJumpPressed + _jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!_collisions.down)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
            {
                _apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (_jumpStartThisFrame && CanUseCoyote || HasBufferedJump)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                OnPlayerJumped?.Invoke();

                if (!_twitchBouncy)
                {
                    _twitchJumpRequested = false;
                }
            }

            // End the jump early if button released
            if (!_collisions.down && _jumpReleaseThisFrame && !_endedJumpEarly && _velocity.y > 0)
            {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            //Overwrite movement with external force if one is being applied, pre-collision adjustment
            if (_forceReceiver.ForcedMove.HasValue)
            {
                _currentVerticalSpeed = _forceReceiver.ForcedMove.Value.y;
            }

            if (_collisions.up)
            {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
        }

        private void TwitchJump()
        {
            _twitchJumpRequested = true;
        }

        private void TwitchBouncy()
        {
            _twitchJumpRequested = true;
            _twitchBouncy = true;
        }
    }
}
