using GunLogic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HealthBot : AbstractHealth
{
    private EnemySides side;
    private string enemyBullet;
    public static event Action<BotController> death;
    private GameObject _body;
    private AudioClip _audio;
    private GameObject[] _bodyPrefabs;
    private AudioClip _deathSound;

    private int deathNum;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IBullet Bullet = collision.gameObject.GetComponent<IBullet>();
        
        if (Bullet != null)
        {
            if (collision.gameObject.tag == "Projectile" && collision.gameObject.layer != LayerMask.NameToLayer(Bullet.sideBullet.GetOwnLayer()))
            {
                if (Bullet.sideBullet.IsEnemyMask(this.gameObject.layer))
                {
                    //Debug.Log("Col");
                    GetDamage(Bullet);
                }

            }
        }

    }

    public override void GetDamage(int value)
    {
        if (!isInvulnerable)
        {
            currentHealth -= value;

            if (currentHealth <= 0)
            {
                Debug.Log(value);
                Death();
                return;
            }
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
    public override void GetDamage(ProjectileData bullet)
    {
        GetDamage((int)bullet.Damage);
    }

    public void GetDamage(Knife knife)
    {
        GetDamage((int)knife.Damage);
    }

    public override void GetDamage(ShrapnelGrenade granade)
    {
        GetDamage((int)granade.damageExplosion);
    }

    public override void Death()
    {
        DisableBotComponents(this.transform.parent.gameObject);

        death?.Invoke(transform.parent.GetComponent<BotController>());

        this.transform.parent.GetComponent<AudioSource>()?.PlayOneShot(_deathSound);

        var animator = this.transform.parent.GetComponentInChildren<Animator>();
        deathNum = UnityEngine.Random.Range(0, 2);
        string deathTrigger =  deathNum == 0 ? "Death1" : "Death2";
        animator.SetTrigger(deathTrigger);

        

        Destroy(transform.parent.gameObject, Math.Max(animator.GetCurrentAnimatorStateInfo(0).length, _deathSound.length));
    }

    //private IEnumerator HandleDeathAnimation(GameObject botObject, Animator animator, int prefab)
    //{
    //    // Ждём один кадр, чтобы анимация начала проигрываться
    //    yield return null;

    //    // Получаем длительность текущей анимации
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    float animationLength = stateInfo.length;

    //    // Ждём окончания анимации (или звука — если он дольше)
    //    float waitTime = Mathf.Max(animationLength, _deathSound.length);
    //    yield return new WaitForSeconds(waitTime);

    //    // Спавним тело
    //    //Instantiate(_bodyPrefabs[prefab], transform.position, Quaternion.identity);

    //    // Удаляем объект
    //    Destroy(botObject);
    //}
    private void OnDestroy()
    {
        GameObject.Instantiate(_bodyPrefabs[deathNum], this.transform.position, Quaternion.identity);
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
        side = GetComponentInParent<Side>().side;
        string type = transform.parent.name;
        if (type.Contains("GreenSoldier")) _bodyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies/GreenSoldierBodies");
        if (type.Contains("PurpleSoldier")) _bodyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies/PurpleSoldierBodies");

        _deathSound = Resources.Load<AudioClip>("Audios/Enemies/DeathSound");
        currentHealth = maxHealth;

        isInvulnerable = true;
        StartCoroutine(ResetInvulnerability(1));
    }

    private IEnumerator ResetInvulnerability(float countEnv = 1)
    {
        yield return new WaitForSeconds(countEnv);
        isInvulnerable = false;
    }
}


