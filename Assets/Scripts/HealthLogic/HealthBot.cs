using GunLogic;
using System;
using System.Drawing;
using Grenades;
using UnityEngine;

public class HealthBot : AbstractHealth
{
    [SerializeField] private EnemySides side;
    private string enemyBullet;
    public static event Action<BotController> death;
    private GameObject _body;
    private AudioClip _audio;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProjectileData Bullet = collision.gameObject.GetComponent<ProjectileData>();
        if (Bullet != null)
        {
            if (collision.gameObject.tag == "Projectile" && collision.gameObject.layer != LayerMask.NameToLayer(Bullet.sideBullet.GetOwnLayer()))
            {
                if(Bullet.sideBullet.IsEnemyMask(this.gameObject.layer))
                {
                    GetDamage(Bullet);
                }
                
            }
        }
        
    }

    protected override void GetDamage(int value)
    {
        if (!isInvulnerable)
        {
            currentHealth -= value;

            if (currentHealth <= 0)
            {
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

    public override void GetDamage(SimpleGrenade granade)
    {
        GetDamage((int)granade.DamageExplosion);
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


