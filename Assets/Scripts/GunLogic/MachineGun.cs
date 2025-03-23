using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GunLogic
{
    /// <summary>
    /// Класс, реализующий "автомат".
    /// </summary>
    public class MachineGun : MonoBehaviour, IGun
    {
        /// <summary>
        /// Префаб пули, вылетающей из автомата.
        /// </summary>
        [SerializeField] protected GameObject _prefabProjectile;
        /// <summary>
        /// Наносимый урон.
        /// </summary>
        [SerializeField] protected float _damage = 6.5f;
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
        /// Сила шума оружия при выстреле
        /// </summary>
        [SerializeField] private float noiseIntensity = 5;
        /// <summary>
        /// Свойство для шума
        /// </summary>
        public float NoiseIntensity { get; set; }
        /// <summary>
        /// Событие вызова реакции на шум стрельбы
        /// </summary>
        public static event Action<Transform, float> makeNoiseShooting;
        /// <summary>
        /// Время перезарядки.
        /// </summary>
        [SerializeField] protected float _timeRecharging = 1f;
        /// <summary>
        /// Начальная скорость вылета пули.
        /// </summary>
        [SerializeField] protected float _speedProjectile = 50f;
        /// <summary>
        /// Компонент, управляющий вызовами звуков.
        /// </summary>
        protected AudioSource _audio;
        /// <summary>
        /// Звук выстрела из автомата.
        /// </summary>
        [SerializeField] protected AudioClip _audioFire;
        /// <summary>
        /// Звук перезарядки автомата.
        /// </summary>
        [SerializeField] protected AudioClip _audioRecharge;
        /// <summary>
        /// Дальность стрельбы оружия (для ботов)
        /// </summary>
        [SerializeField] protected float attackRange;
        /// <summary>
        /// Свойство дальности атаки (для ботов)
        /// </summary>
        public float AttackRange { get; set; }
        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// Возвращает и изменяет текущее число патронов в очереди.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int AmmoTotalCurrent
        {
            get
            {
                return _ammoTotalCurrent;
            }
            protected set
            {
                if (value > AmmoCapacity) throw new ArgumentOutOfRangeException("MachineGun: value > AmmoCapacity");
                if (value <= 0) _ammoTotalCurrent = 0;
                else _ammoTotalCurrent = value;
            }
        }
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        public float Damage
        {
            get
            {
                return _damage;
            }
        }
        /// <summary>
        /// Возврашает флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Возврашает флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting { get; set; } = false;
        /// <summary>
        /// Возвращает суммарное число патронов.
        /// </summary>
        public int AmmoTotal
        {
            get
            {
                return _ammoTotal;
            }
            protected set
            {
                if (value <= 0) _ammoTotal = 0;
                else _ammoTotal = value;
            }
        }
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected void Awake()
        {
            _audio = this.GetComponent<AudioSource>();

            if (Damage < 0) throw new ArgumentOutOfRangeException("MachineGun: Damage < 0");
            if (_delayShot < 0) throw new ArgumentOutOfRangeException("MachineGun: _delayFire < 0");
            if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("MachineGun: AmmoTotal < 0");
            if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("MachineGun: AmmoCapacity < 0");
            if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("MachineGun: _timeRecharging < 0");
            if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("MachineGun: AmmoCapacity < AmmoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("MachineGun: _prefabPellet is null");
            if (_audio == null) throw new ArgumentNullException("MachineGun: _audio is null");
            if (_audioFire == null) throw new ArgumentNullException("MachineGun: _audioFire is null");
            if (_audioRecharge == null) throw new ArgumentNullException("MachineGun: _audioRecharge is null");
        }
        /// <summary>
        /// Выстрел из автомата.<br/>
        /// </summary>
        /// <remarks>Порождает на сцене снаряд, вылетающий из автомата.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot(Side sideShooter, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging && Time.time > _nextTimeShot)
            {
                if (AmmoTotalCurrent > 0)
                {
                    IsShooting = true;
                    _nextTimeShot = Time.time + _delayShot;
                    _audio.PlayOneShot(_audioFire, 0.5f);

                    GameObject currentPellet = Instantiate(_prefabProjectile, this.transform.GetChild(0).position, this.transform.GetChild(0).rotation); //Вылет снаряда.

                    var interim_projectile_component = currentPellet.GetComponent<ProjectileData>();
                    if (interim_projectile_component != null)
                    {
                        interim_projectile_component.sideBullet = sideShooter.CreateSideBullet();
                        interim_projectile_component.Damage = this._damage;
                        interim_projectile_component.GunType = Type;
                    }

                    Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                    if (rg == null) throw new ArgumentNullException("MachineGun: _prefabProjectile hasn't got Rigidbody2D");
                    //rg.isKinematic = true; // Кинематическое движение

                    currentPellet.layer = LayerMask.NameToLayer(sideShooter.GetOwnLayer());

                    // Запуск скрипта для управления движением пули
                    var bulletController = currentPellet.AddComponent<BulletMovement>();
                    bulletController.SetSpeed(_speedProjectile);

                    AmmoTotalCurrent--;
                    IsShooting = false;
                    //if (IsPlayerShoot)
                    //{
                        makeNoiseShooting?.Invoke(transform, noiseIntensity);
                    //}
                }
                else Recharge();
            }
        }
        /// <summary>
        /// Остановка стрельбы из автомата.<br/>
        /// Не содержит реализации.
        /// </summary>
        public void StopShoot() { }
        /// <summary>
        /// Перезарядка автомата.
        /// </summary>
        public void Recharge()
        {
            if (AmmoTotal > 0 && !IsShooting && !IsRecharging)
            {
                IsRecharging = true; //Начинаем перезарядку.
                StartCoroutine(RechargeCoroutine());
            }
        }
        /// <summary>
        /// Проверяет, пуст ли автомата.
        /// </summary>
        public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
        /// <summary>
        /// Корутина для перезарядки автомата.<br/>
        /// Отвечает за распределение патронов и запуск аудио.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RechargeCoroutine()
        {
            yield return new WaitForSeconds(_timeRecharging);
            _audio.PlayOneShot(_audioRecharge);
            int count_need_patrons = AmmoCapacity - AmmoTotalCurrent; //Количество нехватаемых патронов.
            if (AmmoTotal > count_need_patrons)
            {
                AmmoTotalCurrent += count_need_patrons;
                AmmoTotal -= count_need_patrons;
            }
            else
            {
                AmmoTotalCurrent += AmmoTotal;
                AmmoTotal = 0;
            }
            IsRecharging = false; //Перезарядка окончена.
        }
        /// <summary>
        /// Проверяет, эффективное ли расстояние стрельбы до цели
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool IsInRange(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= attackRange;
        }
    }
}