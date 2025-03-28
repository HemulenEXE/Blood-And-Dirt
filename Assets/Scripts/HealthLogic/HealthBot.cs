using GunLogic;
using System;
using UnityEngine;

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
        SetLayerRecursively(transform.parent.gameObject, LayerMask.NameToLayer("Invisible"));
        death?.Invoke(transform.root.GetComponent<BotController>());
        this.transform.parent.GetComponent<AudioSource>()?.PlayOneShot(_deathSound);
        GameObject.Instantiate(_bodyPrefabs[UnityEngine.Random.Range(0, _bodyPrefabs.Length)], this.transform.position, Quaternion.identity);
        Destroy(transform.root.gameObject, _deathSound.length);

    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
            Destroy(child.GetComponent<Collider2D>());
            Destroy(child.GetComponent<Rigidbody2D>());
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


