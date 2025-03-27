﻿using GunLogic;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс, реализующий "пистолет".
/// </summary>
public class Pistol : MonoBehaviour, IGun
{
    [SerializeField] private float noiseIntensity = 5;
    public static event Action<Transform, float> makeNoiseShooting; // Событие вызова реакции на шум стрельбы

    [SerializeField] protected GameObject _prefabProjectile;

    [SerializeField] private AudioSource _audioControl;
    [SerializeField] private AudioClip _audioFire;
    [SerializeField] private AudioClip _audioRecharge;
    [SerializeField] private AudioClip _audioPlatoon;

    public GunType Type { get; } = GunType.Light;
    public bool IsHeld { get; set; } = true;
    [field: SerializeField] public float Damage { get; private set; } = 2f;
    [field: SerializeField] public float ShotDelay { get; private set; } = 0.2f;
    public float SpeedProjectile { get; private set; } = 50f;
    [field: SerializeField] public int AmmoTotal { get; private set; } = 100;
    [field: SerializeField] public int AmmoCapacity { get; private set; } = 10;
    [field: SerializeField] public int AmmoTotalCurrent { get; private set; } = 0;
    [field: SerializeField] public float RechargingTime { get; private set; } = 1f;
    public bool IsRecharging { get; set; } = false;
    public bool IsShooting { get; set; } = false;
    public float NoiseIntensity { get; set; }


    public void Shoot(Side sideShooter, bool IsPlayerShoot = false)
    {
        if (!IsShooting && !IsRecharging)
        {
            if (AmmoTotalCurrent > 0)
            {
                _audioControl.PlayOneShot(_audioFire);
                IsShooting = true;

                var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); // Вылет снаряда
                currentBullet.layer = LayerMask.NameToLayer(sideShooter.GetOwnLayer());
                AmmoTotalCurrent--;

                var projectileData = currentBullet.GetComponent<IBullet>();
                if (projectileData != null)
                {
                    projectileData.sideBullet = sideShooter.CreateSideBullet();
                    projectileData.Damage = Damage;
                    projectileData.GunType = Type;
                    projectileData.Speed = SpeedProjectile;
                }

                IsShooting = false;

                //if (IsPlayerShoot)
                //{
                    makeNoiseShooting?.Invoke(transform, noiseIntensity);
                //}
            }
            else Recharge();
        }
    }
    /// <summary>
    /// Перезарядка пистолета.
    /// </summary>
    public void Recharge()
    {
        if (AmmoTotal > 0 && !IsRecharging && !IsShooting)
        {
            IsRecharging = true;
            StartCoroutine(RechargeCoroutine());
        }
    }
    public bool IsEmpty()
    {
        return AmmoTotal == 0 && AmmoTotalCurrent == 0;
    }
    private IEnumerator RechargeCoroutine()
    {
        yield return new WaitForSeconds(RechargingTime);

        if (!IsHeld)
        {
            IsRecharging = false;
            yield break;
        }

        int count_need_patrons = AmmoCapacity - AmmoTotalCurrent; //Количество нехватаемых патронов.
        _audioControl.PlayOneShot(_audioRecharge);
        if (AmmoTotal > count_need_patrons)
        {
            AmmoTotalCurrent += count_need_patrons;
            AmmoTotal -= count_need_patrons;
        }
        else
        {
            AmmoTotalCurrent += AmmoTotal;
            AmmoTotal = 0;
        }
        IsRecharging = false;
    }
    /// <summary>
    /// Настройка и проверка полей.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    protected void Awake()
    {
        _audioControl = this.GetComponent<AudioSource>();

        if (Damage < 0) throw new ArgumentOutOfRangeException("Pistol: Damage < 0");
        if (ShotDelay < 0) throw new ArgumentOutOfRangeException("Pistol: ShotDelay < 0");
        if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("Pistol: AmmoTotal < 0");
        if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("Pistol: AmmoCapacity < 0");
        if (RechargingTime < 0) throw new ArgumentOutOfRangeException("Pistol: RechargingTime < 0");
        if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("Pistol: AmmoCapacity < AmmoTotalCurrent");
        if (_prefabProjectile == null) throw new ArgumentNullException("Pistol: _prefabPellet is null");
        if (_audioControl == null) throw new ArgumentNullException("Pistol: _audioControl is null");
        if (_audioFire == null) throw new ArgumentNullException("Pistol: _audioFire is null");
        if (_audioRecharge == null) throw new ArgumentNullException("Pistol: _audioRecharge is null");
    }
    public bool IsInRange(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= attackRange;
        }
}
