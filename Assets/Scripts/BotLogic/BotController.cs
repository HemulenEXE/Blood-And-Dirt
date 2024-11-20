using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.AI;
using TMPro;
using System;
using Gun;

public class BotController : MonoBehaviour
{
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float loseSightTime = 3f;
    [SerializeField] private List<GameObject> patrolPoints;
    [SerializeField] private bool addStartPositionToPatrol = true;
    [SerializeField] private float rotationAngle = 15f;
    [SerializeField] private float rotationSpeed = 1;


    private Animator animator;
    private IGun gun;
    private StateBot stateBot;
    private GameObject lastPatrolPoint;
    private Quaternion initialRotation;
    private AudioSource audioSource;
    private Transform selfTransform;
    private Transform targetPlayer;
    private NavMeshAgent agent;
    private int maskVision;
    private float timeSinceLastSeen;
    private bool hasCollidedWithPlayer;

    public static event  Action<Transform, Transform> DetectedEnemy;

    private void Awake()
    {
        InitializeComponents();
        ConfigureAgent();
    }

    private void InitializeComponents()
    {
        selfTransform = transform;
        agent = GetComponent<NavMeshAgent>();
        maskVision = LayerMask.GetMask("Player", "Default") & ~LayerMask.GetMask("Enemy");
        audioSource = GetComponent<AudioSource>();
        initialRotation = selfTransform.rotation;

        if (addStartPositionToPatrol)
        {
            patrolPoints.Add(Helper.CopyTransformInGameObject(selfTransform));
        }

        gun = GetComponent<IGun>();
        animator = transform.GetComponentInChildren<Animator>();
        stateBot = patrolPoints.Count > 1 ? StateBot.patrol : StateBot.peace;
    }

    private void ConfigureAgent()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            TryDetectPlayer(collider.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            hasCollidedWithPlayer = false;
        }
    }

    private void TryDetectPlayer(Transform playerTransform)
    {
        Vector2 directionToPlayer = (playerTransform.position - selfTransform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(selfTransform.position, directionToPlayer, visionRange, maskVision);

        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
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
        if(stateBot != StateBot.combat)
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

    private void Update()
    {
        switch (stateBot) 
        {
            case StateBot.combat:
                animator.SetBool("IsRun", true);
                CombateState();
                break;
            case StateBot.peace:
                animator.SetBool("IsRun", false);
                PeaceState();
                break;
            case StateBot.patrol:
                animator.SetBool("IsRun", true);
                PatrolState();
                break;

        }
    }

    private void CombateState()
    {
        if (targetPlayer != null)
        {
            ChasePlayer();
            UpdateChaseTimer();
            if(IsPlayerVisible())
            {
                gun.Shoot();
            }
            else if(gun.IsShooting)
            {
                gun.StopShoot();
            }
        }
        else
        {
            gun.IsShooting = false;
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
        agent.SetDestination(targetPlayer.position);
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
        RaycastHit2D hit = Physics2D.Raycast(selfTransform.position, directionToPlayer, visionRange, maskVision);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }


    private void StopChase()
    {
        targetPlayer = null;
        hasCollidedWithPlayer = false;

        if (patrolPoints.Count == 1)
        {
            stateBot = StateBot.peace;
            agent.SetDestination(patrolPoints[0].transform.position);
        }
        else if(patrolPoints.Count > 1)
        {
            stateBot = StateBot.patrol;
        }
        
        
    }

    private void LookToDirection(Transform targetTransform)
    {
        Vector3 direction = ((Vector2)targetTransform.position - (Vector2)selfTransform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        selfTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
