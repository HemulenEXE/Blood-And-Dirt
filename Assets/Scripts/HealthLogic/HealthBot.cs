using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gun;
using System;

public class HealthBot : AbstractHealth
{
    public static event Action<BotController> death;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Col");
        if (collision.gameObject.tag == "Projectile" && collision.gameObject.layer != LayerMask.NameToLayer("EnemyProjectile"))
        {
            
            var dataBullet = collision.gameObject.GetComponent<ProjectileData>();
            GetDamge(dataBullet);
        }
    }

    public override void GetDamge(ProjectileData bullet)
    {
        if (!isInvulnerable)
        {
            Debug.Log("check");
            currentHealth -= (int)bullet.Damage;

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
        Debug.Log(currentHealth);
    }
}


