using System;
using System.Collections;
using UnityEngine;

public class ShotGun : MonoBehaviour, IGun
{
    [SerializeField] private float noiseIntensity = 5;
    public static event Action<Transform, float> makeNoiseShooting; // Событие вызова реакции на шум стрельбы

    [SerializeField] protected GameObject _prefabProjectile;
    [SerializeField] protected int _countPerShotProjectile = 6;
    [SerializeField] protected float _spreadAngle = 15f;


    public static event Action<Transform, string> AudioEvent;

    public bool IsHeld { get; set; } = true;
    public GunType Type { get; } = GunType.Heavy;
    public bool IsRecharging { get; set; } = false;
    public bool IsShooting { get; set; } = false;

    [field: SerializeField] public float Damage { get; private set; } = 5f;
    [field: SerializeField] public float ShotDelay { get; private set; } = 0.5f;
    public float SpeedProjectile { get; private set; } = 50f;
    [field: SerializeField] public int AmmoTotal { get; private set; } = 100;
    [field: SerializeField] public int AmmoCapacity { get; private set; } = 5;
    [field: SerializeField] public int AmmoTotalCurrent { get; private set; } = 0;
    [field: SerializeField] public float RechargingTime { get; private set; } = 2;
    [field: SerializeField] public float AttackRange { get; set; } = 15;
    public float NoiseIntensity { get; set; }

    /// <summary>
    /// Выстрел из дробовика.<br/>
    /// </summary>
    /// <remarks>Порожает на сцене снаряд, вылетающий из дробовика.</remarks>
    /// <exception cref="ArgumentNullException"></exception>
    public void Shoot(Side sideShooter, bool IsPlayerShoot = false)
    {
        if (!IsShooting && !IsRecharging)
        {
            if (AmmoTotalCurrent > 0)
            {

                IsShooting = true;
                AudioEvent?.Invoke(this.transform, "fire_shotgun_audio");

                for (int i = 1; i <= _countPerShotProjectile; ++i) // Вылет дробинок
                {
                    float interim_spread_angle = UnityEngine.Random.Range(-_spreadAngle, _spreadAngle); //Определение угла распространения текущего снаряда.
                    Vector3 direction = this.transform.forward * Mathf.Cos(interim_spread_angle); //Определение направления движения снаряда.

                    var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                    GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); //Вылет дробинки.
                    currentBullet.layer = LayerMask.NameToLayer(sideShooter.GetOwnLayer());
                    currentBullet.transform.Rotate(0, 0, interim_spread_angle); //Поворот снаряда.

                    var projectileData = currentBullet.GetComponent<IBullet>();
                    if (projectileData != null)
                    {
                        projectileData.sideBullet = sideShooter.CreateSideBullet();
                        projectileData.Damage = Damage;
                        projectileData.GunType = Type;
                        projectileData.Speed = SpeedProjectile;
                    }
                }
                AmmoTotalCurrent--;
                IsShooting = false;
                //if (IsPlayerShoot)
                //{
                makeNoiseShooting?.Invoke(transform, noiseIntensity);
                //}
            }
            else Recharge();
        }
    }
    public void Recharge()
    {
        if (AmmoTotalCurrent != AmmoCapacity && AmmoTotal > 0 && !IsRecharging && !IsShooting)
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
        while (AmmoTotalCurrent < AmmoCapacity)
        {
            AudioEvent?.Invoke(this.transform, "recharge_shotgun_audio");
            AmmoTotal--;
            AmmoTotalCurrent++;
            yield return new WaitForSeconds(RechargingTime / AmmoCapacity);
            if (!IsHeld)
            {
                IsRecharging = false;
                yield break;
            }
        }
        AudioEvent?.Invoke(this.transform, "platoon_shotgun_audio");
        IsRecharging = false;
    }
    protected void Awake()
    {
        if (Damage < 0) throw new ArgumentOutOfRangeException("ShotGun: Damage < 0");
        if (ShotDelay < 0) throw new ArgumentOutOfRangeException("ShotGun: ShotDelay < 0");
        if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("ShotGun: AmmoTotal < 0");
        if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("ShotGun: AmmoCapacity < 0");
        if (RechargingTime < 0) throw new ArgumentOutOfRangeException("ShotGun: RechargingTime < 0");
        if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("ShotGun: AmmoCapacity < AmmoTotalCurrent");
        if (_prefabProjectile == null) throw new ArgumentNullException("ShotGun: _prefabPellet is null");
        if (_countPerShotProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _countFlyingPellets < 0");
        if (SpeedProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: SpeedProjectile < 0");
    }

    /// <summary>
    /// Проверяет, эффективное ли расстояние стрельбы до цели
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public bool IsInRange(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= AttackRange;
    }
}