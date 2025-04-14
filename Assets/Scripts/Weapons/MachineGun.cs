using System;
using System.Collections;
using UnityEngine;

public class MachineGun : MonoBehaviour, IGun
{

    [SerializeField] protected GameObject _prefabProjectile;
    /// <summary>
    /// Сила звука
    /// </summary>
    [SerializeField] private float noiseIntensity = 5;
    /// <summary>
    /// Событие отвечающие за передачу источника и силы звука медиатору событий
    /// </summary>
    public static event Action<Transform, float> makeNoiseShooting;

    public static event Action<Transform, string> AudioEvent;


    [SerializeField] protected float attackRange;
    public float AttackRange { get; set; }
    public GunType Type { get; } = GunType.Heavy;
    public bool IsRecharging { get; set; } = false;
    public bool IsShooting { get; set; } = false;

    public bool IsHeld { get; set; } = true;
    [field: SerializeField] public float Damage { get; private set; } = 6.5f;
    [field: SerializeField] public float ShotDelay { get; private set; } = 0.1f;
    [field: SerializeField] public int AmmoTotal { get; private set; } = 100;
    [field: SerializeField] public int AmmoCapacity { get; private set; } = 30;
    [field: SerializeField] public int AmmoTotalCurrent { get; private set; } = 0;
    [field: SerializeField] public float RechargingTime { get; private set; } = 1f;
    [field: SerializeField] public float SpeedProjectile { get; private set; } = 50f;
    public float NoiseIntensity { get; set; }

    /// <summary>
    /// Выполнение механизма стрельбы с создание и корректированием пули.<br/>
    /// </summary>
    /// <remarks>.</remarks>
    /// <exception cref="ArgumentNullException"></exception>
    public void Shoot(Side sideShooter, bool IsPlayerShoot = false)
    {
        Debug.Log("IsShooting: " + IsShooting);
        Debug.Log("IsRecharging: " + IsRecharging);
        if (!IsShooting && !IsRecharging)
        {
            if (AmmoTotalCurrent > 0)
            {
                AudioEvent?.Invoke(this.transform, "fire_machinegun");
                IsShooting = true;

                var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation);


                var projectileData = currentBullet.GetComponent<IBullet>();
                if (projectileData != null)
                {
                    projectileData.sideBullet = sideShooter.CreateSideBullet();
                    projectileData.Damage = this.Damage;
                    projectileData.GunType = Type;
                    projectileData.Speed = SpeedProjectile;
                }


                AmmoTotalCurrent--;
                currentBullet.layer = LayerMask.NameToLayer(sideShooter.GetOwnLayer());
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
    /// Перезарядка
    /// </summary>
    public void Recharge()
    {
        if (AmmoTotal > 0 && !IsShooting && !IsRecharging)
        {
            IsRecharging = true;
            StartCoroutine(RechargeCoroutine());
        }
    }
    /// <summary>
    /// Проверка на пустоту магазина
    /// </summary>
    public bool IsEmpty()
    {
        return AmmoTotal == 0 && AmmoTotalCurrent == 0;
    }
    /// <summary>
    /// Выполняют задежржку скрипта стрельбы во время перезарядки
    /// </summary>
    /// <returns></returns>
    private IEnumerator RechargeCoroutine()
    {
        yield return new WaitForSeconds(RechargingTime);

        if (!IsHeld)
        {
            IsRecharging = false;
            yield break;
        }

        int count_need_patrons = AmmoCapacity - AmmoTotalCurrent;
        AudioEvent?.Invoke(this.transform, "recharge_machinegun");
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
    /// Проверяет, эффективное ли расстояние стрельбы до цели
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public bool IsInRange(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= attackRange;
    }
    protected void Awake()
    {
        if (Damage < 0) throw new ArgumentOutOfRangeException("MachineGun: Damage < 0");
        if (ShotDelay < 0) throw new ArgumentOutOfRangeException("MachineGun: ShotDelay < 0");
        if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("MachineGun: AmmoTotal < 0");
        if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("MachineGun: AmmoCapacity < 0");
        if (RechargingTime < 0) throw new ArgumentOutOfRangeException("MachineGun: RechargingTime < 0");
        if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("MachineGun: AmmoCapacity < AmmoTotalCurrent");
        if (_prefabProjectile == null) throw new ArgumentNullException("MachineGun: _prefabPellet is null");
    }
}