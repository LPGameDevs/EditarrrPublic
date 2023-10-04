using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceReceiver : MonoBehaviour
{
	public Vector2? ForcedMove { get; private set; }
	float Timer { get; set; }
	bool IsActive { get; set; }
	Vector3 Origin { get; set; }

    [field: SerializeField] private Vector3 ToOrigin { get; set; }
	Vector3 Force { get; set; }
	float Strength { get; set; }

	public void ReceiveImpulse(Vector3 origin)
    {
		this.ToOrigin = origin - this.transform.position;
		this.Force = this.ToOrigin * 3;
		this.Strength = 1;
    }

	public void ReceiveImpulse(float hitStopDuration, float knockbackDuration, AnimationCurve knockbackCurveX, AnimationCurve knockbackCurveY)
	{
		//this.IsActive = true;
		//this.Timer = -hitStopDuration;

		StartCoroutine(CoroutineImpulse(hitStopDuration, knockbackDuration, knockbackCurveX, knockbackCurveY));
	}

	IEnumerator CoroutineImpulse(float hitStopDuration, float knockbackDuration, AnimationCurve knockbackCurveX, AnimationCurve knockbackCurveY)
	{
		ForcedMove = Vector2.zero;

		yield return new WaitForSeconds(hitStopDuration);

		float timeLimit = knockbackDuration + Time.time;

		while (Time.time < timeLimit)
        {
			float t = 1f - ((timeLimit - Time.time) / knockbackDuration);
			ForcedMove = new Vector2(knockbackCurveX.Evaluate(t) * -transform.localScale.x, knockbackCurveY.Evaluate(t));
			yield return new WaitForEndOfFrame();
        }

		//while (Time.time < timeLimit)
		//{
		//	float t = Mathf.Clamp((timeLimit - Time.time), 0, 1);
		//	Vector2 ab = Vector2.Lerp(bezierPoints[0], bezierPoints[1], t);
		//	Vector2 bc = Vector2.Lerp(bezierPoints[1], bezierPoints[2], t);
		//	Vector3 target = Vector2.Lerp(ab, bc, t);
		//	ForcedMove = target - transform.position;

		//	yield return null;
		//}

		ForcedMove = null;
	}

    private void Update()
    {
		if (this.Strength <= 0)
		{
			this.ForcedMove = null;
			return;
		}

		this.Strength -= Time.deltaTime;

		this.ForcedMove = this.Force * this.Strength;
    }
}
