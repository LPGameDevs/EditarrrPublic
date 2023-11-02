using Editarrr.Input;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        [field: SerializeField, Header("INPUT")] private InputValue InputMove { get; set; }
        [field: SerializeField] private InputValue InputJump { get; set; }


        private float RawInputMove { get; set; }

        bool InputJumpPressed { get; set; }
        bool InputJumpReleased { get; set; }

        float InputJumpTime { get; set; }

        bool InputLocked { get; set; }
        bool IsMoving { get; set; }
        bool InputLock { get; set; }

        private void UpdateInput()
        {
            this.UpdateInputLock();

            this.RawInputMove = this.InputLocked ? 0 : this.InputMove.Read<Vector2>().x;

            this.IsMoving = this.InputMove.IsPressed && !this.InputLocked;

            this.InputJumpPressed = this.InputJump.WasPressed && !this.InputLocked;
            this.InputJumpReleased = !this.InputJump.IsPressed && !this.InputLocked;

            if (this.InputJumpPressed)
            {
                this.InputJumpTime = Time.time;
            }
        }

        private void SetInputLock(bool value)
        {
            this.InputLock = value;
        }

        private void UpdateInputLock()
        {
            this.InputLocked = this.InputLock || this.IsStunned;
        }
    }


    //#region Gather Input
    //[field: SerializeField, Tooltip("Move input map")] private InputValue MoveInput { get; set; }
    //[field: SerializeField, Tooltip("Jump input map")] private InputValue JumpInput { get; set; }
    //private bool _isMoving;
    //private bool _jumpStartThisFrame;
    //private bool _jumpReleaseThisFrame;
    //private float _movementValue;


    //private void GatherInput()
    //{
    //    _jumpStartThisFrame = _jumpReleaseThisFrame = false;
    //    _movementValue = _inputLocked ? 0f : MoveInput.Read<Vector2>().x;

    //    _isMoving = MoveInput.IsPressed && !_inputLocked;

    //    if (_inputLocked)
    //        return;


    //    if (JumpInput.WasPressed)
    //    {
    //        _jumpStartThisFrame = true;
    //        _lastJumpPressed = Time.time;
    //    }
    //    else if (JumpInput.WasReleased)
    //    {
    //        _jumpReleaseThisFrame = true;
    //    }
    //}

    //#endregion

}
