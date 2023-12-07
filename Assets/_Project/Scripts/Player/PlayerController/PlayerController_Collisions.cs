using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{

    public partial class PlayerController
    {
        private Action<bool> OnGroundCheck { get; set; }

        [field: SerializeField, Header("COLLISION")] private Collider2D Collider { get; set; }
        [field: SerializeField] private ContactFilter2D GroundContactFilter { get; set; }
        [field: SerializeField] private ContactFilter2D SpecialContactFilter { get; set; }
        [field: SerializeField] private float CollisionCheckDistance { get; set; } = 1f / 32f;
        [field: SerializeField] private float CollisionJumpCheckDistance { get; set; } = 2f / 32f;

        [field: SerializeField] private Direction2D<bool> Collisions { get; set; }
        private bool CollisionJump { get; set; }

        private LayerMask GroundLayer { get => this.GroundContactFilter.layerMask; }

        // Is Landing this frame
        private bool IsLanding { get; set; }
        private float LastGroundedTime { get; set; }

        private void StartCollision()
        {
            this.Collisions = new Direction2D<bool>(false);
        }

        private void UpdateCollision()
        {
            RaycastHit2D[] hit = new RaycastHit2D[1];

            bool groundCheck = this.Collider.Cast(Vector2.down, this.GroundContactFilter, hit, this.CollisionCheckDistance) > 0;

            if (groundCheck && hit[0].transform.GetProxyComponent<IGroundFriction>() is IGroundFriction groundFriction)
            {
                this.Friction = groundFriction.Friction;
                Debug.Log($"Friction: {this.Friction}");
            }
            // else if (groundCheck)
            // {
            //     this.Friction = 1f;
            // }
            // else
            // {
            //     this.Friction.Lerp(1, this.TimeScale);
            // }
            else
            {
                this.Friction = 1f;
            }

            this.OnGroundCheck?.Invoke(groundCheck);

            this.IsLanding = !this.Collisions.Down && groundCheck;
            if (this.IsLanding)
            {
                OnPlayerLanded?.Invoke(this.VerticalSpeed);
                this.AirTime = 0;
            }

            this.CollisionJump = this.Collider.Cast(Vector2.down, this.GroundContactFilter, hit, this.CollisionJumpCheckDistance) > 0;
            this.Collisions.Down = groundCheck;
            this.Collisions.Left = this.Collider.Cast(Vector2.left, this.GroundContactFilter, hit, this.CollisionCheckDistance) > 0;
            this.Collisions.Up = this.Collider.Cast(Vector2.up, this.GroundContactFilter, hit, this.CollisionCheckDistance) > 0;
            this.Collisions.Right = this.Collider.Cast(Vector2.right, this.GroundContactFilter, hit, this.CollisionCheckDistance) > 0;

            this.LastGroundedTime = (this.Collisions.Down && !this.IsLanding) ? Time.time : this.LastGroundedTime;

            //    // Generate ray ranges.
            //    CalculateRayRanges();

            //    // Ground
            //    var groundedCheck = RunDetection(_rays.down);
            //    if (!groundedCheck) _timeInAir += Time.deltaTime;
            //    if (_collisions.down && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
            //    else if (!_collisions.down && groundedCheck)
            //    {
            //        _coyoteUsable = true; // Only trigger when first touching
            //        OnPlayerLanded?.Invoke(_currentVerticalSpeed);
            //        _timeInAir = 0f;
            //    }
            //    _collisions.down = groundedCheck;

            //    // The rest
            //    _collisions.up = RunDetection(_rays.up);
            //    _collisions.left = RunDetection(_rays.left);
            //    _collisions.right = RunDetection(_rays.right);

            //    bool RunDetection(RayRange range)
            //    {
            //        return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            //    }

        }


        //[Header("COLLISION")] [SerializeField] private Bounds _characterBounds;
        //[SerializeField, Tooltip("Layer(s) to interact with for neutral collisions")] private LayerMask _groundLayer;
        //[SerializeField, Tooltip("Number of ray casts per bounding box side")] private int _detectorCount = 3;
        //[SerializeField, Tooltip("Length of collision raycasts")] private float _detectionRayLength = 0.1f;
        //[SerializeField, Range(0, 0.3f), Tooltip("Inset distance of raycasts")] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        //private Direction2D<RayRange> _rays = new Direction2D<RayRange>(new RayRange());
        //private Direction2D<bool> _collisions = new Direction2D<bool>(false);

        //private float _timeLeftGrounded;
        //private float _timeInAir;

        //// We use these raycast checks for pre-collision information
        //private void RunCollisionChecks()
        //{
        //    // Generate ray ranges.
        //    CalculateRayRanges();

        //    // Ground
        //    var groundedCheck = RunDetection(_rays.down);
        //    if (!groundedCheck) _timeInAir += Time.deltaTime;
        //    if (_collisions.down && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
        //    else if (!_collisions.down && groundedCheck)
        //    {
        //        _coyoteUsable = true; // Only trigger when first touching
        //        OnPlayerLanded?.Invoke(_currentVerticalSpeed);
        //        _timeInAir = 0f;
        //    }
        //    _collisions.down = groundedCheck;

        //    // The rest
        //    _collisions.up = RunDetection(_rays.up);
        //    _collisions.left = RunDetection(_rays.left);
        //    _collisions.right = RunDetection(_rays.right);

        //    bool RunDetection(RayRange range)
        //    {
        //        return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
        //    }
        //}

        //private void CalculateRayRanges()
        //{
        //    float bottom = _characterBounds.min.y + transform.position.y;
        //    float top = _characterBounds.max.y + transform.position.y;
        //    float left = _characterBounds.min.x + transform.position.x;
        //    float right = _characterBounds.max.x + transform.position.x;

        //    _rays.down = new RayRange(left + _rayBuffer, bottom, right - _rayBuffer, bottom, Vector2.down);
        //    _rays.up = new RayRange(left + _rayBuffer, top, right - _rayBuffer, top, Vector2.up);
        //    _rays.left = new RayRange(left, bottom + _rayBuffer, left, top - _rayBuffer, Vector2.left);
        //    _rays.right = new RayRange(right, bottom + _rayBuffer, right, top - _rayBuffer, Vector2.right);
        //}


        //private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        //{
        //    for (var i = 0; i < _detectorCount; i++)
        //    {
        //        var t = (float)i / (_detectorCount - 1);
        //        yield return Vector2.Lerp(range.Start, range.End, t);
        //    }
        //}

        //private void OnDrawGizmos_Collision()
        //{
        //    // Bounds
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

        //    // Rays
        //    if (!Application.isPlaying)
        //    {
        //        CalculateRayRanges();
        //        Gizmos.color = Color.blue;
        //        foreach (var range in new List<RayRange> { _rays.up, _rays.right, _rays.down, _rays.left })
        //        {
        //            foreach (var point in EvaluateRayPositions(range))
        //            {
        //                Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
        //            }
        //        }
        //    }

        //    if (!Application.isPlaying) return;

        //    // Draw the future position. Handy for visualizing gravity
        //    Gizmos.color = Color.red;
        //    var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
        //    Gizmos.DrawWireCube(transform.position + _characterBounds.center + move, _characterBounds.size);
        //}
    }
}
