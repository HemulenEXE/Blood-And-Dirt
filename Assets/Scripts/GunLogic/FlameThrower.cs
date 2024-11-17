using System.Collections;
using UnityEngine;

using System;

/// <summary>
/// Класс, реализующий "огнемёт".
/// </summary>
public class FlameThrower : MonoBehaviour, IGun
{
    /// <summary>
    /// Префаб пламени.
    /// </summary>
    [SerializeField] protected ParticleSystem _prefabProjectile;
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
    [SerializeField] protected int _ammoTotal = 10_000;
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
    [SerializeField] protected int _ammoCapacity = 2_000;
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
    public bool IsShooting
    {
        get => _prefabProjectile.isEmitting; set
        {
            if (value.Equals(true)) _prefabProjectile.Play();
            else _prefabProjectile.Stop();
        }
    }
    /// <exception cref="ArgumentNullException"></exception>
    protected void Awake()
    {
        //Проверка полей, настраиваемых в редакторе Unity.
        if (_prefabProjectile == null) throw new ArgumentNullException("FlameThrower: _prefabProjectile is null");
    }
    /// <summary>
    /// Распыление из огнемёта.
    /// </summary>
    public void Shoot()
    {
        if (!IsRecharging && Time.time >= _nextTimeShot)
        {
            if (AmmoTotalCurrent > 0)
            {
                if (!IsShooting)
                {
                    IsShooting = true; //_prefabProjectile.Play();
                }
                AmmoTotalCurrent--;
            }
            else Recharge();
        }
    }
    /// <summary>
    /// Остановка распыления из огнемёта.
    /// </summary>
    public void StopShoot()
    {
        if (IsShooting)
        {
            IsShooting = false; //_prefabProjectile.Stop();
        }
    }
    /// <summary>
    /// Перезарядка огнемёта.<br/>
    /// Вызывает корутину для перезарядки огнемёта.
    /// </summary>
    public void Recharge()
    {
        StopShoot();
        if (AmmoTotal > 0 && !IsRecharging)
        {
            IsRecharging = true;

            StartCoroutine(RechargeCoroutine());
        }
    }
    /// <summary>
    /// Корутина для перезарядки огнемёта.
    /// </summary>
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
