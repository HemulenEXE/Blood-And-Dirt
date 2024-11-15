using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс, реализующий "дробовик".
/// </summary>
public class ShotGun : MonoBehaviour, IGun
{
    /// <summary>
    /// Наносимый урон.
    /// </summary>
    [SerializeField] protected float _damage = 5;
    /// <summary>
    /// Возвращает величину наносимого урона.
    /// </summary>
    public float Damage { get => _damage; }
    /// <summary>
    /// Задержка между выстрелами.
    /// </summary>
    [SerializeField] protected float _delayShot = 2;
    /// <summary>
    /// Время до следующего выстрела.
    /// </summary>
    protected float _nextTimeShot = 0f;
    /// <summary>
    /// Суммарное число снарядов.
    /// </summary>
    [SerializeField] protected int _ammoTotal = 100;
    /// <summary>
    /// Возвращает и изменяет суммарное число снарядов.
    /// </summary>
    public int AmmoTotal
    {
        get => _ammoTotal;
        set
        {
            if (value <= 0) _ammoTotal = 0;
            else _ammoTotal = value;
        }
    }
    /// <summary>
    /// Вместимость очереди.
    /// </summary>
    [SerializeField] protected int _ammoCapacity = 5;
    /// <summary>
    /// Возвращает вместимость очереди.
    /// </summary>
    public int AmmoCapacity { get => _ammoCapacity; }
    /// <summary>
    /// Текущее число снарядов в очереди.
    /// </summary>
    [SerializeField] protected int _ammoTotalCurrent = 0;
    /// <summary>
    /// Возвращает текущее число снарядов в очереди.
    /// </summary>
    public int AmmoTotalCurrent
    {
        get => _ammoTotalCurrent;
        set
        {
            if (value <= 0) _ammoTotalCurrent = 0;
            else _ammoTotalCurrent = value;
        }
    }
    /// <summary>
    /// Время перезарядки.
    /// </summary>
    [SerializeField] protected float _timeRecharging = 5;
    /// <summary>
    /// Возврашает флаг, указывающий, идёт ли перезарядка.
    /// </summary>
    public bool IsRecharging { get; set; } = false;
    /// <summary>
    /// Возврашает флаг, указывающий, идёт ли стрельба.
    /// </summary>
    public bool IsShooting { get; set; } = false;
    /// <summary>
    /// Префаб дробинки, вылетающий из дробовика.
    /// </summary>
    [SerializeField] protected GameObject _prefabProjectile;
    /// <summary>
    /// Количество вылетающих дробинок при одном выстреле.
    /// </summary>
    [SerializeField] protected int _countProjectile = 3;
    /// <summary>
    /// Начальная скорость вылета дробинки.
    /// </summary>
    [SerializeField] protected float _speedProjectile = 5f;
    /// <summary>
    /// Угол распространения дробинок при выстреле.<br/>
    /// Максимальный угол отклонения между двумя векторами, вдоль которых летят дробинки.
    /// </summary>
    [SerializeField] protected float _spreadAngle = 15f;
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void Awake()
    {
        if (Damage < 0) throw new ArgumentOutOfRangeException("AbstractGun: _damage < 0");
        if (_delayShot < 0) throw new ArgumentOutOfRangeException("AbstractGun: _delayFire < 0");
        if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("AbstractGun: _ammoTotal < 0");
        if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("AbstractGun: _capacityAmmo < 0");
        if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("AbstractGun: _timeRecharging < 0");
        if (AmmoCapacity < _ammoTotalCurrent) throw new ArgumentOutOfRangeException("AbstractGun: _ammoCapacity < _ammoTotalCurrent");
        if (_prefabProjectile == null) throw new ArgumentNullException("ShotGun: _prefabPellet is null");
        if (_countProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _countFlyingPellets < 0");
        if (_speedProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _speedShot < 0");
    }
    /// <summary>
    /// Выстрел из дробовика.<br/>
    /// </summary>
    /// <remarks>Порожает на сцене снаряд, вылетающий из дробовика.</remarks>
    /// <exception cref="ArgumentNullException"></exception>
    public void Shoot()
    {
        if (!IsShooting && !IsRecharging && Time.time > _nextTimeShot)
        {
            if (AmmoTotalCurrent > 0)
            {
                IsShooting = true;
                _nextTimeShot = Time.time + _delayShot;
                for (int i = 1; i <= _countProjectile; i++) //Механика вылета дробинок.
                {
                    float interim_spread_angle = UnityEngine.Random.Range(-_spreadAngle, _spreadAngle); //Определение угла распространения текущего снаряда.
                    Vector3 direction = this.transform.forward * Mathf.Cos(interim_spread_angle); //Определение направления движения снаряда.
                    GameObject currentPellet = Instantiate(_prefabProjectile, this.transform.position, this.transform.rotation); //Вылет снаряда.
                    currentPellet.transform.Rotate(0, 0, interim_spread_angle); //Поворот снаряда.
                    Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                    if (rg == null) throw new ArgumentNullException("ShotGun: _prefabProjectile hasn't got Rigidbody2D");
                    rg.velocity = currentPellet.transform.right * _speedProjectile;
                }
                AmmoTotalCurrent--;
                IsShooting = false;
            }
            else Recharge();
        }
    }
    /// <summary>
    /// Остановка стрельбы из дробовика.<br/>
    /// Не содержит реализации.
    /// </summary>
    public void StopShoot() { }
    /// <summary>
    /// Перезарядка дробовика.
    /// </summary>
    public void Recharge()
    {
        if (_ammoTotal > 0 && !IsRecharging)
        {
            IsRecharging = true;
            IsShooting = false;
            StartCoroutine(RechargeCoroutine()); //На перезарядку отводится некоторое время.
        }
    }
    /// <summary>
    /// Корутина для перезарядки дробовика.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RechargeCoroutine()
    {
        while (AmmoTotalCurrent != AmmoCapacity)
        {
            AmmoTotal--;
            AmmoTotalCurrent++;
            yield return new WaitForSeconds(_timeRecharging / AmmoCapacity); //Игрок может выстрелить до полной перезарядки ружья.
        }
        IsRecharging = false;
    }
    /// <summary>
    /// Проверяет, пусто ли ружьё.
    /// </summary>
    public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
}
