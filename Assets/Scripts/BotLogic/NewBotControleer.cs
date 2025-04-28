using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBotControleer : MonoBehaviour
{
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float loseSightTime = 3f;
    [SerializeField] private List<GameObject> patrolPoints;
    [SerializeField] private bool addStartPositionToPatrol = true;
    [SerializeField] private float rotationAngle = 15f;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private EnemySides stateSide;
    [SerializeField] private StateBot stateBot;

    private float _nextAttackTime;
    private float _lastVisibilityCheck;
    private float visibilityCheckInterval = 0.3f;

    private Side sideBot;
    private Animator animator;
    private IGun gun;
    private GameObject lastPatrolPoint;
    private Quaternion initialRotation;
    private AudioSource audioSource;
    private Transform selfTransform;
    [SerializeField] private Transform targetPlayer;
    private NavMeshAgent agent;
    private float timeSinceLastSeen;
    private Transform sourceNoise;
    private bool hasCollidedWithPlayer;

    private bool lastIsMovingState = false;

    public static event Action<Transform, Transform> DetectedEnemy;
    public static bool IsPlayerDetected = false;
    public static event Action AudioEvent;

    private void Awake()
    {
        InitializeComponents();
        ConfigureAgent();
        InitEnemy(stateSide, true);
    }

    private void Update()
    {
        _nextAttackTime -= Time.deltaTime;

        switch (stateBot)
        {
            case StateBot.combat:
                CombateState();
                break;
            case StateBot.peace:
                PeaceState();
                break;
            case StateBot.patrol:
                PatrolState();
                break;
            case StateBot.checkNoise:
                break;
        }

        bool isMoving = Helper.IsAgentMoving(agent);
        if (lastIsMovingState != isMoving)
        {
            animator.SetBool("IsMoving", isMoving);
            lastIsMovingState = isMoving;
        }
    }

    private void InitializeComponents()
    {
        selfTransform = transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        initialRotation = selfTransform.rotation;

        if (addStartPositionToPatrol)
        {
            patrolPoints.Add(Helper.CopyTransformInGameObject(selfTransform));
        }

        animator = transform.GetComponentInChildren<Animator>();
        gun = transform.GetComponentInChildren<IGun>();
        InitToStartState();
    }

    public void InitEnemy(Side sideEnemy)
    {
        this.sideBot = sideEnemy;
    }

    public void InitEnemy(EnemySides side = EnemySides.agressive, bool playerEnemy = true)
    {
        sideBot = new Side();
        sideBot.Init(side, playerEnemy);
    }

    private void ConfigureAgent()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.avoidancePriority = UnityEngine.Random.Range(30, 80);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (sideBot.IsEnemyMask(collider.gameObject.layer) && stateBot != StateBot.combat)
        {
            TryDetectPlayer(collider.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (sideBot.IsEnemyMask(collider.gameObject.layer))
        {
            hasCollidedWithPlayer = false;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void TryDetectPlayer(Transform playerTransform)
    {
        if (Time.time - _lastVisibilityCheck < visibilityCheckInterval) return;

        _lastVisibilityCheck = Time.time;
        Vector2 directionToPlayer = (playerTransform.position - selfTransform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(selfTransform.position, directionToPlayer, visionRange, sideBot.GetTargetMask());

        if (hit.collider != null && sideBot.IsEnemyMask(hit.collider.gameObject.layer))
        {
            OnPlayerDetected(playerTransform);
        }
        else
        {
            hasCollidedWithPlayer = false;
        }
    }

    private void OnPlayerDetected(Transform playerTransform)
    {
        if (stateBot != StateBot.combat && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        targetPlayer = playerTransform;
        stateBot = StateBot.combat;
        timeSinceLastSeen = 0;
        hasCollidedWithPlayer = true;

        DetectedEnemy?.Invoke(selfTransform, playerTransform);
    }

    public void NotifiedOfEnemy(Transform playerTransform)
    {
        targetPlayer = playerTransform;
        stateBot = StateBot.combat;
        timeSinceLastSeen = 0;
        hasCollidedWithPlayer = true;
    }

    private void CombateState()
    {
        if (targetPlayer != null && targetPlayer.gameObject.activeInHierarchy)
        {
            bool playerVisible = false;
            if (Time.time - _lastVisibilityCheck > visibilityCheckInterval)
            {
                playerVisible = IsPlayerVisible();
                _lastVisibilityCheck = Time.time;
            }

            if (playerVisible && gun.IsInRange(targetPlayer.position))
            {
                agent.isStopped = true;
                if (_nextAttackTime <= 0)
                {
                    _nextAttackTime = gun.ShotDelay;
                    gun.Shoot(sideBot.CreateSideBullet());
                }
            }
            else
            {
                agent.isStopped = false;
                if (Vector2.Distance(agent.destination, targetPlayer.position) > 1f)
                {
                    agent.SetDestination(targetPlayer.position);
                }
            }

            LookToDirection(targetPlayer);
        }
        else
        {
            StopChase();
        }
    }

    private void PeaceState()
    {
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
        transform.rotation = initialRotation * Quaternion.Euler(0, 0, angle);
    }

    private void PatrolState()
    {
        if (lastPatrolPoint == null)
        {
            SetNextPatrolPoint(0);
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            int nextIndex = (patrolPoints.IndexOf(lastPatrolPoint) + 1) % patrolPoints.Count;
            SetNextPatrolPoint(nextIndex);
        }
    }

    private void SetNextPatrolPoint(int index)
    {
        lastPatrolPoint = patrolPoints[index];
        LookToDirection(lastPatrolPoint.transform);
        agent.SetDestination(lastPatrolPoint.transform.position);
    }

    private void StopChase()
    {
        targetPlayer = null;
        hasCollidedWithPlayer = false;
        agent.isStopped = false;
        InitToStartState();
    }

    private void InitToStartState()
    {
        if (patrolPoints.Count == 1)
        {
            stateBot = StateBot.peace;
            agent.SetDestination(patrolPoints[0].transform.position);
        }
        else if (patrolPoints.Count > 1)
        {
            stateBot = StateBot.patrol;
        }
        else
        {
            stateBot = StateBot.peace;
        }
    }

    private bool IsPlayerVisible()
    {
        if (targetPlayer == null) return false;

        Vector2 directionToPlayer = (targetPlayer.position - selfTransform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(selfTransform.position, directionToPlayer, visionRange, sideBot.GetTargetMask());

        return hit.collider != null && sideBot.IsEnemyMask(hit.collider.gameObject.layer);
    }

    private void LookToDirection(Transform targetTransform)
    {
        Vector3 direction = ((Vector2)targetTransform.position - (Vector2)selfTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        selfTransform.rotation = Quaternion.RotateTowards(selfTransform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed * 100);
    }

    public void AddPatrolState(Transform target)
    {
        patrolPoints.Add(target.gameObject);
        InitToStartState();
    }
}
