using System;
using System.Collections;
using UnityEngine;

namespace GunLogic
{
    /// <summary>
    /// Класс, реализующий "автомат".
    /// </summary>
    public class MachineGun : MonoBehaviour, IGun
    {
        //Поля.

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
        /// Текущее число патронов в очереди.
        /// </summary>
        [SerializeField] protected int _ammoTotalCurrent = 0;
        /// <summary>
        /// Сила шума оружия при выстреле
        /// </summary>
        [SerializeField] private float noiseIntensity = 5;
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

        //Свойства.

        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// Возвращает текущее число патронов в очереди.
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
        /// Возвращает вместимость очереди.
        /// </summary>
        public int AmmoCapacity
        {
            get
            {
                return _ammoCapacity;
            }
        }
        /// <summary>
        /// Свойство для шума.
        /// </summary>
        public float NoiseIntensity { get; set; }
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

        //Методы.

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
        public void Shoot(int layerMask = 0, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging && Time.time > _nextTimeShot)
            {
                if (AmmoTotalCurrent > 0)
                {
                    IsShooting = true;
                    _nextTimeShot = Time.time + _delayShot;

                    var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                    GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); //Вылет снаряда.
                    AmmoTotalCurrent--;
                    currentBullet.layer = layerMask;
                    _audio.PlayOneShot(_audioFire);

                    var projectileData = currentBullet.GetComponent<ProjectileData>();
                    if (projectileData != null)
                    {
                        projectileData.Damage = Damage;
                        projectileData.GunType = Type;
                    }

                    //Запуск скрипта для управления движением пули
                    var bulletController = currentBullet.AddComponent<BulletMovement>();
                    bulletController.SetSpeed(_speedProjectile);

                    IsShooting = false;
                    if (IsPlayerShoot)
                    {
                        makeNoiseShooting?.Invoke(transform, noiseIntensity);
                    }
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
        public bool IsEmpty()
        {
            return AmmoTotal == 0 && AmmoTotalCurrent == 0;
        }
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
    }
}