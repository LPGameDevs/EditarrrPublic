using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float viewRadius;
    [Range(0, 360)]
    [SerializeField] public float viewAngle;
    [SerializeField] private bool isTopDown = false;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new();

    private float _correctionAngle = -90f;
    private float _directionEnemyIsFacing = 1f;
    private float baseRadius, baseAngle;
    private Vector3 _directionFacing;


    private void Start()
    {
        baseRadius = viewRadius; 
        baseAngle = viewAngle;
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private void Update()
    {
        if (!isTopDown)
        {
            _directionFacing = transform.localScale.x == 1 ? Vector3.left : Vector3.right;
            _directionEnemyIsFacing = -1 * transform.localScale.x;
            _correctionAngle = Vector2.Angle(transform.right, transform.up); //changed for side scrolling
        }
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets2D();
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the view radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // Draw the view angle
        var value = _directionEnemyIsFacing != 0 ? _directionEnemyIsFacing : 1;
        float angleUp = (value * _correctionAngle) - (viewAngle / 2);
        float angleDown = (value * _correctionAngle) + (viewAngle / 2);
        Vector3 viewAngleA = DirFromAngle3D(angleUp, false);
        Vector3 viewAngleB = DirFromAngle3D(angleDown, true);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        // Draw lines to visible targets
        Gizmos.color = Color.blue;
        foreach (Transform visibleTarget in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
    }

    private void FindVisibleTargets3D()
    {
        visibleTargets.Clear();
        var targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (var t in targetsInViewRadius)
        {
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
        visibleTargets.Clear();
        var targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        foreach (var t in targetsInViewRadius)
        {
            var target = t.transform;
            var dirToTarget = (target.position - transform.position).normalized;
            float angleFromDirectionFacingToTarget = Vector2.Angle(_directionFacing, dirToTarget);
            if (!(angleFromDirectionFacingToTarget < viewAngle / 2)) continue;
            var disToTarget = Vector2.Distance(transform.position, target.position);
            RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, dirToTarget, disToTarget, obstacleMask);
            bool isPlayerVisible = true;
            foreach (var hit in allHits)
            {
                if (!IsNotThisEnemy(hit)) { continue; } //we don't want to detect this enemy
                isPlayerVisible = false; //some other obstacle is in between the enemy and the player
            }
            if (isPlayerVisible)
            {
                visibleTargets.Add(target);
            }
        }
    }

    private bool IsNotThisEnemy(RaycastHit2D hit)
    {
        return hit.transform.gameObject != gameObject;
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

    /// <summary>
    /// Change the FOV until it is reverted using RevertFOV. <br></br>
    /// Paramters are not adjusted if left out of the method call or set negative.
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    public void AdjustFOV(float radius = -1, float angle = -1)
    {
        viewRadius = radius < 0 ? viewRadius : radius;
        viewAngle = angle < 0 ?  viewAngle : angle;
    }

    public void RevertFOV()
    {
        viewRadius = baseRadius;
        viewAngle = baseAngle;
    }
}
