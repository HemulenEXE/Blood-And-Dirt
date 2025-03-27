using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GunLogic
{
    public class MachineGun : MonoBehaviour, IGun
    {

        /// <summary>
        /// Префаб пули.
        /// </summary>
        [SerializeField] protected GameObject _prefabProjectile;
        /// <summary>
        /// Сила звука
        /// </summary>
        [SerializeField] private float noiseIntensity = 5;
        /// <summary>
        /// Событие отвечающие за передачу источника и силы звука медиатору событий
        /// </summary>
        public static event Action<Transform, float> makeNoiseShooting;
        /// <summary>
        /// звук стрельбы.
        /// </summary>
        [SerializeField] private AudioClip _audioFire;
        /// <summary>
        /// компонент отвечающий за звук перезарядки.
        /// </summary>
        [SerializeField] private AudioClip _audioRecharge;
        /// <summary>
        /// Компонент отвечающий за звук.
        /// </summary>
        [SerializeField] private AudioClip _audioPlatoon;
        /// <summary>
        /// компонент Источник звуков.
        /// </summary>
        private AudioSource _audioControl;
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
        /// Состояние перезарядки.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Состояние стрельбы.
        /// </summary>
        public bool IsShooting { get; set; } = false;

        public bool IsHeld { get; set; } = true;
        /// <summary>
        /// Урон.
        /// </summary>
        [field: SerializeField] public float Damage { get; private set; } = 6.5f;
        /// <summary>
        /// Задержка стрельбы.
        /// </summary>
        [field: SerializeField] public float ShotDelay { get; private set; } = 0.1f;
        /// <summary>
        /// Общее количество патрон.
        /// </summary>
        [field: SerializeField] public int AmmoTotal { get; private set; } = 100;
        /// <summary>
        /// Количество поатронов в рожке .
        /// </summary>
        [field: SerializeField] public int AmmoCapacity { get; private set; } = 30;
        /// <summary>
        /// Текущее количество патронов.
        /// </summary>
        [field: SerializeField] public int AmmoTotalCurrent { get; private set; } = 0;
        /// <summary>
        /// Время перезарядки.
        /// </summary>
        [field: SerializeField] public float RechargingTime { get; private set; } = 1f;
        /// <summary>
        /// Скорость пуль.
        /// </summary>
        [field: SerializeField] public float SpeedProjectile { get; private set; } = 50f;
        /// <summary>
        /// Громкость звука.
        /// </summary>
        public float NoiseIntensity { get; set; }

        

        /// <summary>
        /// Проверка на корректность перемен
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

        

        /// <summary>
        /// Выполнение механизма стрельбы с создание и корректированием пули.<br/>
        /// </summary>
        /// <remarks>.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot(Side sideShooter, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging)
            {
                if (AmmoTotalCurrent > 0)
                {
                    _audioControl.PlayOneShot(_audioFire, 0.5f);
                    IsShooting = true;

                    var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                    GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); 
                    

                    var projectileData = currentBullet.GetComponent<IBullet>();
                    if (projectileData != null)
                    {
                        projectileData.sideBullet = sideShooter.CreateSideBullet();
                        projectileData.Damage = this.Damage;
                        projectileData.GunType = Type;
                        projectileData.Speed = SpeedProjectile;
                    }


                    AmmoTotalCurrent--;
                    currentBullet.layer = LayerMask.NameToLayer(sideShooter.GetOwnLayer());
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
        /// Перезарядка
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
        /// Проверка на пустоту магазина
        /// </summary>
        public bool IsEmpty()
        {
            return AmmoTotal == 0 && AmmoTotalCurrent == 0;
        }
        /// <summary>
        /// Выполняют задежржку скрипта стрельбы во время перезарядки
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