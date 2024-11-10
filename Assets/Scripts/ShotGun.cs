using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс дробовика.
/// </summary>
public class ShotGun : AbstractShootingGun
{
    /// <summary>
    /// Количестве вылетающих дробинок при одном выстреле из этого дробовика.
    /// </summary>
    public int _countFlyingPellets;
    /// <summary>
    /// Угол распространения дробинки при выстреле.
    /// Максимальный угол отклонения между двумя разными дробинками, вылетающими из данного дробовика.
    /// </summary>
    public float _spreadAngle;
    protected override void Awake()
    {
        //Проверка полей

        base.Awake();

        if (_prefabFiredObject == null) throw new ArgumentNullException("ShotGun: _prefabPellet is null");
        if (_countFlyingPellets < 0) throw new ArgumentOutOfRangeException("ShotGun: _countFlyingPellets < 0");
        if (_speedShot < 0) throw new ArgumentOutOfRangeException("ShotGun: _speedShot < 0");
    }
    /// <summary>
    /// Выстрел из данного дробовика.
    /// Реализует механику порождения PrefabPellet в позиции position и под углом angle
    /// </summary>
    public override void Shoot()
    {
        if (_currAmmoTotal > 0)
        {
            for (int i = 1; i <= _countFlyingPellets; i++) //Механика вылета дробинок
            {
                float interim_spread_angle = UnityEngine.Random.Range(-_spreadAngle, _spreadAngle); //Угол распространения произвольной дробинки
                Vector3 direction = this.transform.forward * Mathf.Cos(interim_spread_angle); //Определение направления
                GameObject currentPellet = Instantiate(_prefabFiredObject, this.transform.position, this.transform.rotation); //Вылет дробинки
                currentPellet.transform.Rotate(0, 0, interim_spread_angle);
                Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                if (rg == null) throw new ArgumentNullException();
                rg.velocity = currentPellet.transform.right * _speedShot;
            }
            _currAmmoTotal--;
            StartCoroutine(DelayFire());
        }
        else Recharge();
    }
    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(_delayFire);
    }
    /// <summary>
    /// Перезарядка этого дробовика.
    /// </summary>
    public override void Recharge()
    {
        if (_ammoTotal > 0 && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine(RechargeCoroutine());
        }
    }
    private IEnumerator RechargeCoroutine()
    {
        while (_currAmmoTotal != _capacityAmmo)
        {
            _ammoTotal--;
            _currAmmoTotal++;
            yield return new WaitForSeconds(_timeRecharging / _capacityAmmo);
        }
        _isReloading = false;
    }
}
