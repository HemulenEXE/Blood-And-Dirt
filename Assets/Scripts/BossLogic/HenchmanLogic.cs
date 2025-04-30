using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HenchmanLogic : MonoBehaviour
{
    [SerializeField] private int distationToExplosive;
    [SerializeField] private int damageExplosive;
    [SerializeField] private int radiusExplosive;
    [SerializeField] GameObject prefabExplosive;


    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private float updateThreshold = 0.5f;
    private Vector3 lastTargetPosition;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            if (Vector3.Distance(player.transform.position, lastTargetPosition) > updateThreshold)
            {
                lastTargetPosition = player.transform.position;
                navMeshAgent.SetDestination(lastTargetPosition);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.remainingDistance <= distationToExplosive) 
        {
            Explode();
        }
        UpdateLookDirection();
    }

    private void Explode()
    {
        if (prefabExplosive != null)
        {
            Instantiate(prefabExplosive, transform.position, Quaternion.identity);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radiusExplosive);

        foreach (Collider2D nearbyObject in enemies)
        {

            PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.GetDamage(damageExplosive);
            }
        }

        Destroy(gameObject);
    }

    private void UpdateLookDirection()
    {
        Vector3 velocity = navMeshAgent.velocity;

        // Проверим, движется ли агент
        if (velocity.sqrMagnitude > 0.01f)
        {
            Vector2 dir = new Vector2(velocity.x, velocity.y).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle );

            // Плавно поворачиваем к нужному углу
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
