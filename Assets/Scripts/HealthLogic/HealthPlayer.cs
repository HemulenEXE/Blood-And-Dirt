using GunLogic;
using System.Collections;
using UnityEngine;
using System;
using CameraLogic.CameraEffects;
using InventoryLogic;

/// <summary>
/// Класс здоровья игрока. Скрипт навешивается на игрока
/// </summary>
public class HealthPlayer : AbstractHealth
{
    [SerializeField]
    private int _damageBleeding = 5;
    [SerializeField]
    private int _countArmor;
    [SerializeField]
    private float _frameDuration = 0.5f; //Сколько длится кадр, во время которого игрок не получает урон
    [SerializeField]
    private int _bandageHalth = 10;
    [SerializeField]
    private int _firstAidKitHalth = 25;

    private BloodEffect bloodController;


    private bool isBlood = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            var dataBullet = collision.gameObject.GetComponent<ProjectileData>();
            GetDamage(dataBullet);
        }
    }

    /// <summary>
    /// Реализует получение урона от снаряда
    /// </summary>
    /// <param name="bullet"></param>
    public override void GetDamage(ProjectileData bullet)
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
    /// <summary>
    /// Высчитывает текущее состояние эффекта крови
    /// </summary>
    /// <returns></returns>
    StateBloodEffect CalculateStateDamaged()
    {
        //print($"Damaged = {Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0))}");
        return (StateBloodEffect)Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && ConsumableCounter.FirstAidKitCount > 0)
        {
            if (currentHealth + _firstAidKitHalth > maxHealth)
                currentHealth = maxHealth;
            else currentHealth += _firstAidKitHalth;

            ConsumableCounter.FirstAidKitCount--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && ConsumableCounter.BandageCount > 0)
        {
            Debug.Log("Применены бинты!");
            if (currentHealth + _bandageHalth > maxHealth)
                currentHealth = maxHealth;
            else currentHealth += _bandageHalth;

            ConsumableCounter.BandageCount--;
        }
    }

    void FixedUpdate()
    {
           
        //Debug.Log("XP = " + currentHealth);
        bloodController.SetBloodEffect(CalculateStateDamaged());
    }
}
