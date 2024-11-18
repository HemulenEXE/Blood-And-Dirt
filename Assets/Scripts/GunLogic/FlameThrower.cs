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
    /// Звук взвода оружия.
    /// </summary>
    [SerializeField] protected AudioClip _audioPlatoon;
    /// <summary>
    /// Наносимый урон.
    /// </summary>
    [SerializeField] protected float _damage = 10f;
    /// <summary>
    /// Возвращает величину наносимого урона.
    /// </summary>
    public float Damage { get => _damage; }
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
    [SerializeField] protected float _timeRecharging = 8f;
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
        _audio = this.GetComponent<AudioSource>();
        if (_audio.Equals(null)) throw new ArgumentNullException("FlameThrower: _audio is null");
        if (Damage < 0) throw new ArgumentOutOfRangeException("FlameThrower: _damage < 0");
        if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("FlameThrower: _ammoTotal < 0");
        if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("FlameThrower: _capacityAmmo < 0");
        if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("FlameThrower: _timeRecharging < 0");
        if (AmmoCapacity < _ammoTotalCurrent) throw new ArgumentOutOfRangeException("FlameThrower: _ammoCapacity < _ammoTotalCurrent");
        if (_prefabProjectile.Equals(null)) throw new ArgumentNullException("FlameThrower: _prefabPellet is null");
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
                    _audio.loop = true;
                    _audio.PlayOneShot(_audioFire);
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
            _audio.loop = false;
            _audio.Stop();
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
            _audio.PlayOneShot(_audioRecharge);
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
        _audio.Stop();
        _audio.PlayOneShot(_audioPlatoon);
        IsRecharging = false;
    }
    /// <summary>
    /// Проверяет, пусто ли ружьё.
    /// </summary>
    public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
}
