using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public void TryToLocatePlayer(EnemyAI ai, NavMeshAgent agent = null)
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        if (targetsInRange.Length > 0)
        {
            Transform target = targetsInRange[0].transform;
            var dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    if (!ai.playerSpotted)
                    {
                        ai.target = target;
                        ai.playerSpotted = true;
                    }
                }
                else
                {
                    if (ai.playerSpotted)
                    {
                        ai.target = null;
                        ai.playerSpotted = false;
                    }
                }
            }
            else
            {
                if (ai.playerSpotted)
                {
                    ai.target = null;
                    ai.playerSpotted = false;
                    StartCoroutine(ai.LostSight());
                }
            }
        }
        else
        {
            if (ai.playerSpotted)
            {
                ai.playerSpotted = false;
                ai.target = null;
                StartCoroutine(ai.LostSight());
            }
        }
    }

    public Vector3 DirFromAngle(float angle, bool globalAngle)
    {
        if (!globalAngle)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
