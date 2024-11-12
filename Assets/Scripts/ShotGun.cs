using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс, реализующий "дробовик".
/// </summary>
public class ShotGun : AbstractGun
{
    /// <summary>
    /// Префаб снаряда, вылетающего из дробовика.
    /// </summary>
    [SerializeField] protected GameObject _prefabProjectile;
    /// <summary>
    /// Количество вылетающих снарядов при одном выстреле.
    /// </summary>
    [SerializeField] protected int _countProjectile = 3;
    /// <summary>
    /// Начальная скорость вылета снаряда.
    /// </summary>
    [SerializeField] protected float _speedProjectile = 5f;
    /// <summary>
    /// Угол распространения снарядов при выстреле.<br/>
    /// Максимальный угол отклонения между двумя векторами, вдоль которых летят снаряды.
    /// </summary>
    [SerializeField] protected float _spreadAngle = 15f;
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void Awake()
    {
        //Проверка полей
        base.Awake();
        if (_prefabProjectile == null) throw new ArgumentNullException("ShotGun: _prefabPellet is null");
        if (_countProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _countFlyingPellets < 0");
        if (_speedProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _speedShot < 0");
    }
    /// <summary>
    /// Выстрел из дробовика.<br/>
    /// </summary>
    /// <remarks>Порожает на сцене снаряд, вылетающий из дробовика.</remarks>
    /// <exception cref="ArgumentNullException"></exception>
    public override void Shoot()
    {
        if (_ammoTotalCurrent > 0)
        {
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
            _ammoTotalCurrent--;
            StartCoroutine(DelayShotCoroutine()); //Между выстрелами идёт задержка.
            StopShoot(); //Ничего не делает.
        }
        else Recharge();
    }
    /// <summary>
    /// Остановка стрельбы из дробовика.<br/>
    /// Реализации не содержит.
    /// </summary>
    public override void StopShoot() { }
    /// <summary>
    /// Перезарядка дробовика.
    /// </summary>
    public override void Recharge()
    {
        if (_ammoTotal > 0 && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine(RechargeCoroutine()); //На перезарядку отводится некоторое время.
        }
    }
    /// <summary>
    /// Корутина для задержки между выстрелами.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayShotCoroutine()
    {
        yield return new WaitForSeconds(_delayShot);
    }
    /// <summary>
    /// Корутина для перезарядки дробовика.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RechargeCoroutine()
    {
        while (_ammoTotalCurrent != _ammoCapacity)
        {
            _ammoTotal--;
            _ammoTotalCurrent++;
            yield return new WaitForSeconds(_timeRecharging / _ammoCapacity); //Игрок может выстрелить до полной перезарядки ружья.
        }
        _isReloading = false;
    }
}
