using Player;
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

	private void OnTriggerStay2D(Collider2D other)
    {
		KnockBackPlayer(other);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
		KnockBackPlayer(collision.collider);
	}

	public void KnockBackPlayer(Collider2D other)
	{
		HealthSystem healthSystem;
		PlayerForceReceiver forceReceiver;
		if (!other.TryGetComponent<HealthSystem>(out healthSystem) || healthSystem.IsInvincible())
			return;
		if (!other.TryGetComponent<PlayerForceReceiver>(out forceReceiver))
			return;

		healthSystem.TakeDamage(_damage, _hitStopDuration + _knockbackDuration + _stunDuration);
		forceReceiver.ReceiveImpulse(_hitStopDuration, _knockbackDuration, _knockbackCurveX, _knockbackCurveY);
	}
}
