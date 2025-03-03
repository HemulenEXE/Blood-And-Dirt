using UnityEngine;
using GunLogic;
using System;

public class HealthBot : AbstractHealth
{
    public static event Action<BotController> death;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Col");
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
            Debug.Log("check");
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


