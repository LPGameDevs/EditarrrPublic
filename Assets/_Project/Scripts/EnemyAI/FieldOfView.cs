using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float viewRadius;
    [Range(0, 360)]
    [SerializeField] public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new();

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets2D();
        }
    }

    private void FindVisibleTargets3D()
    {
        var targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (var t in targetsInViewRadius)
        {
            visibleTargets.Clear();
            var target = t.transform;
            var dirToTarget = (target.position - transform.position).normalized;
            if (!(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)) continue;
            var disToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask))
            {
                visibleTargets.Add(target);
            }
        }
    }

    private void FindVisibleTargets2D()
    {
        var targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        foreach (var t in targetsInViewRadius)
        {
            visibleTargets.Clear();
            var target = t.transform;
            var dirToTarget = (target.position - transform.position).normalized;
            if (!(Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)) continue;
            var disToTarget = Vector2.Distance(transform.position, target.position);
            if (!Physics2D.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask))
            {
                visibleTargets.Add(target);
            }

        }
    }


    public Vector3 DirFromAngle3D(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public Vector2 DirFromAngle2D(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
