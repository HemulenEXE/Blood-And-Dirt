﻿using System;
using System.Collections;
using UnityEngine;

namespace Gun
{
    /// <summary>
    /// Класс, реализующий "дробовик".
    /// </summary>
    public class ShotGun : MonoBehaviour, IGun
    {
        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// Наносимый урон.
        /// </summary>
        private float _damage = 5f;
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        public float Damage { get => _damage; }
        /// <summary>
        /// Задержка между выстрелами.
        /// </summary>
        [SerializeField] protected float _delayShot = 0.5f;
        /// <summary>
        /// Время до следующего выстрела.
        /// </summary>
        protected float _nextTimeShot = 0f;
        /// <summary>
        /// Суммарное число снарядов.
        /// </summary>
        [SerializeField] protected int _ammoTotal = 100;
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
        [SerializeField] protected int _ammoCapacity = 5;
        /// <summary>
        /// Возвращает вместимость очереди.
        /// </summary>
        public int AmmoCapacity { get => _ammoCapacity; }
        /// <summary>
        /// Текущее число снарядов в очереди.
        /// </summary>
        [SerializeField] protected int _ammoTotalCurrent = 0;
        /// <summary>
        /// Возвращает и изменяет текущее число снарядов в очереди.
        /// </summary>
        public int AmmoTotalCurrent
        {
            get => _ammoTotalCurrent;
            set
            {
                if (value > AmmoCapacity) throw new ArgumentOutOfRangeException("ShotGun: value > AmmoCapacity");
                if (value <= 0) _ammoTotalCurrent = 0;
                _ammoTotalCurrent = value;
            }
        }
        /// <summary>
        /// Время перезарядки.
        /// </summary>
        [SerializeField] protected float _timeRecharging = 2;
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting { get; set; } = false;
        /// <summary>
        /// Префаб дробинки, вылетающий из дробовика.
        /// </summary>
        [SerializeField] protected GameObject _prefabProjectile;
        /// <summary>
        /// Количество вылетающих дробинок при одном выстреле.
        /// </summary>
        [SerializeField] protected int _countPerShotProjectile = 6;
        /// <summary>
        /// Начальная скорость вылета дробинки.
        /// </summary>
        [SerializeField] protected float _speedProjectile = 50f;
        /// <summary>
        /// Угол распространения дробинок при выстреле.<br/>
        /// Максимальный угол отклонения между двумя векторами, вдоль которых летят дробинки.
        /// </summary>
        [SerializeField] protected float _spreadAngle = 15f;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected void Awake()
        {
            if (_damage < 0) throw new ArgumentOutOfRangeException("ShotGun: _damage < 0");
            if (_delayShot < 0) throw new ArgumentOutOfRangeException("ShotGun: _delayFire < 0");
            if (_ammoTotal < 0) throw new ArgumentOutOfRangeException("ShotGun: _ammoTotal < 0");
            if (_ammoCapacity < 0) throw new ArgumentOutOfRangeException("ShotGun: _capacityAmmo < 0");
            if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("ShotGun: _timeRecharging < 0");
            if (_ammoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("ShotGun: _ammoCapacity < _ammoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("ShotGun: _prefabPellet is null");
            if (_countPerShotProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _countFlyingPellets < 0");
            if (_speedProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _speedShot < 0");
        }
        /// <summary>
        /// Выстрел из дробовика.<br/>
        /// </summary>
        /// <remarks>Порожает на сцене снаряд, вылетающий из дробовика.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot()
        {
            if (!IsShooting && !IsRecharging && Time.time > _nextTimeShot)
            {
                if (AmmoTotalCurrent > 0)
                {
                    IsShooting = true;
                    _nextTimeShot = Time.time + _delayShot;

                    for (int i = 1; i <= _countPerShotProjectile; i++) //Механика вылета дробинок.
                    {
                        float interim_spread_angle = UnityEngine.Random.Range(-_spreadAngle, _spreadAngle); //Определение угла распространения текущего снаряда.
                        Vector3 direction = this.transform.forward * Mathf.Cos(interim_spread_angle); //Определение направления движения снаряда.
                        GameObject currentPellet = Instantiate(_prefabProjectile, this.transform.GetChild(0).position, this.transform.GetChild(0).rotation); //Вылет снаряда.
                        currentPellet.transform.Rotate(0, 0, interim_spread_angle); //Поворот снаряда.

                        var interim_projectile_component = currentPellet.AddComponent<ProjectileData>();
                        interim_projectile_component.Damage = this._damage;
                        interim_projectile_component.GunType = Type;

                        Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                        if (rg == null) throw new ArgumentNullException("ShotGun: _prefabProjectile hasn't got Rigidbody2D");
                        rg.velocity = currentPellet.transform.right * _speedProjectile;
                    }

                    AmmoTotalCurrent--;
                    IsShooting = false;
                }
                else Recharge();
            }
        }
        /// <summary>
        /// Остановка стрельбы из дробовика.<br/>
        /// Не содержит реализации.
        /// </summary>
        public void StopShoot() { }
        /// <summary>
        /// Перезарядка дробовика.
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
        /// Корутина для перезарядки дробовика.
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
        /// Проверяет, пуст ли дробовик.
        /// </summary>
        public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
    }

}