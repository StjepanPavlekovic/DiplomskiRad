using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyAIType
{
    Patrolling, Lookout
}

public class EnemyAI : MonoBehaviour, IAlertable
{
    public LayerMask playerLayer;
    private EnemyScript thisEnemy;
    private bool alerted = false;
    [SerializeField]
    private float alertTime;
    public FieldOfView fov;
    public Transform target;
    public bool playerSpotted = false;
    public EnemyAIType enemyAIType;
    public float movementSpeed;
    private NavMeshAgent agent;
    public Transform[] pathPoints;
    private Transform targetPoint;
    private Vector3 initialDirection;
    public float rotationSpeed;

    private Quaternion lookRotation;
    private Vector3 direction;
    public float fovCheckFrequency;
    public float lookTime;
    private bool looking = false;

    private bool spotted = false;
    public bool lostSight = false;

    public float shootingCooldown;
    private float currentShootCooldown;
    private bool shooting = false;
    public EnemySpawner spawner;

    public bool inversePatroll = true;

    public void InitAI(Transform[] waypoints, EnemyAIType aIType, EnemySpawner enemySpawner, bool inverse)
    {
        GameManager.instance.cleanUpOnReset.Add(gameObject);
        pathPoints = waypoints;
        enemyAIType = aIType;
        spawner = enemySpawner;
        inversePatroll = inverse;
    }

    private void Start()
    {
        currentShootCooldown = shootingCooldown;
        thisEnemy = GetComponent<EnemyScript>();
        initialDirection = transform.forward;
        if (enemyAIType == EnemyAIType.Patrolling)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination((targetPoint = pathPoints[0]).position);
        }
        else if (enemyAIType == EnemyAIType.Lookout)
        {
            targetPoint = pathPoints[0];
        }
        fov = GetComponent<FieldOfView>();
        StartCoroutine(DelayedPlayerLookout());
    }

    void ShootPlayer()
    {
        thisEnemy.weapon.GetComponentInChildren<GunScript>().Shoot();
    }

    IEnumerator DelayedPlayerLookout()
    {
        while (true)
        {
            yield return new WaitForSeconds(fovCheckFrequency);
            fov.TryToLocatePlayer(this);
        }
    }

    private void Update()
    {
        if (!thisEnemy.isDead && !GameManager.instance.gamePaused)
        {
            if (enemyAIType == EnemyAIType.Patrolling)
            {
                if (playerSpotted)
                {
                    agent.isStopped = true;
                    Rotate(playerSpotted);
                    if (!agent.isStopped)
                    {
                        agent.isStopped = true;
                    }
                }
                else
                {
                    if (agent.isStopped)
                    {
                        agent.isStopped = false;
                    }
                    Patrolling();
                }
            }
            else if (enemyAIType == EnemyAIType.Lookout)
            {
                if (playerSpotted)
                {
                    Rotate(playerSpotted);
                }
                else
                {
                    Lookout();
                }
            }

            if (playerSpotted)
            {
                if (!shooting)
                {
                    StartCoroutine(ShootRoutine());
                }
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        shooting = true;
        while (playerSpotted && !thisEnemy.isDead && !GameManager.instance.gamePaused)
        {
            yield return new WaitForSeconds(fovCheckFrequency);
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, fov.viewRadius, playerLayer))
            {
                ShootPlayer();
            }
        }
        shooting = false;
    }

    public IEnumerator LostSight()
    {
        lostSight = false;
        if (enemyAIType == EnemyAIType.Lookout)
        {
            while (!closeEnough(transform.forward, initialDirection) && !playerSpotted && !thisEnemy.isDead)
            {
                lookRotation = Quaternion.LookRotation(initialDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                yield return new WaitForEndOfFrame();
            }
        }
        lostSight = false;
    }

    private void Rotate(bool rotateToPlayer)
    {
        if (rotateToPlayer)
        {
            direction = (target.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            direction = (targetPoint.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator LookoutRoutine()
    {
        looking = true;
        yield return new WaitForSeconds(lookTime);
        if (!thisEnemy.isDead)
        {
            GetNextPoint();
            initialDirection = (targetPoint.position - transform.position).normalized;
            looking = false;
        }
    }

    private void Lookout()
    {
        Rotate(false);
        if (!looking)
        {
            StartCoroutine(LookoutRoutine());
        }
    }

    private void Patrolling()
    {
        Rotate(false);
        if (!spotted)
        {
            if (closeEnough(transform.position, targetPoint.position))
            {
                agent.SetDestination(GetNextPoint().position);
            }
        }
    }

    private Transform GetNextPoint()
    {
        if (targetPoint != pathPoints[pathPoints.GetUpperBound(0)])
        {
            targetPoint = pathPoints[System.Array.FindIndex(pathPoints, point => point == targetPoint) + 1];
        }
        else
        {
            if (inversePatroll)
            {
                System.Array.Reverse(pathPoints);
            }
            targetPoint = pathPoints[1];
        }
        return targetPoint;
    }

    public static bool closeEnough(Vector3 pos1, Vector3 pos2, float maxDifference = 0.2f)
    {
        return closeEnough(pos1.x, pos2.x, maxDifference) && closeEnough(pos1.y, pos2.y, maxDifference) && closeEnough(pos1.z, pos2.z, maxDifference);
    }

    public static bool closeEnough(float numberA, float numberB, float maxDifference = 0.2f)
    {
        return Mathf.Abs(numberA - numberB) < maxDifference;
    }

    public void Alert(Vector3 origin)
    {
        if (!playerSpotted)
        {
            if (!alerted)
            {
                alerted = true;
                float initialRadius = fov.viewRadius;
                float initialAngle = fov.viewAngle;

                fov.viewRadius = initialRadius * 10;
                fov.viewAngle = 360;

                StartCoroutine(EnemyAlerted(initialRadius, initialAngle));
            }
        }
    }

    IEnumerator EnemyAlerted(float initialRadius, float initialAngle)
    {
        yield return new WaitForSeconds(alertTime);
        fov.viewRadius = initialRadius;
        fov.viewAngle = initialAngle;
        alerted = false;
    }
}
