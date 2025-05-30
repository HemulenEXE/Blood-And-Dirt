using GunLogic;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HealthBot : AbstractHealth
{
    [SerializeField] bool explosionProof = false;
    [SerializeField] int resiestExplosion = 3;
    private EnemySides side;
    private string enemyBullet;
    public static event Action<BotController> death;
    private GameObject _body;
    private AudioClip _audio;
    private GameObject[] _bodyPrefabs;
    private AudioClip _deathSound;

    public static event Action<Transform, string> AudioEvent;

    private int deathNum;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IBullet Bullet = collision.gameObject.GetComponent<IBullet>();
        Debug.Log("Init");
        if (Bullet != null)
        {
            Debug.Log("Reg bullet");
            Debug.Log(Bullet.GetType());
            if (collision.gameObject.tag == "Projectile" && collision.gameObject.layer != LayerMask.NameToLayer(Bullet.sideBullet.GetOwnLayer()))
            {
                Debug.Log("Enemy?");
                if (Bullet.sideBullet.IsEnemyMask(this.gameObject.layer))
                {
                    Debug.Log("Hit");
                    GetDamage(Bullet);
                    PlayAnimationHit(collision.gameObject.transform);
                }

            }
        }

    }

    public override void GetDamage(int value)
    {
        if (!isInvulnerable)
        {
            AudioEvent?.Invoke(this.transform, "taking_damage" + UnityEngine.Random.Range(0, 11));
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
            AudioEvent?.Invoke(this.transform, "taking_damage" + UnityEngine.Random.Range(0, 11));
            int damage = bullet.GetType() == typeof(ExplosionBullet) && explosionProof ? (int)(bullet.Damage / resiestExplosion) : (int)bullet.Damage;
            currentHealth -= damage;

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
        PlayAnimationHit(knife.gameObject.transform);
        GetDamage((int)knife.Damage);
    }

    public override void GetDamage(ShrapnelGrenade granade)
    {
        if (!explosionProof)
        {
            GetDamage((int)granade.damageExplosion);
        }
        else
        {
            GetDamage((int)granade.damageExplosion / resiestExplosion);
        }
    }
    public override void Death()
    {
        var parent = this.transform.parent;
        if (parent == null) return;

        if (parent.name.Equals("HenchMan"))
        {
            Destroy(parent);
            return;
        }

        if (parent != null)
        {

            death?.Invoke(parent.GetComponent<BotController>());

            AudioEvent?.Invoke(this.transform, "death_sound" + UnityEngine.Random.Range(0, 6));


            DisableBotComponents(parent.gameObject);

            foreach(Transform x in parent.transform)
            {
                Debug.Log(x.gameObject.name + "<=>" + parent.name);
                x.SetParent(null);
                if (x.gameObject.name.Equals("Visual"))
                {
                    var temp = x.AddComponent<BoxCollider2D>();
                    temp.isTrigger = true;
                    x.AddComponent<Body>();

                    var animator = x.GetComponent<Animator>();

                    deathNum = UnityEngine.Random.Range(0, 2);
                    x.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    string deathTrigger = deathNum == 0 ? "Death1" : "Death2";
                    animator.SetTrigger(deathTrigger);

                    Destroy(x.gameObject, Math.Max(animator.GetCurrentAnimatorStateInfo(0).length, _deathSound.length));
                }
                else if (!x.name.Equals(this.name)) Destroy(x.gameObject);
            }
            Destroy(parent.gameObject, 0.05f);
            Destroy(this.gameObject);
        }
    }

    //private IEnumerator HandleDeathAnimation(GameObject botObject, Animator animator, int prefab)
    //{
    //    // ��� ���� ����, ����� �������� ������ �������������
    //    yield return null;

    //    // �������� ������������ ������� ��������
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    float animationLength = stateInfo.length;

    //    // ��� ��������� �������� (��� ����� � ���� �� ������)
    //    float waitTime = Mathf.Max(animationLength, _deathSound.length);
    //    yield return new WaitForSeconds(waitTime);

    //    // ������� ����
    //    //Instantiate(_bodyPrefabs[prefab], transform.position, Quaternion.identity);

    //    // ������� ������
    //    Destroy(botObject);
    //}
    private void DisableBotComponents(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();
        foreach (var x in colliders) Destroy(x);

        Rigidbody2D[] rigidbodies = obj.GetComponentsInChildren<Rigidbody2D>();
        foreach (var x in rigidbodies) Destroy(x);

        NavMeshAgent[] navMeshAgents = obj.GetComponentsInChildren<NavMeshAgent>();
        foreach (var x in navMeshAgents)
            if (x != null && x.isActiveAndEnabled)
                Destroy(x);


        var components = obj.GetComponents<MonoBehaviour>();
        foreach (var x in components) if (x.GetType() != typeof(AudioSource)) x.enabled = false;
    }

    private void Awake()
    {
        side = GetComponentInParent<Side>().side;

        string type = transform.parent.name;
        if (type.Contains("GreenSoldier")) _bodyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies/GreenSoldierBodies");
        if (type.Contains("PurpleSoldier")) _bodyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies/PurpleSoldierBodies");
        _deathSound = Resources.Load<AudioClip>("Audios/Enemies/DeathAudios/death_sound0");
        currentHealth = maxHealth;

        isInvulnerable = true;
        StartCoroutine(ResetInvulnerability(1));
    }

    private IEnumerator ResetInvulnerability(float countEnv = 1)
    {
        yield return new WaitForSeconds(countEnv);
        isInvulnerable = false;
    }

    public override void PlayAnimationHit(Transform sourceDamage)
    {
        string namePrefab = "Prefabs/HitAnimation/blood" + (UnityEngine.Random.Range(0, 5) + 1);
        Debug.Log(namePrefab);
        GameObject prefamHitAnimation = Resources.Load<GameObject>(namePrefab);
        float offset = 0.1f;
        // 1. �������� ����� ����� � ����������� "����"
        Vector3 entryPoint = sourceDamage.position;
        Vector3 direction = (transform.position - sourceDamage.position).normalized;

        // 2. ���������� ����� �������
        var bounds = GetComponent<BoxCollider2D>().bounds;
        Vector3 center = bounds.center;

        // 3. ������ �� ������ � ����� ����� (����������� ��������� ������)
        Vector3 localHitDirection = (entryPoint - center).normalized;

        // 4. ��������� ����� ������ (��������������� ������� �� ������)
        Vector3 exitPoint = center - localHitDirection * bounds.extents.magnitude;

        // 5. ������� ������� �� ����������� ������
        exitPoint += localHitDirection * offset;

        float angle = Mathf.Atan2(localHitDirection.y, localHitDirection.x) * Mathf.Rad2Deg;

        // ������ �������� ������ Z
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // 6. ������ ������ ������
        GameObject effect = Instantiate(prefamHitAnimation, exitPoint, rotation);

        // 7. ������������� �������� (���� ���� Animator)
        Animator anim = effect.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Play");
            // ������ ����� ������������ �����, ���� ����� �������������� �������
            Destroy(effect, anim.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        }
        else
        {
            // ������ ����� ������������� �����, ���� Animator �� ������
            Destroy(effect, 2f);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}


