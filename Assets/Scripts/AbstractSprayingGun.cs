using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Абстрактный класс распыляющего ружья.
/// </summary>
public abstract class AbstractSprayingGun : MonoBehaviour
{
    /// <summary>
    /// Распыляемый из ружья объект.
    /// </summary>
    [SerializeField] protected GameObject _prefabFiredObject;
    /// <summary>
    /// Аниматор распыляемого объекта.
    /// </summary>
    protected Animator _animatorPrefabFireObject;
    /// <summary>
    /// Наносимый урон.
    /// </summary>
    public float _damage;
    /// <summary>
    /// Суммарный объём топлива.
    /// </summary>
    public int _volumeFuel;
    /// <summary>
    /// Вместимость топлива в "обойме".
    /// </summary>
    public int _capacityFuel;
    /// <summary>
    /// Текущий объём топлива в "обойме".
    /// </summary>
    public int _currentVolumeFuel;
    /// <summary>
    /// Время перезарядки.
    /// </summary>
    public float _timeRecharging;
    /// <summary>
    /// Флаг, указывающий, идёт ли перезарядка.
    /// </summary>
    public bool _isReloading;
    /// <summary>
    /// Флаг, проверяющий, распыляет ли PrefabFiredObject огнемёт.
    /// </summary>
    protected bool _isSpraying = false;
    protected virtual void Awake()
    {
        //Настройка полей

        _animatorPrefabFireObject = _prefabFiredObject.GetComponent<Animator>();

        //Проверка полей.

        if (_animatorPrefabFireObject == null) throw new ArgumentNullException("GunThrower: _animatorPrefabFireObject is null ");
        if (_damage < 0) throw new ArgumentNullException("AbstractGun: _damage < 0");
        if (_volumeFuel < 0) throw new ArgumentOutOfRangeException("AbstractGun: _volumeFuel < 0");
        if (_capacityFuel < 0) throw new ArgumentOutOfRangeException("AbstractGun: _capacityFuel < 0");
        if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("AbstractGun: _timeRecharging < 0");
    }
    /// <summary>
    /// Начинает распыление из ружья.
    /// </summary>
    public abstract void StartFiring();
    /// <summary>
    /// Длительное распыление из ружья.
    /// </summary>
    protected abstract void Fire();
    /// <summary>
    /// Заканчивает распыление из ружья.
    /// </summary>
    public abstract void StopFiring();
    /// <summary>
    /// Перезарядка ружья.
    /// </summary>
    public abstract void Recharge();
    /// <summary>
    /// Проверяет, пусто ли ружьё.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty() => _volumeFuel.Equals(0) && _currentVolumeFuel.Equals(0);
}
