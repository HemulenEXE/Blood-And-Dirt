using GunLogic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class HealthBot : AbstractHealth
{
    private GameObject[] _bodyPrefabs;
    private AudioClip _deathSound;

    public static event Action<BotController> death;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile" && collision.gameObject.layer != LayerMask.NameToLayer("EnemyProjectile"))
        {

            var dataBullet = collision.gameObject.GetComponent<IBullet>();
            GetDamage(dataBullet);
        }
    }

    public override void GetDamage(IBullet bullet)
    {
        if (!isInvulnerable)
        {
            currentHealth -= (int)bullet.Damage;

            if (currentHealth <= 0)
            {
                Death();
                return;
            }
        }
    }

    public void GetDamage(Knife knife)
    {
        if (!isInvulnerable)
        {
            currentHealth -= (int)knife.Damage;

            if (currentHealth <= 0)
            {
                Death();
                return;
            }
        }
    }

    public override void Death()
    {
        DisableBotComponents(this.transform.parent.gameObject);

        death?.Invoke(transform.root.GetComponent<BotController>());

        this.transform.parent.GetComponent<AudioSource>()?.PlayOneShot(_deathSound);

        GameObject.Instantiate(_bodyPrefabs[UnityEngine.Random.Range(0, _bodyPrefabs.Length)], this.transform.position, Quaternion.identity);

        var animator = this.transform.parent.GetComponentInChildren<Animator>();
        string deathTrigger = UnityEngine.Random.Range(0, 2) == 0 ? "Death1" : "Death2";
        animator.SetTrigger(deathTrigger);

        Destroy(transform.root.gameObject, Math.Max(animator.GetCurrentAnimatorStateInfo(0).length, _deathSound.length));
    }
    private void DisableBotComponents(GameObject start)
    {
        Collider2D[] colliders = start.GetComponentsInChildren<Collider2D>();
        foreach (var x in colliders) x.enabled = false;

        var components = start.GetComponents<MonoBehaviour>();
        foreach (var x in components) if (x.GetType() != typeof(AudioSource)) x.enabled = false;

        var navMeshAgent = start.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }
    }

    private void Awake()
    {
        string type = transform.parent.name;
        if (type.Contains("GreenSoldier")) _bodyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies/GreenSoldierBodies");
        if (type.Contains("PurpleSoldier")) _bodyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies/PurpleSoldierBodies");

        _deathSound = Resources.Load<AudioClip>("Audios/Enemies/DeathSound");
        currentHealth = maxHealth;
    }
}


