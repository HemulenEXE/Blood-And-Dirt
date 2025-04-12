using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
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

    public static event Action<Transform, Transform> DetectedEnemy;

    private void Awake()
    {
        InitializeComponents();
        ConfigureAgent();
        InitEnemy(stateSide,true);
    }
    private void Update()
    {
        _nextAttackTime -= Time.deltaTime;
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
        sideBot.Init(side,playerEnemy);
    }
    private void ConfigureAgent()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.avoidancePriority = UnityEngine.Random.Range(30, 80);

        //agent.stoppingDistance = stoppingDistance;
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
        if (stateBot != StateBot.combat)
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

    private void FixedUpdate()
    {
        switch (stateBot)
        {
            case StateBot.combat:
                //animator.SetBool("IsMoving", true);
                CombateState();
                break;
            case StateBot.peace:
                //animator.SetBool("IsMoving", Helper.IsAgentMoving(agent));
                PeaceState();
                break;
            case StateBot.patrol:
                //nimator.SetBool("IsMoving", true);
                PatrolState();
                break;
            case StateBot.checkNoise:

                //LookToDirection(sourceNoise);
                break;

        }
        animator.SetBool("IsMoving", Helper.IsAgentMoving(agent)); 

    }

    private void CombateState()
    {
        if (targetPlayer != null && targetPlayer.gameObject.activeInHierarchy)
        {
            ChasePlayer();
            UpdateChaseTimer();
            if (IsPlayerVisible() && _nextAttackTime <= 0)
            {
                gun.Shoot(sideBot.CreateSideBullet());
            }
            else if(gun.IsShooting)
            {
                //gun.StopShoot();
            }
        }
        else
        {
            //gun.IsShooting = false;
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

    private void ChasePlayer()
    {
        if (IsPlayerVisible() && gun.IsInRange(targetPlayer.transform.position))
        {
            // Если игрок в поле зрения и в радиусе атаки, останавливаемся
            agent.isStopped = true;
        }
        else
        {
            // Если игрок не в зоне атаки, продолжаем погоню
            agent.isStopped = false;
            agent.SetDestination(targetPlayer.position);
        }

        LookToDirection(targetPlayer);
    }

    private void UpdateChaseTimer()
    {
        if (!IsPlayerVisible())
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= loseSightTime)
            {
                StopChase();
            }
        }
        else
        {
            timeSinceLastSeen = 0;
        }
    }

    private bool IsPlayerVisible()
    {
        if (targetPlayer == null)
            return false;

        Vector2 directionToPlayer = (targetPlayer.position - selfTransform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(selfTransform.position, directionToPlayer, visionRange, sideBot.GetTargetMask());

        return hit.collider != null && sideBot.IsEnemyMask(hit.collider.gameObject.layer);
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

    private void LookToDirection(Transform targetTransform)
    {
        Vector3 direction = ((Vector2)targetTransform.position - (Vector2)selfTransform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        selfTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void ReactToNoise(Transform noiseTransform)
    {

        if (stateBot != StateBot.combat)
        {
            stateBot = StateBot.checkNoise;
            StartCoroutine(CheckNoiseState(noiseTransform));
        }
    }

    private IEnumerator CheckNoiseState(Transform noiseTransform, float timeAwaiting = 5)
    {
        if (noiseTransform == null)
        {
            yield break; // Завершаем корутину, если изначально объект отсутствует
        }

        sourceNoise = noiseTransform;
        stateBot = StateBot.checkNoise;

        float timeout = 2f;
        float timer = 0f;

        while (noiseTransform != null && Mathf.Abs(Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, SmoothLookToDirection(noiseTransform)))) > 5f)
        {
            if (timer > timeout)
                break; // Выход из цикла, если не успели повернуться за отведённое время

            timer += Time.deltaTime;
            yield return null;
        }
        if (noiseTransform == null) yield break; // Проверяем снова перед перемещением

        animator.SetBool("IsMoving", true);
        agent.SetDestination(noiseTransform.position);

        while (agent != null && !agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
        {
            if (noiseTransform == null) yield break; // Если объект пропал, прерываем корутину
            yield return null;
        }
        animator.SetBool("IsMoving", false);
        yield return new WaitForSeconds(timeAwaiting);

        if (noiseTransform == null) yield break; // Еще одна финальная проверка

        animator.SetBool("IsMoving", true);
        InitToStartState();
    }


    private float SmoothLookToDirection(Transform target)
    {
        // Получаем направление от текущей позиции к цели
        Vector3 direction = (target.position - transform.position).normalized;

        // Рассчитываем угол поворота по оси Z (в 2D поворачиваем только по этой оси)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Плавно поворачиваем объект с использованием Lerp
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        return angle;
    }

    public void AddPatrolState(Transform target) 
    {
        patrolPoints.Add(target.gameObject);
        InitToStartState();
    }

    
}
