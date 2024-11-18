using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс, реализующий "пистолет".
/// </summary>
public class Pistol : MonoBehaviour, IGun
{
    /// <summary>
    /// Префаб пули, вылетающий из дробовика.
    /// </summary>
    [SerializeField] protected GameObject _prefabProjectile;
    /// <summary>
    /// Компонент, управляющий вызовами звуков.
    /// </summary>
    protected AudioSource _audio;
    /// <summary>
    /// Звук выстрела.
    /// </summary>
    [SerializeField] protected AudioClip _audioFire;
    /// <summary>
    /// Звук перезарядки.
    /// </summary>
    [SerializeField] protected AudioClip _audioRecharge;
    /// <summary>
    /// Наносимый урон.
    /// </summary>
    [SerializeField] protected float _damage = 2f;
    /// <summary>
    /// Возвращает величину наносимого урона.
    /// </summary>
    public float Damage { get => _damage; }
    /// <summary>
    /// Задержка между выстрелами.
    /// </summary>
    [SerializeField] protected float _delayShot = 1f;
    /// <summary>
    /// Время до следующего выстрела.
    /// </summary>
    protected float _nextTimeShot = 0f;
    /// <summary>
    /// Суммарное число снарядов.
    /// </summary>
    [SerializeField] protected int _ammoTotal = 100;
    /// <summary>
    /// Возвращает и изменяет суммарное число патронов.
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
    [SerializeField] protected int _ammoCapacity = 10;
    /// <summary>
    /// Возвращает вместимость очереди.
    /// </summary>
    public int AmmoCapacity { get => _ammoCapacity; }
    /// <summary>
    /// Текущее число патронов в очереди.
    /// </summary>
    [SerializeField] protected int _ammoTotalCurrent = 0;
    /// <summary>
    /// Возвращает текущее число патронов в очереди.
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
    [SerializeField] protected float _timeRecharging = 2;
    /// <summary>
    /// Возврашает флаг, указывающий, идёт ли перезарядка.
    /// </summary>
    public bool IsRecharging { get; set; } = false;
    /// <summary>
    /// Возврашает флаг, указывающий, идёт ли стрельба.
    /// </summary>
    public bool IsShooting { get; set; } = false;
    /// <summary>
    /// Начальная скорость вылета пули.
    /// </summary>
    [SerializeField] protected float _speedProjectile = 30f;
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void Awake()
    {
        _audio = this.GetComponent<AudioSource>();
        if (_audio.Equals(null)) throw new ArgumentNullException("Pistol: _audio is null");
        if (Damage < 0) throw new ArgumentOutOfRangeException("Pistol: _damage < 0");
        if (_delayShot < 0) throw new ArgumentOutOfRangeException("Pistol: _delayFire < 0");
        if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("Pistol: _ammoTotal < 0");
        if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("Pistol: _capacityAmmo < 0");
        if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("Pistol: _timeRecharging < 0");
        if (AmmoCapacity < _ammoTotalCurrent) throw new ArgumentOutOfRangeException("Pistol: _ammoCapacity < _ammoTotalCurrent");
        if (_prefabProjectile.Equals(null)) throw new ArgumentNullException("Pistol: _prefabPellet is null");
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
                _audio.Stop();
                _audio.PlayOneShot(_audioFire);
                IsShooting = true;
                _nextTimeShot = Time.time + _delayShot;

                GameObject currentPellet = Instantiate(_prefabProjectile, this.transform.GetChild(0).position, this.transform.GetChild(0).rotation); //Вылет снаряда.

                Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                if (rg == null) throw new ArgumentNullException("Pistol: _prefabProjectile hasn't got Rigidbody2D");
                rg.velocity = currentPellet.transform.right * _speedProjectile;

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
            _audio.Stop();
            StartCoroutine(RechargeCoroutine()); //На перезарядку отводится некоторое время.
        }
    }
    /// <summary>
    /// Корутина для перезарядки дробовика.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RechargeCoroutine()
    {
        _audio.PlayOneShot(_audioRecharge);
        yield return new WaitForSeconds(_timeRecharging / AmmoCapacity);
        AmmoTotalCurrent -= AmmoCapacity - AmmoTotalCurrent;
        AmmoTotalCurrent = AmmoCapacity;
        IsRecharging = false;
    }
    /// <summary>
    /// Проверяет, пусто ли ружьё.
    /// </summary>
    public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
}
