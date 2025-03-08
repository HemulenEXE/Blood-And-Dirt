using GunLogic;
using System;
using UnityEngine;

public class HealthBot : AbstractHealth
{
    private GameObject _body;
    private AudioClip _audio;

    public static event Action<BotController> death;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile" && collision.gameObject.layer != LayerMask.NameToLayer("EnemyProjectile"))
        {

            var dataBullet = collision.gameObject.GetComponent<ProjectileData>();
            GetDamage(dataBullet);
        }
    }

    public override void GetDamage(ProjectileData bullet)
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
        death?.Invoke(transform.root.GetComponent<BotController>());
        // this.GetComponent<AudioSource>()?.PlayOneShot(_audio);
        GameObject.Instantiate(_body, this.transform.position, Quaternion.identity);
        Destroy(transform.root.gameObject);

    }

    void Start()
    {
        _body = Resources.Load<GameObject>("Prefabs/Enemies/Body");
        _audio = _audio = Resources.Load<AudioClip>("Audios/death_sound");
        currentHealth = maxHealth;
    }
}


