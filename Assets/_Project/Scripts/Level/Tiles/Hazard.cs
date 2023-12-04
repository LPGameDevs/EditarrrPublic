using Editarrr.Audio;
using Editarrr.Misc;
using MoreMountains.Feedbacks;
using Player;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
	public event Action<Transform> OnCollision;

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

	[field: SerializeField] private bool useStaticForce { get; set; }
    [field: SerializeField] private MMFeedback contactFeedback { get; set; }

	[SerializeField] bool _useTrigger = true, _useCollider = false;

    private void OnTriggerStay2D(Collider2D other)
	{
		if(_useTrigger)
			HandleCollision(other.gameObject);
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(_useCollider)
			HandleCollision(collision.gameObject);
    }
    public void HandleCollision(GameObject other)
	{
		if (!other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem) || healthSystem.IsInvincible())
			return;
		if (!other.TryGetComponent<IExternalForceReceiver>(out IExternalForceReceiver forceReceiver))
			return;

        Vector3 forcePoint = useStaticForce ? this.transform.position : other.transform.position;
        Vector3 forceDirection = (forcePoint - this.ForceOrigin.position).normalized;

        healthSystem.TakeDamage(_damage, this.StunDuration, this.DamageCooldown);
        forceReceiver.ReceiveImpulse(this.KnockbackForce, forceDirection);
		
		contactFeedback?.Play(transform.position);
		OnCollision?.Invoke(other.transform);
    }

    private void OnDrawGizmos()
    {
		Gizmos.DrawSphere(this.transform.position, .1f);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(ForceOrigin.position, this.transform.position);
    }

	public void AdjustDamage(int newDamage) => _damage = newDamage;
	public void AdjustKnockback(int newKnockback) => KnockbackForce = newKnockback;
}
