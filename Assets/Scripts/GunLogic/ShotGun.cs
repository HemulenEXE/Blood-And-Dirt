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
        /// Угол распространения дробинок при выстреле.<br/>
        /// Максимальный угол отклонения между двумя векторами, вдоль которых летят дробинки.
        /// </summary>
        [SerializeField] protected float _spreadAngle = 15f;
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
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting { get; set; } = false;
        /// <summary>
        /// Возвращает наносимый урон.
        /// </summary>
        [field: SerializeField] public float Damage { get; private set; } = 5f;
        /// <summary>
        /// Возвращает задержку между выстрелами.
        /// </summary>
        [field: SerializeField] public float ShotDelay { get; private set; } = 0.5f;
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
        [field: SerializeField] public int AmmoCapacity { get; private set; } = 5;
        /// <summary>
        /// Возвращает текущее число снарядов в очереди.
        /// </summary>
        [field: SerializeField] public int AmmoTotalCurrent { get; private set; } = 0;
        /// <summary>
        /// Возвращает время перезарядки.
        /// </summary>
        [field: SerializeField] public float RechargingTime { get; private set; } = 2;
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        /// <summary>
        /// Свойство для шума
        /// </summary>
        public float NoiseIntensity { get; set; }

        //Встроенные методы.

        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected void Awake()
        {
            _audioControl = this.GetComponent<AudioSource>();

            if (Damage < 0) throw new ArgumentOutOfRangeException("ShotGun: Damage < 0");
            if (ShotDelay < 0) throw new ArgumentOutOfRangeException("ShotGun: ShotDelay < 0");
            if (AmmoTotal < 0) throw new ArgumentOutOfRangeException("ShotGun: AmmoTotal < 0");
            if (AmmoCapacity < 0) throw new ArgumentOutOfRangeException("ShotGun: AmmoCapacity < 0");
            if (RechargingTime < 0) throw new ArgumentOutOfRangeException("ShotGun: RechargingTime < 0");
            if (AmmoCapacity < AmmoTotalCurrent) throw new ArgumentOutOfRangeException("ShotGun: AmmoCapacity < AmmoTotalCurrent");
            if (_prefabProjectile == null) throw new ArgumentNullException("ShotGun: _prefabPellet is null");
            if (_countPerShotProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: _countFlyingPellets < 0");
            if (SpeedProjectile < 0) throw new ArgumentOutOfRangeException("ShotGun: SpeedProjectile < 0");
            if (_audioControl == null) throw new ArgumentNullException("ShotGun: _audioControl is null"); 
            if (_audioFire == null) throw new ArgumentNullException("ShotGun: _audioFire is null");
            if (_audioRecharge == null) throw new ArgumentNullException("ShotGun: _audioRecharge is null");
            if (_audioPlatoon == null) throw new ArgumentNullException("ShotGun: _audioPlatoon is null");
        }

        //Вспомогательные методы.

        /// <summary>
        /// Выстрел из дробовика.<br/>
        /// </summary>
        /// <remarks>Порожает на сцене снаряд, вылетающий из дробовика.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot(int layerMask = 0, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging)
            {
                if (AmmoTotalCurrent > 0)
                {
                    _audioControl.PlayOneShot(_audioFire);
                    IsShooting = true;

                    for (int i = 1; i <= _countPerShotProjectile; i++) //Механика вылета дробинок.
                    {
                        float interim_spread_angle = UnityEngine.Random.Range(-_spreadAngle, _spreadAngle); //Определение угла распространения текущего снаряда.
                        Vector3 direction = this.transform.forward * Mathf.Cos(interim_spread_angle); //Определение направления движения снаряда.

                        var spawnerProjectile = this.transform.Find("SpawnerProjectile");
                        GameObject currentBullet = Instantiate(_prefabProjectile, spawnerProjectile.position, spawnerProjectile.rotation); //Вылет дробинки.
                        currentBullet.layer = layerMask;
                        currentBullet.transform.Rotate(0, 0, interim_spread_angle); //Поворот снаряда.

                        var projectileData = currentBullet.GetComponent<ProjectileData>();
                        if (projectileData != null)
                        {
                            projectileData.Damage = Damage;
                            projectileData.GunType = Type;
                        }

                        var bulletController = currentBullet.AddComponent<BulletMovement>();
                        bulletController.SetSpeed(SpeedProjectile);
                    }
                    AmmoTotalCurrent--;
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
                _audioControl.PlayOneShot(_audioRecharge);
                AmmoTotal--;
                AmmoTotalCurrent++;
                yield return new WaitForSeconds(RechargingTime / AmmoCapacity);
            }
            _audioControl.PlayOneShot(_audioPlatoon);
            IsRecharging = false; //Перезарядка окончена.
        }
    }
}