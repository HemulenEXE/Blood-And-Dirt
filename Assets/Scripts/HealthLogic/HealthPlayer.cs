using Gun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthPlayer : AbstractHealth
{
    [SerializeField]
    private int _damageBleeding = 5;
    [SerializeField]
    private int _countArmor;
    [SerializeField]
    private float _frameDuration = 0.5f;

    private BloodEffect bloodController;


    private bool isBlood = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            var dataBullet = collision.gameObject.GetComponent<ProjectileData>();
            GetDamge(dataBullet);
        }
    }


    public override void GetDamge(ProjectileData bullet)
    {
        if (!isInvulnerable)
        {
            isBlood = true;
            currentHealth -= (int)bullet.Damage;

            if (currentHealth <= 0)
            {
                Death();
                return;
            }

            StartCoroutine(InvulnerabilityFrames(_frameDuration));
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        bloodController = GetComponent<BloodEffect>();
        StartCoroutine(BleedDamage());
    }

    private IEnumerator BleedDamage()
    {
        while (true)
        {
            if (isBlood && !isInvulnerable)
            {
                currentHealth -= _damageBleeding;
            }
            yield return new WaitForSeconds(60f);
        }

    }

    StateBloodEffect CalculateStateDamaged()
    {
        print($"Damaged = {Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0))}");
        return (StateBloodEffect)Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0));
    }

    
    void FixedUpdate()
    {
        Debug.Log("XP = " + currentHealth);
        bloodController.SetBloodEffect(CalculateStateDamaged());
    }
}
