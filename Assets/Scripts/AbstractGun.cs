using System;
using UnityEngine;

/// <summary>
/// Абстрактный класс, реализующий "ружьё".<br/>
/// Ружьё может быть как огнестрельным, так и распыляющим.
/// </summary>
public abstract class AbstractGun : MonoBehaviour
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
    [SerializeField] protected float _delayShot = 0;
    /// <summary>
    /// Суммарное число снарядов.
    /// </summary>
    [SerializeField] protected int _ammoTotal = 100;
    /// <summary>
    /// Возвращает и изменяет величину суммарного числа снарядов.
    /// </summary>
    public int AmmoTotal
    {
        get => _ammoTotal;
        set
        {
            _ammoTotal += value;
            if (_ammoTotal < 0) _ammoTotal = 0;
        }
    }
    /// <summary>
    /// Вместимость очереди.
    /// </summary>
    [SerializeField] protected int _ammoCapacity = 5;
    /// <summary>
    /// Возвращает величину вместимости очереди.
    /// </summary>
    public int AmmoCapacity { get => _ammoCapacity; }
    /// <summary>
    /// Текущее число снарядов в очереди.
    /// </summary>
    [SerializeField] protected int _ammoTotalCurrent = 0;
    /// <summary>
    /// Возвращает величину текущего числа снарядов в очереди.
    /// </summary>
    public int AmmoTotalCurrent
    {
        get => _ammoTotalCurrent;
        set
        {
            _ammoTotalCurrent += value;
            if (_ammoTotalCurrent < 0) _ammoTotalCurrent = 0;
        }
    }
    /// <summary>
    /// Время перезарядки.
    /// </summary>
    [SerializeField] protected float _timeRecharging = 5;
    /// <summary>
    /// Флаг, указывающий, идёт ли перезарядка.
    /// </summary>
    protected bool _isReloading = false;
    /// <summary>
    /// Возвращает и изменяет флаг, указывающий, идёт ли перезарядка.
    /// </summary>
    public bool IsReloading { get => _isReloading; set => _isReloading = value; }
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected virtual void Awake()
    {
        if (_damage < 0) throw new ArgumentOutOfRangeException("AbstractGun: _damage < 0");
        if (_delayShot < 0) throw new ArgumentOutOfRangeException("AbstractGun: _delayFire < 0");
        if (_ammoTotal < 0) throw new ArgumentOutOfRangeException("AbstractGun: _ammoTotal < 0");
        if (_ammoCapacity < 0) throw new ArgumentOutOfRangeException("AbstractGun: _capacityAmmo < 0");
        if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("AbstractGun: _timeRecharging < 0");
        if (_ammoCapacity < _ammoTotalCurrent) throw new ArgumentOutOfRangeException("AbstractGun: _ammoCapacity < _ammoTotalCurrent");
    }
    /// <summary>
    /// Выстрел из ружья.
    /// </summary>
    public abstract void Shoot();
    /// <summary>
    /// Остановка стрельбы из ружья.
    /// </summary>
    public abstract void StopShoot();
    /// <summary>
    /// Перезарядка ружья.
    /// </summary>
    public abstract void Recharge();
    /// <summary>
    /// Проверяет, пусто ли ружьё.
    /// </summary>
    public virtual bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
}
