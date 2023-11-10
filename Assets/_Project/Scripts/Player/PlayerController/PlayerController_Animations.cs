﻿using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        private static readonly int IsMovingAnim = Animator.StringToHash("IsMoving");
        private static readonly int GroundedAnim = Animator.StringToHash("Grounded");
        private static readonly int JumpStartedAnim = Animator.StringToHash("JumpStarted");
        private static readonly int VerticalVelocityAnim = Animator.StringToHash("VerticalVelocity");

        private Animator Animator { get; set; }

        private void UpdateAnimationVariables()
        {
            OnPlayerMoved(_collisions.down && _isMoving);

            Animator.SetFloat(VerticalVelocityAnim, _currentVerticalSpeed);
            Animator.SetBool(GroundedAnim, _collisions.down);

            // @todo Should we take collision checks into account here?
            Animator.SetBool(IsMovingAnim, _isMoving);
            Animator.SetBool(JumpStartedAnim, _jumpStartThisFrame);
        }
    }
}