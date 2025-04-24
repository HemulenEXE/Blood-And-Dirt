using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianController : MonoBehaviour
{
    public enum CivilianState { Idle, Patrol, Flee }

    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private Transform fleeTarget;
    [SerializeField] private float idleTime = 2f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float fleeSpeed = 5f;

    private Animator animator;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private CivilianState currentState = CivilianState.Idle;
    private float idleTimer;

    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponentInChildren<Animator>();
        agent.stoppingDistance = stoppingDistance;
        
        idleTimer = idleTime;

        if (patrolPoints.Count > 0)
        {
            currentState = CivilianState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case CivilianState.Idle:
                HandleIdle();
                break;

            case CivilianState.Patrol:
                HandlePatrol();
                break;

            case CivilianState.Flee:
                HandleFlee();
                break;
        }

        animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);
        UpdateLookDirection();
    }

    private void HandleIdle()
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f && patrolPoints.Count > 0)
        {
            currentState = CivilianState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void HandlePatrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            idleTimer = idleTime;
            currentState = CivilianState.Idle;
        }
    }

    private void HandleFlee()
    {
        if (fleeTarget != null)
        {
            agent.speed = fleeSpeed;
            agent.SetDestination(fleeTarget.position);
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }
    private void UpdateLookDirection()
    {
        Vector3 velocity = agent.velocity;

        // Проверим, движется ли агент
        if (velocity.sqrMagnitude > 0.01f)
        {
            Vector2 dir = new Vector2(velocity.x, velocity.y).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);

            // Плавно поворачиваем к нужному углу
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void TriggerFlee()
    {
        if (fleeTarget != null)
        {
            animator.SetBool("IsRun",true); animator.SetBool("IsMoving", true);
            currentState = CivilianState.Flee;
            agent.SetDestination(fleeTarget.position);
        }
    }

    public bool IsFlee()
    {
        return currentState == CivilianState.Flee;
    }
}
