using UnityEngine;

namespace Player
{

    public partial class PlayerController
    {
        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        [field: SerializeField] private float FreeColliderUnstuckDistance { get; set; } = .1f;


        private Vector3 _velocity;
        private Vector3 _rawMovement;
        private Vector3 Move { get; set; }
        private Vector3 _lastPosition;
        private Vector3 LastValidPosition { get; set; }

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter2()
        {
            Vector3 pos = this.transform.position + this._characterBounds.center;
            Vector3 delta = Vector3.zero;

            if (this.IsOnPlatform)
            {
                // delta = pos - this.LastValidPosition;
            }

            this._rawMovement = new Vector3(this._currentHorizontalSpeed, this._currentVerticalSpeed); // Used externally
            Vector3 move = this._rawMovement * Time.deltaTime - delta;
            Vector3 furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            Collider2D hit = Physics2D.OverlapBox(furthestPoint, this._characterBounds.size, 0, this._groundLayer);
            if (!hit)
            {
                this.transform.position += move + delta;
                this.LastValidPosition = this.transform.position;
                return;
            }

            if (this.IsOnPlatform)
            {
                pos -= delta;
            }

            // We are moving
            // otherwise increment away from current pos; see what closest position we can move to
            Vector3 positionToMoveTo = pos;
            for (int i = 1; i < this._freeColliderIterations; i++)
            {
                // increment to check all but furthestPoint - we did that already
                float t = (float)i / this._freeColliderIterations;
                Vector2 posToTry = Vector2.Lerp(pos, furthestPoint, t) - (Vector2)delta;

                if (Physics2D.OverlapBox(posToTry, this._characterBounds.size, 0, this._groundLayer))
                {
                    this.LastValidPosition = this.transform.position = positionToMoveTo;

                    return;
                }

                positionToMoveTo = posToTry;
            }

        }
        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            Vector3 currentPosition = this.transform.position + this._characterBounds.center;
            Vector3 delta = Vector3.zero;

            // if (this.IsOnPlatform)
            {
                delta = currentPosition - this.LastValidPosition;
            }

            this._rawMovement = new Vector3(this._currentHorizontalSpeed, this._currentVerticalSpeed); // Used externally
            Vector3 move = this._rawMovement * Time.deltaTime + delta;
            Vector3 targetPoint = currentPosition + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            Collider2D hit = Physics2D.OverlapBox(targetPoint, this._characterBounds.size, 0, this._groundLayer);
            if (!hit)
            {
                this.transform.position += move - delta;
                this.LastValidPosition = this.transform.position;
                return;
            }

            // We are moving
            // otherwise increment away from current pos; see what closest position we can move to
            if (!this.MoveCharacter(currentPosition, delta, targetPoint))
            {
                Debug.Log("Emergency Resolve!");
                Vector3[] unstuckPoints = new Vector3[]
                {
                    this.LastValidPosition + new Vector3(move.x, Mathf.Sign(move.y) * -this.FreeColliderUnstuckDistance), // Move Horizontally first
                    this.LastValidPosition + new Vector3(Mathf.Sign(move.x) * -this.FreeColliderUnstuckDistance, move.y) // And Vertically second
                };

                for (int i = 0; i < unstuckPoints.Length; i++)
                {
                    if (this.MoveCharacter(currentPosition, delta, unstuckPoints[i]))
                        break;
                }
            }
        }

        private bool MoveCharacter(Vector3 currentPosition, Vector3 delta, Vector3 furthestPoint)
        {
            Vector3 positionToMoveTo = currentPosition - delta;
            try
            {
                for (int i = 1; i < this._freeColliderIterations; i++)
                {
                    // increment to check all but furthestPoint - we did that already
                    float t = (float)i / this._freeColliderIterations;
                    Vector2 posToTry = Vector2.Lerp(currentPosition, furthestPoint, t) - (Vector2)delta;

                    if (Physics2D.OverlapBox(posToTry, this._characterBounds.size, 0, this._groundLayer))
                    {
                        // We have a collision, set the previous point!

                        if (i > 1)
                            positionToMoveTo -= delta;

                        return i > 1;
                    }

                    // No Collision, keep going!
                    positionToMoveTo = posToTry;
                }
            }
            finally
            {
                this.LastValidPosition = this.transform.position = positionToMoveTo;
            }

            return true;
        }

        private void HandleSpriteDirection()
        {
            if (this._movementValue != 0)
            {
                this.transform.localScale = new Vector2(Mathf.Sign(this._movementValue), 1f);
            }
        }

        private void OnDrawGizmos_Move()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(this.LastValidPosition, Vector3.one * .2f);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(this.LastValidPosition + this.Move, Vector3.one * .2f);
        }
    }
}
