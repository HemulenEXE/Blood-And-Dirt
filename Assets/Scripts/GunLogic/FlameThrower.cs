﻿using System;
using System.Collections;
using UnityEngine;

namespace Gun
{
    /// <summary>
    /// Класс, реализующий "огнемёт".
    /// </summary>
    public class FlameThrower : MonoBehaviour, IGun
    {
        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Firebased;
        /// <summary>
        /// Наносимый урон.
        /// </summary>
        [SerializeField] protected float _damage = 5;
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        public float Damage { get => _damage; }
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
        /// Текущий объём топлива в очереди.
        /// </summary>
        [SerializeField] protected int _ammoTotalCurrent = 0;
        /// <summary>
        /// Возвращает и изменяет текущий объём топлива в очереди.
        /// </summary>
        public int AmmoTotalCurrent
        {
            get => _ammoTotalCurrent;
            set
            {
                if (value > AmmoCapacity) throw new ArgumentOutOfRangeException("FlameThrower: value > AmmoCapacity");
                if (value <= 0) _ammoTotalCurrent = 0;
                else _ammoTotalCurrent = value;
            }
        }
        /// <summary>
        /// Время перезарядки.
        /// </summary>
        [SerializeField] protected float _timeRecharging = 5;
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting
        {
            get => _prefabProjectile.isEmitting;
            set
            {
                if (value.Equals(true)) _prefabProjectile.Play();
                else _prefabProjectile.Stop();
            }
        }
        /// <summary>
        /// Префаб пламени, вылетающий из огнемёта.
        /// </summary>
        [SerializeField] protected ParticleSystem _prefabProjectile;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected void Awake()
        {
            if (_damage < 0) throw new ArgumentOutOfRangeException("FlameThrower: _damage < 0");
            if (_ammoTotal < 0) throw new ArgumentOutOfRangeException("FlameThrower: _ammoTotal < 0");
            if (_ammoCapacity < 0) throw new ArgumentOutOfRangeException("FlameThrower: _capacityAmmo < 0");
            if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("FlameThrower: _timeRecharging < 0");
            if (_ammoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("FlameThrower: _ammoCapacity < _ammoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("FlameThrower: _prefabProjectile is null");
        }
        /// <summary>
        /// Распыление из огнемёта.
        /// </summary>
        /// <remarks>Запускает particle пламени.</remarks>
        public void Shoot()
        {
            if (!IsRecharging)
            {
                if (AmmoTotalCurrent > 0)
                {
                    if (!IsShooting)
                    {
                        IsShooting = true; //Вызывается _prefabProjectile.Play();
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
                IsShooting = false; //Вызывается _prefabProjectile.Stop();
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
            yield return new WaitForSeconds(_timeRecharging);
            AmmoTotal -= AmmoCapacity - AmmoTotalCurrent;
            AmmoTotalCurrent = AmmoCapacity;
            IsRecharging = false;
            IsRecharging = false;
        }
        /// <summary>
        /// Проверяет, пуст ли огнемёт.
        /// </summary>
        public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
    }

}