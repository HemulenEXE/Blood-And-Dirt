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
        /// Сила шума оружия при выстреле
        /// </summary>
        [SerializeField] private float noiseIntensity = 5;
        /// <summary>
        /// Событие вызова реакции на шум стрельбы
        /// </summary>
        public static event Action<Transform, float> makeNoiseShooting;
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
        /// <summary>
        /// Компонент, управляющий аудио.
        /// </summary>
        private AudioSource _audioControl;

        //Автосвойства.

        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// Возврашает флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Возврашает флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting { get; set; } = false;

        public bool IsHeld { get; set; } = true;
        /// <summary>
        /// Наносимый урон.
        /// </summary>
        [field: SerializeField] public float Damage { get; private set; } = 6.5f;
        /// <summary>
        /// Задержка между выстрелами.
        /// </summary>
        [field: SerializeField] public float ShotDelay { get; private set; } = 0.1f;
        /// <summary>
        /// Возвращает суммарное число снарядов.
        /// </summary>
        [field: SerializeField] public int AmmoTotal { get; private set; } = 100;
        /// <summary>
        /// Возвращает вместимость очереди.
        /// </summary>
        [field: SerializeField] public int AmmoCapacity { get; private set; } = 30;
        /// <summary>
        /// Возвращает текущее число снарядов в очереди.
        /// </summary>
        [field: SerializeField] public int AmmoTotalCurrent { get; private set; } = 0;
        /// <summary>
        /// Возвращает время перезарядки.
        /// </summary>
        [field: SerializeField] public float RechargingTime { get; private set; } = 1f;
        /// <summary>
        /// Возвращает скорость вылета пули.
        /// </summary>
        [field: SerializeField] public float SpeedProjectile { get; private set; } = 50f;
        /// <summary>
        /// Свойство для шума.
        /// </summary>
        public float NoiseIntensity { get; set; }

        //Свойства.

        //Встроенные методы.

        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected void Awake()
        {
            _audioControl = this.GetComponent<AudioSource>();

            if (Damage < 0) throw new ArgumentOutOfRangeException("MachineGun: Damage < 0");
            if (ShotDelay < 0) throw new ArgumentOutOfRangeException("MachineGun: ShotDelay < 0");
            if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("MachineGun: AmmoTotal < 0");
            if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("MachineGun: AmmoCapacity < 0");
            if (RechargingTime < 0) throw new ArgumentOutOfRangeException("MachineGun: RechargingTime < 0");
            if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("MachineGun: AmmoCapacity < AmmoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("MachineGun: _prefabPellet is null");
            if (_audioControl == null) throw new ArgumentNullException("MachineGun: _audioControl is null");
            if (_audioFire == null) throw new ArgumentNullException("MachineGun: _audioFire is null");
            if (_audioRecharge == null) throw new ArgumentNullException("MachineGun: _audioRecharge is null");
        }

        //Вспомогательные методы.

        /// <summary>
        /// Выстрел из автомата.<br/>
        /// </summary>
        /// <remarks>Порождает на сцене снаряд, вылетающий из автомата.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot(int layerMask = 0, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging)
            {
                if (AmmoTotalCurrent > 0)
                {
                    _audioControl.PlayOneShot(_audioFire);
                    IsShooting = true;

                    var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                    GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); //Вылет снаряда.
                    AmmoTotalCurrent--;
                    currentBullet.layer = layerMask;

                    var projectileData = currentBullet.GetComponent<IBullet>();
                    if (projectileData != null)
                    {
                        projectileData.Damage = Damage;
                        projectileData.GunType = Type;
                        projectileData.Speed = SpeedProjectile;
                    }

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
        /// Перезарядка автомата.
        /// </summary>
        public void Recharge()
        {
            if (AmmoTotal > 0 && !IsShooting && !IsRecharging)
            {
                IsRecharging = true;
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
            yield return new WaitForSeconds(RechargingTime);

            if (!IsHeld)
            {
                IsRecharging = false;
                yield break;
            }

            int count_need_patrons = AmmoCapacity - AmmoTotalCurrent;
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
            IsRecharging = false;
        }
    }
}