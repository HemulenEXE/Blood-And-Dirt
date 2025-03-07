using CameraLogic.CameraEffects;
using GunLogic;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс здоровья игрока. Скрипт навешивается на игрока
/// </summary>
public class PlayerHealth : AbstractHealth
{
    private float _frameDuration = 0.5f; // Сколько длится кадр, во время которого игрок не получает урон

    private BloodEffect bloodController;

    private float _updateStateTimeBleeding = 10f;
    private float _currentTimeBleeding = 0f;

    private int _currentHitsToSurvive;
    private float _updateStateTime = 10f; // Время обновления состояния игрока
    private float _currentTimeHits = 0f;


    public override void GetDamage(ProjectileData bullet)
    {
        if (!PlayerData.IsGod)
        {
            PlayerData.IsBleeding = true;
            PlayerData.CurrentHealth -= (int)bullet.Damage;
            _currentTimeBleeding = _updateStateTimeBleeding;

            if (PlayerData.CurrentHealth <= 0)
            {
                var temp = PlayerData.GetSkill<Reincarnation>();
                if (temp != null && PlayerData.ResurrectionCount > 0)
                {
                    temp.SpawnBody(this.gameObject); // Спавн трупа
                    PlayerData.MaxHealth /= 2;
                    PlayerData.CurrentHealth = PlayerData.MaxHealth;
                    return;
                }
                else
                {
                    Death();
                    return;
                }
            }
            StartCoroutine(InvulnerabilityFrames(_frameDuration));
        }
    }

    private IEnumerator BleedDamage()
    {
        while (true)
        {
            if (PlayerData.IsBleeding && !PlayerData.IsGod)
            {
                PlayerData.CurrentHealth -= PlayerData.BleedingDamage;
            }
            yield return new WaitForSeconds(60f); // Что за 60f?
        }
    }
    /// <summary>
    /// Высчитывает текущее состояние эффекта крови
    /// </summary>
    /// <returns></returns>
    private StateBloodEffect CalculateStateDamaged()
    {
        // print($"Damaged = {Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0))}");
        return (StateBloodEffect)Math.Floor((PlayerData.MaxHealth - PlayerData.CurrentHealth) / (PlayerData.MaxHealth / 5.0));
    }

    private void Start()
    {
        // PlayerInfo.ExecuteSkill("StartOfANewLife", this.gameObject);
        // PlayerInfo.ExecuteSkill("MusclesSecondSkeleton", this.gameObject);

        bloodController = GetComponent<BloodEffect>();
        StartCoroutine(BleedDamage());
    }

    private void Update()
    {
        if (_currentTimeHits <= 0)
        {
            _currentTimeHits = _updateStateTime;
            _currentHitsToSurvive = PlayerData.HitsToSurvive; // Обновление количества пропускаемых ударов
        }
        _currentTimeHits -= Time.deltaTime;

        if (_currentTimeBleeding <= 0)
        {
            _currentTimeBleeding = _updateStateTimeBleeding;
            PlayerData.IsBleeding = false; // Обновление состояния кровотечения
        }
        _currentTimeBleeding -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha3) && PlayerData.FirstAidKitCount > 0)
        {
            if (PlayerData.CurrentHealth + PlayerData.FirstAidKitHealth > PlayerData.MaxHealth)
                PlayerData.CurrentHealth = PlayerData.MaxHealth;
            else PlayerData.CurrentHealth += PlayerData.FirstAidKitHealth;

            --PlayerData.FirstAidKitCount;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && PlayerData.BandageCount > 0)
        {
            if (PlayerData.CurrentHealth + PlayerData.BandageHealth > PlayerData.MaxHealth)
                PlayerData.CurrentHealth = PlayerData.MaxHealth;
            else PlayerData.CurrentHealth += PlayerData.BandageHealth;

            --PlayerData.BandageCount;
        }
    }
    void FixedUpdate()
    {
        bloodController?.SetBloodEffect(CalculateStateDamaged());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            if (_currentHitsToSurvive > 0)
            {
                --_currentHitsToSurvive;
                _currentTimeHits = _updateStateTime;
            }
            else
            {
                var dataBullet = collision.gameObject.GetComponent<ProjectileData>();
                GetDamage(dataBullet);
            }
        }
    }
}
