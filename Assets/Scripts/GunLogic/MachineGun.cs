using System;
using System.Collections;
using UnityEngine;

namespace Gun
{
    /// <summary>
    /// Класс, реализующий "автомат.
    /// </summary>
    public class MachineGun : MonoBehaviour, IGun
    {
        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// Префаб пули, вылетающий из автомата.
        /// </summary>
        [SerializeField] protected GameObject _prefabProjectile;
        /// <summary>
        /// Наносимый урон.
        /// </summary>
        [SerializeField] protected float _damage = 6.5f;
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        public float Damage { get => _damage; }
        /// <summary>
        /// Задержка между выстрелами.
        /// </summary>
        [SerializeField] protected float _delayShot = 0.05f;
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
        [SerializeField] protected int _ammoCapacity = 30;
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
                if (value > AmmoCapacity) throw new ArgumentOutOfRangeException("MachineGun: value > AmmoCapacity");
                if (value <= 0) _ammoTotalCurrent = 0;
                else _ammoTotalCurrent = value;
            }
        }
        /// <summary>
        /// Время перезарядки.
        /// </summary>
        [SerializeField] protected float _timeRecharging = 1f;
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
        [SerializeField] protected float _speedProjectile = 50f;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected void Awake()
        {
            if (Damage < 0) throw new ArgumentOutOfRangeException("MachineGun: _damage < 0");
            if (_delayShot < 0) throw new ArgumentOutOfRangeException("MachineGun: _delayFire < 0");
            if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("MachineGun: _ammoTotal < 0");
            if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("MachineGun: _capacityAmmo < 0");
            if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("MachineGun: _timeRecharging < 0");
            if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("MachineGun: _ammoCapacity < _ammoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("MachineGun: _prefabPellet is null");
        }
        /// <summary>
        /// Выстрел из пистолета.<br/>
        /// </summary>
        /// <remarks>Порождает на сцене снаряд, вылетающий из пистолета.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot()
        {
            if (!IsShooting && !IsRecharging && Time.time > _nextTimeShot)
            {
                if (AmmoTotalCurrent > 0)
                {
                    IsShooting = true;
                    _nextTimeShot = Time.time + _delayShot;

                    GameObject currentPellet = Instantiate(_prefabProjectile, this.transform.GetChild(0).position, this.transform.GetChild(0).rotation); //Вылет снаряда.

                    var interim_projectile_component = currentPellet.AddComponent<ProjectileData>();
                    interim_projectile_component.Damage = this._damage;
                    interim_projectile_component.GunType = Type;

                    Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                    if (rg == null) throw new ArgumentNullException("MachineGun: _prefabProjectile hasn't got Rigidbody2D");
                    rg.velocity = currentPellet.transform.right * _speedProjectile;

                    AmmoTotalCurrent--;
                    IsShooting = false;
                }
                else Recharge();
            }
        }
        /// <summary>
        /// Остановка стрельбы из пистолета.<br/>
        /// Не содержит реализации.
        /// </summary>
        public void StopShoot() { }
        /// <summary>
        /// Перезарядка пистолета.
        /// </summary>
        public void Recharge()
        {
            if (_ammoTotal > 0 && !IsRecharging)
            {
                IsRecharging = true;
                IsShooting = false;
                StartCoroutine(RechargeCoroutine()); //На перезарядку отводится некоторое время.
            }
        }
        /// <summary>
        /// Корутина для перезарядки пистолета.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RechargeCoroutine()
        {
            yield return new WaitForSeconds(_timeRecharging);
            AmmoTotal -= AmmoCapacity - AmmoTotalCurrent;
            AmmoTotalCurrent = AmmoCapacity;
            IsRecharging = false;
        }
        /// <summary>
        /// Проверяет, пуст ли пистолет.
        /// </summary>
        public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
    }
}