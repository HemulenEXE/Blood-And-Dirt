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
    [SerializeField] private GameObject smokePrefab; // ��� ��� ������������
    [SerializeField] private float minionSpawnCooldown = 10f;
    [SerializeField] private float toxicPoolCooldown = 8f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int countToxicPool = 3;
    [SerializeField] private float radiusToxicPool = 2f;
    [SerializeField] private int counthanchman = 2;

    [Header("Second Phase Settings")]
    [SerializeField] private float healthThresholdForPhase2 = 0.5f; // ����� �������� ��� ��������
    [SerializeField] private int extraMinionsPhase2 = 5; // ������� �������� ������������� ��������
    [SerializeField] private float secondPhaseAttackSpeedMultiplier = 1.5f;
    private bool isSecondPhase = false;
    private bool isSpawningToxic = false;

    private float _nextMinionSpawnTime;
    private float _nextToxicPoolTime;
    private Animator anim;
    private BossHealth healthBot;

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("WithGun", true);
        healthBot = GetComponentInChildren<BossHealth>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        if (GetIsPlayerDetected()) // ���� � ���
        {
            if (!isSecondPhase && healthBot.GetHealth() <= healthBot.maxHealth * healthThresholdForPhase2)
            {
                StartCoroutine(TransitionToSecondPhase());
            }
            else
            {
                HandleBossAbilities();
            }
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
            gun.Recharge();
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

        int spawnCount = isSecondPhase ? counthanchman + extraMinionsPhase2 : counthanchman;

        // ��������� ����� �� ��������� �����
        for (int i = 0; i < spawnCount; i++) 
        {
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
        }
        
    }

    private void SpawnToxicPool()
    {
        if (!isSpawningToxic)
        {
            StartCoroutine(SpawnToxicPoolCoroutine());
        }
    }

    private IEnumerator SpawnToxicPoolCoroutine()
    {
        isSpawningToxic = true;

        // ��������� �������� �����
        anim.SetTrigger("Attack");

        
        yield return new WaitForSeconds(2.2f); // �������� �� ������������ ����� ��������

        if (toxicPoolPrefab != null && targetPlayer != null)
        {
            Vector3 playerPos = targetPlayer.position;
            int modificator = 360 / countToxicPool;

            for (int i = 0; i < countToxicPool; i++)
            {
                float angleDeg = modificator * i;
                float angleRad = angleDeg * Mathf.Deg2Rad;

                Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * 2.5f;
                Vector3 spawnPosition = playerPos + offset;

                Instantiate(toxicPoolPrefab, spawnPosition, Quaternion.identity);
            }
        }

        



        isSpawningToxic = false;
    }

    private IEnumerator TransitionToSecondPhase()
    {
        isSecondPhase = true;

        // ������� ��� �� ����� �����
        if (smokePrefab != null)
        {
            Instantiate(smokePrefab, transform.position, Quaternion.identity);
        }
        
        // ������ ����� ��������� � ���������� �� ����� ��������
        gameObject.GetComponentInChildren<BossHealth>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        anim.SetBool("IsMoving", false);

        // ���� ������� ��� ������� ������������
        yield return new WaitForSeconds(2f);

        // ������� ���� ��������
        for (int i = 0; i < extraMinionsPhase2; i++)
        {
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
        }

        // ���� ���������� �����
        gameObject.GetComponentInChildren<BossHealth>().enabled = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

        // ����������� ��� �������������
        minionSpawnCooldown /= secondPhaseAttackSpeedMultiplier;
        toxicPoolCooldown /= secondPhaseAttackSpeedMultiplier;

        anim.SetBool("IsMoving", true);
    }
}
