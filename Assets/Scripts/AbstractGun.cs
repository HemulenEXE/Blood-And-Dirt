using System;
using UnityEngine;

/// <summary>
/// Абстрактный класс оружия.
/// </summary>
public abstract class AbstractGun : MonoBehaviour
{
    /// <summary>
    /// Наносимый оружием урон.
    /// </summary>
    public float _damage;
    /// <summary>
    /// Задержка между выстрелами.
    /// </summary>
    public float _delayFire;
    /// <summary>
    /// Суммарное число патронов.
    /// </summary>
    public int _ammoTotal;
    /// <summary>
    /// Вместимость патронов в стволе.
    /// Максимальное число патронов, помещённых в ствол оружия.
    /// </summary>
    public int _capacityAmmo;
    /// <summary>
    /// Текущее число патронов в стволе.
    /// </summary>
    public int _currAmmoTotal;
    /// <summary>
    /// Время перезарядки.
    /// </summary>
    public float _timeRecharging;
    /// <summary>
    /// Флаг, указывающий, идёт ли перезарядка.
    /// </summary>
    public bool _isReloading;

    private void Awake()
    {
        //Проверка полей
        if (_damage < 0) throw new ArgumentNullException("AbstractGun: _damage < 0");
        if (_delayFire < 0) throw new ArgumentNullException("AbstractGun: _delayFire < 0");
        if (_ammoTotal < 0) throw new ArgumentOutOfRangeException("AbstractGun: _ammoTotal < 0");
        if (_capacityAmmo < 0) throw new ArgumentOutOfRangeException("AbstractGun: _capacityAmmo < 0");
        if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("AbstractGun: _timeRecharging < 0");
    }

    /// <summary>
    /// Выстрел из оружия.
    /// </summary>
    public abstract void Shoot();
    /// <summary>
    /// Перезарядка оружия.
    /// </summary>
    public abstract void Recharge();
    /// <summary>
    /// Проверяет, есть ли патроны в оружии.
    /// </summary>
    /// <returns></returns>
    public abstract bool IsEmpty();
}
