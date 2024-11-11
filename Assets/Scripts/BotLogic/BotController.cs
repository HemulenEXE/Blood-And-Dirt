using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.AI;
using TMPro;
using System;

public class BotController : MonoBehaviour
{
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float loseSightTime = 3f;
    [SerializeField] private List<GameObject> patrolPoints;
    [SerializeField] private bool addStartPositionToPatrol = true;
    [SerializeField] private float rotationAngle = 15f;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private StateBot stateBot;

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

    private void TryDetectPlayer(Transform playerTransform)
    {
        Vector2 directionToPlayer = (playerTransform.position - selfTransform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(selfTransform.position, directionToPlayer, visionRange, maskVision);
        print("piy");
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("check");
            OnPlayerDetected(playerTransform);
        }
        else
        {
            print("loose");
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
                CombateState();
                break;
            case StateBot.peace:
                PeaceState();
                break;
            case StateBot.patrol:
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

    private void ChasePlayer()
    {
        agent.SetDestination(targetPlayer.position);
        LookToDirection(targetPlayer);
    }

    private void UpdateChaseTimer()
    {
        if (!hasCollidedWithPlayer)
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= loseSightTime)
            {
                StopChase();
            }
        }
    }

    private void StopChase()
    {
        if(patrolPoints.Count == 1)
        {
            stateBot = StateBot.peace;
            agent.SetDestination(patrolPoints[0].transform.position);
        }
        else if(patrolPoints.Count > 1)
        {
            stateBot = StateBot.patrol;
        }
        
        targetPlayer = null;
        hasCollidedWithPlayer = false;
    }

    private void LookToDirection(Transform targetTransform)
    {
        Vector3 direction = ((Vector2)targetTransform.position - (Vector2)selfTransform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        selfTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
