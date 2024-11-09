using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float loseSightTime = 3f;
    [SerializeField] private StateBot stateBot = StateBot.peace;

    private Transform selfTransform;
    private Transform targetPlayer;
    private NavMeshAgent agent;
    private int maskVision;
    private float timeSinceLastSeen;
    private bool hasCollidedWithPlayer;

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

        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
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
        targetPlayer = playerTransform;
        stateBot = StateBot.combat;
        timeSinceLastSeen = 0;
        hasCollidedWithPlayer = true;
    }

    private void FixedUpdate()
    {
        if (stateBot == StateBot.combat && targetPlayer != null)
        {
            ChasePlayer();
            UpdateChaseTimer();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(targetPlayer.position);
        //selfTransform.LookAt(targetPlayer.position);
        //transform.rotation = new Quaternion(0, 0, transform.rotation.z, transform.rotation.w);
        LookAtTarget();
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
        stateBot = StateBot.peace;
        targetPlayer = null;
        hasCollidedWithPlayer = false;
    }

    private void LookAtTarget()
    {
        Vector2 direction = (targetPlayer.position - selfTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        selfTransform.rotation = Quaternion.Lerp(selfTransform.rotation, targetRotation, 5 * Time.deltaTime);
    }


    //void Idle(float time, int tickAngle = 5)
    //{
    //    int vectorDirection = 1;

    //    int currentAngle = 0;

    //    while(true)
    //    {
    //        selfTransform.rotation = new Quaternion(selfTransform.rotation.x, selfTransform.rotation.y, selfTransform.rotation.z + tickAngle * vectorDirection, selfTransform.rotation.w);

    //        currentAngle += tickAngle * vectorDirection;

    //        if(currentAngle == tickAngle * 3 || currentAngle == tickAngle * -3)
    //        {
    //            vectorDirection *= -1;
    //        }


    //    }

    //}
}
