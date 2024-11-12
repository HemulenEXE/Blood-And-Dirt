using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Класс, реализующий "огнемёт".
/// </summary>
public class FlameThrower : AbstractGun
{
    /// <summary>
    /// Префаб пламени.
    /// </summary>
    [SerializeField] private ParticleSystem _prefabProjectile;
    /// <exception cref="ArgumentNullException"></exception>
    protected override void Awake()
    {
        //Проверка и настройка полей.
        base.Awake();
        if (_prefabProjectile == null) throw new ArgumentNullException("FlameThrower: _prefabProjectile is null");
    }
    /// <summary>
    /// Распыление из огнемёта.
    /// </summary>
    public override void Shoot()
    {
        if (_ammoTotalCurrent > 0)
        {
            if (!_prefabProjectile.isPlaying) _prefabProjectile.Play();
            _ammoTotalCurrent--;
        }
        else Recharge();
    }
    /// <summary>
    /// Остановка распыления из огнемёта.
    /// </summary>
    public override void StopShoot()
    {
        if (!_prefabProjectile.isStopped) _prefabProjectile.Stop();
    }
    /// <summary>
    /// Перезарядка огнемёта.<br/>
    /// Вызывает корутину для перезарядки огнемёта.
    /// </summary>
    public override void Recharge()
    {
        StopShoot();
        if (_ammoTotal > 0 && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine(RechargeCoroutine());
        }
    }
    /// <summary>
    /// Корутина для перезарядки огнемёта.
    /// </summary>
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
