using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossController : BotController
{
    [Header("Boss Settings")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private GameObject toxicPoolPrefab;
    [SerializeField] private float minionSpawnCooldown = 10f;
    [SerializeField] private float toxicPoolCooldown = 8f;
    [SerializeField] private Transform[] spawnPoints;

    private float _nextMinionSpawnTime;
    private float _nextToxicPoolTime;
    private Animator Animator;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Animator.SetBool("WithGun", true);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        if (GetIsPlayerDetected()) // Если в бою
        {
            HandleBossAbilities();
        }
    }

    private void HandleBossAbilities()
    {
        if (_nextMinionSpawnTime <= 0f)
        {
            SpawnMinion();
            _nextMinionSpawnTime = minionSpawnCooldown;
        }
        else
        {
            _nextMinionSpawnTime -= Time.deltaTime;
        }

        if (_nextToxicPoolTime <= 0f)
        {
            SpawnToxicPool();
            _nextToxicPoolTime = toxicPoolCooldown;
        }
        else
        {
            _nextToxicPoolTime -= Time.deltaTime;
        }
    }

    private void SpawnMinion()
    {
        if (minionPrefab == null || spawnPoints.Length == 0) return;

        // Рандомный спавн из доступных точек
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void SpawnToxicPool()
    {
        if (toxicPoolPrefab == null) return;

        Vector3 spawnPosition;

        if (targetPlayer != null)
        {
            // Кидаем лужу под игрока с небольшой погрешностью
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 1.5f;
            spawnPosition = targetPlayer.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        }
        else
        {
            spawnPosition = transform.position;
        }

        Instantiate(toxicPoolPrefab, spawnPosition, Quaternion.identity);
    }
}
