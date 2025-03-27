using System;
using System.Collections;
using UnityEngine;
using Grenades;
using GunLogic;
using CameraLogic.CameraEffects;
/// <summary>
/// Класс здоровья игрока. Скрипт навешивается на игрока
/// </summary>
public class PlayerHealth : AbstractHealth
{
    
    private float _frameDuration = 0.5f; // Сколько длится кадр, во время которого игрок не получает урон

    private BloodEffect bloodController;
    protected override void GetDamage(int value)
    {
        if (PlayerData.IsGod) return;

        PlayerData.IsBleeding = true;
        PlayerData.CurrentHealth -= value;

        PlayerData.GetSkill<IncreasedMetabolism>()?.Execute(this.gameObject);
        PlayerData.GetSkill<IncreasedMetabolism>()?.RebootTimer();

        if (PlayerData.CurrentHealth <= 0) HandleDeath();

        StartCoroutine(InvulnerabilityFrames(_frameDuration));
    }
    public override void GetDamage(IBullet bullet)
    {
        GetDamage((int)bullet.Damage);
    }
    public override void GetDamage(ProjectileData bullet)
    {
        GetDamage((int)bullet.Damage);
    }


    public override void GetDamage(SimpleGrenade grenade)
    {
        GetDamage((int)grenade.DamageExplosion);
    }
    public override void GetDamage(ShrapnelGrenade granade)
    {
        GetDamage((int)granade.damageExplosion);
    }
    private void HandleDeath()
    {
        var temp = PlayerData.GetSkill<Reincarnation>();
        if (temp != null && PlayerData.CurrentResurrectionCount > 0)
        {
            temp.SpawnBody(this.gameObject); // Спавн трупа
            return;
        }
        else Death();
    }

    private IEnumerator BleedDamage()
    {
        while (true)
        {
            if (PlayerData.IsBleeding && !PlayerData.IsGod)
                PlayerData.CurrentHealth -= PlayerData.BleedingDamage;
            yield return new WaitForSeconds(60f); // Что за магическое число?
        }
    }
    /// <summary>
    /// Высчитывает текущее состояние эффекта крови
    /// </summary>
    /// <returns></returns>
    private StateBloodEffect CalculateStateDamaged()
    {
        return (StateBloodEffect)Math.Floor((PlayerData.MaxHealth - PlayerData.CurrentHealth) / (PlayerData.MaxHealth / 5.0));
    }

    public void Awake()
    {
        bloodController = GetComponent<BloodEffect>();
        Debug.Log(bloodController);
        StartCoroutine(BleedDamage());
    }

    private void Update()
    {
        if (Input.GetKeyDown(SettingData.FirstAidKit) && PlayerData.FirstAidKitCount > 0)
        {
            if (PlayerData.CurrentHealth + PlayerData.FirstAidKitHealth > PlayerData.MaxHealth)
                PlayerData.CurrentHealth = PlayerData.MaxHealth;
            else PlayerData.CurrentHealth += PlayerData.FirstAidKitHealth;

            --PlayerData.FirstAidKitCount;
        }

        if (Input.GetKeyDown(SettingData.Bandage) && PlayerData.BandageCount > 0)
        {
            if (PlayerData.CurrentHealth + PlayerData.BandageHealth > PlayerData.MaxHealth)
                PlayerData.CurrentHealth = PlayerData.MaxHealth;
            else PlayerData.CurrentHealth += PlayerData.BandageHealth;

            --PlayerData.BandageCount;
        }

        PlayerData.GetSkill<InevitableDeath>()?.Execute(this.gameObject);
    }
    private void FixedUpdate()
    {
        bloodController.SetBloodEffect(CalculateStateDamaged());
        Debug.Log(PlayerData.CurrentHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            var dataBullet = collision.gameObject.GetComponent<IBullet>();
            GetDamage(dataBullet);
        }
    }
}
