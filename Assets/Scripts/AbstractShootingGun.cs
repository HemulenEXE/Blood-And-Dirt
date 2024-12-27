using System;
using UnityEngine;

/// <summary>
/// Абстрактный класс огнестрельного ружья.
/// </summary>
public abstract class AbstractShootingGun : MonoBehaviour
{
    /// <summary>
    /// Выстреливаемый из ружия объект.
    /// В качестве такого объекта могут выступать патрон, дробинка и т.п.
    /// </summary>
    [SerializeField] protected GameObject _prefabFiredObject;
    /// <summary>
    /// Скорость вылета выстреливаемого объекта.
    /// </summary>
    public float _speedShot = 40;
    /// <summary>
    /// Наносимый урон.
    /// </summary>
    public float _damage = 5;
    /// <summary>
    /// Задержка между выстрелами.
    /// </summary>
    public float _delayFire = 1;
    /// <summary>
    /// Суммарное число патронов.
    /// </summary>
    public int _ammoTotal = 100;
    /// <summary>
    /// Вместимость обоймы.
    /// </summary>
    public int _capacityAmmo = 5;
    /// <summary>
    /// Текущее число патронов в обойме.
    /// </summary>
    public int _currAmmoTotal = 0;
    /// <summary>
    /// Время перезарядки.
    /// </summary>
    public float _timeRecharging = 5;
    /// <summary>
    /// Флаг, указывающий, идёт ли перезарядка.
    /// </summary>
    public bool _isReloading = false;
    protected virtual void Awake()
    {
        //Проверка полей

        if (_damage < 0) throw new ArgumentNullException("AbstractGun: _damage < 0");
        if (_delayFire < 0) throw new ArgumentNullException("AbstractGun: _delayFire < 0");
        if (_ammoTotal < 0) throw new ArgumentOutOfRangeException("AbstractGun: _ammoTotal < 0");
        if (_capacityAmmo < 0) throw new ArgumentOutOfRangeException("AbstractGun: _capacityAmmo < 0");
        if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("AbstractGun: _timeRecharging < 0");
    }

    /// <summary>
    /// Выстрел из ружья.
    /// </summary>
    public abstract void Shoot();
    /// <summary>
    /// Перезарядка ружья.
    /// </summary>
    public abstract void Recharge();
    /// <summary>
    /// Проверяет, пусто ли ружьё.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsEmpty() => _ammoTotal == 0 && _currAmmoTotal == 0;
}
