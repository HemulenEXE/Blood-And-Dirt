using System;
using System.Collections;
using UnityEngine;

namespace GunLogic
{
    /// <summary>
    /// Класс, реализующий "пистолет".
    /// </summary>
    public class Pistol : MonoBehaviour, IGun
    {
        //Поля.

        /// <summary>
        /// Префаб пули, вылетающий из пистолета.
        /// </summary>
        [SerializeField] protected GameObject _prefabProjectile;
        /// <summary>
        /// Сила шума оружия при выстреле
        /// </summary>
        [SerializeField] private float noiseIntensity = 5;
        /// <summary>
        /// Событие вызова реакции на шум стрельбы
        /// </summary>
        public static event Action<Transform, float> makeNoiseShooting;
        /// <summary>
        /// Компонент, управляющий аудио.
        /// </summary>
        [SerializeField] private AudioSource _audioControl;
        /// <summary>
        /// Возвращает звук выстрела.
        /// </summary>
        [SerializeField] private AudioClip _audioFire;
        /// <summary>
        /// Возвращает звук перезарядки.
        /// </summary>
        [SerializeField] private AudioClip _audioRecharge;
        /// <summary>
        /// Возвращает звук взвода.
        /// </summary>
        [SerializeField] private AudioClip _audioPlatoon;

        //Автосвойства.

        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Light;

        public bool IsHeld { get; set; } = true;

        /// <summary>
        /// Возвращает наносимый урон.
        /// </summary>
        [field: SerializeField] public float Damage { get; private set; } = 2f;
        /// <summary>
        /// Возвращает задержку между выстрелами.
        /// </summary>
        [field: SerializeField] public float ShotDelay { get; private set; } = 0.2f;
        /// <summary>
        /// Возвращает скорость вылета пули.
        /// </summary>
        public float SpeedProjectile { get; private set; } = 50f;
        /// <summary>
        /// Возвращает суммарное число снарядов.
        /// </summary>
        [field: SerializeField] public int AmmoTotal { get; private set; } = 100;
        /// <summary>
        /// Возвращает вместимость очереди.
        /// </summary>
        [field: SerializeField] public int AmmoCapacity { get; private set; } = 10;
        /// <summary>
        /// Возвращает текущее число снарядов в очереди.
        /// </summary>
        [field: SerializeField] public int AmmoTotalCurrent { get; private set; } = 0;
        /// <summary>
        /// Возвращает время перезарядки.
        /// </summary>
        [field: SerializeField] public float RechargingTime { get; private set; } = 1f;
        /// <summary>
        /// Возврашает флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Возврашает флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting { get; set; } = false;
        /// <summary>
        /// Свойство для шума
        /// </summary>
        public float NoiseIntensity { get; set; }

        //Свойства.

        //Встроенные методы.

        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// /// <summary>
        /// Дальность стрельбы оружия (для ботов)
        /// </summary>
        [SerializeField] protected float attackRange;
        /// <summary>
        /// Свойство дальности атаки (для ботов)
        /// </summary>
        public float AttackRange { get; set; }
        protected void Awake()
        {
            _audioControl = this.GetComponent<AudioSource>();

            if (Damage < 0) throw new ArgumentOutOfRangeException("Pistol: Damage < 0");
            if (ShotDelay < 0) throw new ArgumentOutOfRangeException("Pistol: ShotDelay < 0");
            if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("Pistol: AmmoTotal < 0");
            if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("Pistol: AmmoCapacity < 0");
            if (RechargingTime < 0) throw new ArgumentOutOfRangeException("Pistol: RechargingTime < 0");
            if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("Pistol: AmmoCapacity < AmmoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("Pistol: _prefabPellet is null");
            if (_audioControl == null) throw new ArgumentNullException("Pistol: _audioControl is null");
            if (_audioFire == null) throw new ArgumentNullException("Pistol: _audioFire is null");
            if (_audioRecharge == null) throw new ArgumentNullException("Pistol: _audioRecharge is null");
        }

        //Вспомогательные методы.

        /// <summary>
        /// Выстрел из пистолета.<br/>
        /// </summary>
        /// <remarks>Порождает на сцене снаряд, вылетающий из пистолета.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot(Side sideShooter, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging)
            {
                if (AmmoTotalCurrent > 0)
                {
                    _audioControl.PlayOneShot(_audioFire);
                    IsShooting = true;
                    _nextTimeShot = Time.time + _delayShot;
                    _audio.PlayOneShot(_audioFire,0.5f);

                    var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                    GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); //Вылет снаряда.
                    currentBullet.layer = layerMask;
                    AmmoTotalCurrent--;

                    var projectileData = currentBullet.GetComponent<ProjectileData>();
                    if (projectileData != null)
                    {
                        interim_projectile_component.sideBullet = sideShooter.CreateSideBullet();
                        interim_projectile_component.Damage = this._damage;
                        interim_projectile_component.GunType = Type;
                    }

                    var bulletController = currentBullet.AddComponent<BulletMovement>();
                    bulletController.SetSpeed(SpeedProjectile);

                    currentPellet.layer = LayerMask.NameToLayer(sideShooter.GetOwnLayer());

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
        /// Перезарядка пистолета.
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
        /// Проверяет, пуст ли пистолет.
        /// </summary>
        public bool IsEmpty()
        {
            return AmmoTotal == 0 && AmmoTotalCurrent == 0;
        }
        /// <summary>
        /// Корутина для перезарядки пистолета.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RechargeCoroutine()
        {
            yield return new WaitForSeconds(RechargingTime);

            if (!IsHeld)
            {
                IsRecharging = false;
                yield break;
            }

            int count_need_patrons = AmmoCapacity - AmmoTotalCurrent; //Количество нехватаемых патронов.
            _audioControl.PlayOneShot(_audioRecharge);
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
