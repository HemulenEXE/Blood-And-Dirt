using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace GunLogic
{
    /// <summary>
    /// Класс, реализующий "дробовик".
    /// </summary>
    public class ShotGun : MonoBehaviour, IGun
    {
        //Поля.

        /// <summary>
        /// Наносимый урон.
        /// </summary>
        private float _damage = 5f;
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
        /// Вместимость очереди.
        /// </summary>
        [SerializeField] protected int _ammoCapacity = 5;
        /// <summary>
        /// Текущее число снарядов в очереди.
        /// </summary>
        [SerializeField] protected int _ammoTotalCurrent = 0;
        /// <summary>
        /// Время перезарядки.
        /// </summary>
        [SerializeField] protected float _timeRecharging = 2;
        /// <summary>
        /// Сила шума оружия при выстреле
        /// </summary>
        [SerializeField] private float noiseIntensity = 5;
        /// <summary>
        /// Событие вызова реакции на шум стрельбы
        /// </summary>
        public static event Action<Transform, float> makeNoiseShooting;
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
        /// Компонент, управляющий вызовами звуков.
        /// </summary>
        protected AudioSource _audio;
        /// <summary>
        /// Звук выстрела из дробовика.
        /// </summary>
        [SerializeField] protected AudioClip _audioFire;
        /// <summary>
        /// Звук перезарядки дробовика.
        /// </summary>
        [SerializeField] protected AudioClip _audioRecharge;
        /// <summary>
        /// Звук взвода дробовика.
        /// </summary>
        [SerializeField] protected AudioClip _audioPlatoon;

        //Свойства.

        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
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
        /// Возвращает и изменяет суммарное число снарядов.
        /// </summary>
        public int AmmoTotal
        {
            get
            {
                return _ammoTotal;
            }
            set
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
        /// Возвращает и изменяет текущее число снарядов в очереди.
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
                if (value > AmmoCapacity) throw new ArgumentOutOfRangeException("ShotGun: value > AmmoCapacity");
                if (value <= 0) _ammoTotalCurrent = 0;
                _ammoTotalCurrent = value;
            }
        }
        /// <summary>
        /// Свойство для шума
        /// </summary>
        public float NoiseIntensity { get; set; }
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли стрельба.
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

            if (Damage < 0) throw new ArgumentOutOfRangeException("ShotGun: Damage < 0");
            if (_delayShot < 0) throw new ArgumentOutOfRangeException("ShotGun: _delayFire < 0");
            if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("ShotGun: AmmoTotal < 0");
            if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("ShotGun: AmmoCapacity < 0");
            if (_timeRecharging < 0) throw new ArgumentOutOfRangeException("ShotGun: _timeRecharging < 0");
            if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("ShotGun: AmmoCapacity < AmmoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("ShotGun: _prefabPellet is null");
            if (_countPerShotProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _countFlyingPellets < 0");
            if (_speedProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _speedShot < 0");
            if (_audio == null) throw new ArgumentNullException("ShotGun: _audio is null");
            if (_audioFire == null) throw new ArgumentNullException("ShotGun: _audioFire is null");
            if (_audioRecharge == null) throw new ArgumentNullException("ShotGun: _audioRecharge is null");
            if (_audioPlatoon == null) throw new ArgumentNullException("ShotGun: _audioPlatoon is null");
        }
        /// <summary>
        /// Выстрел из дробовика.<br/>
        /// </summary>
        /// <remarks>Порожает на сцене снаряд, вылетающий из дробовика.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot(int layerMask = 0, bool IsPlayerShoot = false)
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

                        var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                        GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); //Вылет дробинки.
                        _audio.PlayOneShot(_audioFire);
                        currentBullet.layer = layerMask;
                        AmmoTotalCurrent--;
                        currentBullet.transform.Rotate(0, 0, interim_spread_angle); //Поворот снаряда.

                        var projectileData = currentBullet.GetComponent<ProjectileData>();
                        if (projectileData != null)
                        {
                            projectileData.Damage = this._damage;
                            projectileData.GunType = Type;
                        }

                        var bulletController = currentBullet.AddComponent<BulletMovement>();
                        bulletController.SetSpeed(_speedProjectile);
                    }
                    IsShooting = false;
                    if(IsPlayerShoot)
                    {
                        makeNoiseShooting?.Invoke(transform, noiseIntensity);
                    }
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
            if (AmmoTotal > 0 && !IsRecharging && !IsShooting)
            {
                IsRecharging = true; //Начало перезарядки.
                StartCoroutine(RechargeCoroutine());
            }
        }
        /// <summary>
        /// Проверяет, пуст ли дробовик.
        /// </summary>
        public bool IsEmpty()
        {
            return AmmoTotal == 0 && AmmoTotalCurrent == 0;
        }
        /// <summary>
        /// Корутина для перезарядки дробовика.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RechargeCoroutine()
        {
            while (AmmoTotalCurrent < AmmoCapacity)
            {
                _audio.PlayOneShot(_audioRecharge);
                AmmoTotal--;
                AmmoTotalCurrent++;
                yield return new WaitForSeconds(_timeRecharging / AmmoCapacity);
            }
            IsRecharging = false; //Перезарядка окончена.
        }
    }
}