using Editarrr.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceReceiver : MonoBehaviour, IExternalForceReceiver
{
    #region Events
    public Func<Vector3, bool> OnPositionRequest { get; set; }
    public Action OnCancelMovementRequest { get; set; }
    public Action<Vector3> OnForceStarted { get; set; }
    public Action OnForceEnded { get; set; }
    #endregion

	public Vector2? ForcedMove { get; private set; }

    [field: SerializeField] private float Duration { get; set; } = .2f;
    [field: SerializeField] private float ForceMultiplier { get; set; } = 1f;

    Vector3 Force { get; set; }
	float ImpulseTime { get; set; }

	public void ReceiveImpulse(float force, Vector3 direction)
	{
		/*(this.transform.position - origin).normalized;*/
		this.Force = direction * force * this.ForceMultiplier;
		this.ImpulseTime = 1;

        this.OnForceStarted?.Invoke(this.Force);

		this.CalculateForceMove();
	}

	private void FixedUpdate()
    {
        this.CalculateForceMove();
    }

    private void CalculateForceMove()
    {
        if (this.ImpulseTime <= 0)
        {
            if (this.ForcedMove.HasValue)
            {
                this.ForcedMove = null;
                this.OnForceEnded?.Invoke();
            }
            return;
        }

        this.ImpulseTime -= Time.fixedDeltaTime * (1f / this.Duration);

        this.ForcedMove = this.Force * this.ImpulseTime.ClampMin(0);
    }

    public bool SetPosition(Vector3 position)
    {
        return this.OnPositionRequest?.Invoke(position) == true;
    }

    public void CancelMovement()
    {
        this.OnCancelMovementRequest?.Invoke();
    }
}

public interface IExternalForceReceiver
{
    void ReceiveImpulse(float force, Vector3 direction);
    bool SetPosition(Vector3 position);
    void CancelMovement();
}