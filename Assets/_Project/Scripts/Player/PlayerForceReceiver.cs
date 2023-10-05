using Editarrr.Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceReceiver : MonoBehaviour
{
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

		this.CalculateForceMove();
	}

	private void Update()
    {
        this.CalculateForceMove();
    }

    private void CalculateForceMove()
    {
        if (this.ImpulseTime <= 0)
        {
            this.ForcedMove = null;
            return;
        }

        this.ImpulseTime -= Time.deltaTime * (1f / this.Duration);

        this.ForcedMove = this.Force * this.ImpulseTime;
    }
}