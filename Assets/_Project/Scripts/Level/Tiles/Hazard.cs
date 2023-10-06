using Editarrr.Misc;
using Player;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{

	[SerializeField] int _damage;
	/// <summary>
	/// The Force an entity should be kicked away with. Usually 25 Is good enough for a decent 'kick'.
	/// </summary>
	[field: SerializeField] private float KnockbackForce { get; set; }
	/// <summary>
	/// Additional Invincibility Time
	/// </summary>
	[field: SerializeField] private float DamageCooldown { get; set; }
	/// <summary>
	/// The Duration the Player can not directly control the character
	/// </summary>
	[field: SerializeField] private float StunDuration { get; set; }

    [field: SerializeField] private Transform ForceOrigin { get; set; }

	private void OnTriggerStay2D(Collider2D other)
	{
		Vector3 forceDirection = (other.transform.position - this.ForceOrigin.position).normalized;

		this.KnockBackPlayer(other.gameObject, Vector3.zero, forceDirection);
	}

    private void OnCollisionStay2D(Collision2D collision) => OnTriggerStay2D(collision.collider);

    public void KnockBackPlayer(GameObject other, Vector3 point, Vector3 normal)
	{
		if (!other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem) || healthSystem.IsInvincible())
			return;
		if (!other.TryGetComponent<PlayerForceReceiver>(out PlayerForceReceiver forceReceiver))
			return;

		healthSystem.TakeDamage(_damage, this.StunDuration, this.DamageCooldown);
        forceReceiver.ReceiveImpulse(this.KnockbackForce, normal);
    }

    private void OnDrawGizmos()
    {
		Gizmos.DrawSphere(this.transform.position, .1f);
    }
}
