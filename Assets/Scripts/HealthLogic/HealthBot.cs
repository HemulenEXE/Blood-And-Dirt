using UnityEngine;
using GunLogic;
using System;

public class HealthBot : AbstractHealth
{
    [SerializeField] private EnemySides side;
    private string enemyBullet;
    public static event Action<BotController> death;
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
        Destroy(transform.root.gameObject);

    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        //Debug.Log(currentHealth);
    }
}


