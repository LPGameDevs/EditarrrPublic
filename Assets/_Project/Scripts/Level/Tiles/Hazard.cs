using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
	[SerializeField] int _damage;
	[SerializeField, Range(0.05f, 0.5f)] float _hitStopDuration;
	[SerializeField, Range(0.2f, 3f)] float _knockbackDuration;
	[SerializeField, Range(0f, 5f)] float _stunDuration;
    [SerializeField] AnimationCurve _knockbackCurveX;
	[SerializeField] AnimationCurve _knockbackCurveY;

	private void OnTriggerStay2D(Collider2D collider)
    {
		KnockBackPlayer(collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
		KnockBackPlayer(collision.collider);
	}

	public void KnockBackPlayer(Collider2D collider)
	{
		HealthSystem healthSystem;
		PlayerForceReceiver forceReceiver;
		if (!collider.TryGetComponent<HealthSystem>(out healthSystem) || healthSystem.IsInvincible())
			return;
		if (!collider.TryGetComponent<PlayerForceReceiver>(out forceReceiver))
			return;

		healthSystem.TakeDamage(_damage, _hitStopDuration + _knockbackDuration + _stunDuration);
		forceReceiver.ReceiveImpulse(_hitStopDuration, _knockbackDuration, _knockbackCurveX, _knockbackCurveY);
	}
}
