using System;
using System.Collections;
using UnityEngine;

namespace GunLogic
{
    /// <summary>
    /// �����, ����������� "�������.
    /// </summary>
    public class MachineGun : MonoBehaviour, IGun
    {
        /// <summary>
        /// ���������� ��� ������.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// ������ ����, ���������� �� ��������.
        /// </summary>
        [SerializeField] protected GameObject _prefabProjectile;
        /// <summary>
        /// ��������� ����.
        /// </summary>
        [SerializeField] protected float _damage = 6.5f;
        /// <summary>
        /// ���������� �������� ���������� �����.
        /// </summary>
        public float Damage { get => _damage; }
        /// <summary>
        /// �������� ����� ����������.
        /// </summary>
        [SerializeField] protected float _delayShot = 0.05f;
        /// <summary>
        /// ����� �� ���������� ��������.
        /// </summary>
        protected float _nextTimeShot = 0f;
        /// <summary>
        /// ��������� ����� ��������.
        /// </summary>
        [SerializeField] protected int _ammoTotal = 100;
        /// <summary>
        /// ���������� � �������� ��������� ����� ��������.
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
        /// ����������� �������.
        /// </summary>
        [SerializeField] protected int _ammoCapacity = 30;
        /// <summary>
        /// ���������� ����������� �������.
        /// </summary>
        public int AmmoCapacity { get => _ammoCapacity; }
        /// <summary>
        /// ������� ����� �������� � �������.
        /// </summary>
        [SerializeField] protected int _ammoTotalCurrent = 0;
        /// <summary>
        /// ���������� � �������� ������� ����� �������� � �������.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// ���� ���� ������ ��� ��������
        /// </summary>
        [SerializeField] private float noiseIntensity = 5;
        /// <summary>
        /// �������� ��� ����
        /// </summary>
        public float NoiseIntensity { get; set; }
        /// <summary>
        /// ������� ������ ������� �� ��� ��������
        /// </summary>
        public static event Action<Transform, float> makeNoiseShooting;
        /// <summary>
        /// ����� �����������.
        /// </summary>
        [SerializeField] protected float _timeRecharging = 1f;
        /// <summary>
        /// ���������� ����, �����������, ��� �� �����������.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// ���������� ����, �����������, ��� �� ��������.
        /// </summary>
        public bool IsShooting { get; set; } = false;
        /// <summary>
        /// ��������� �������� ������ ����.
        /// </summary>
        [SerializeField] protected float _speedProjectile = 50f;
        /// <summary>
        /// ���������, ����������� �������� ������.
        /// </summary>
        protected AudioSource _audio;
        /// <summary>
        /// ���� �������� �� ��������.
        /// </summary>
        [SerializeField] protected AudioClip _audioFire;
        /// <summary>
        /// ���� ����������� ��������.
        /// </summary>
        [SerializeField] protected AudioClip _audioRecharge;
        /// <summary>
        /// ��������� � �������� �����.
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
        /// ������� �� ��������.<br/>
        /// </summary>
        /// <remarks>��������� �� ����� ������, ���������� �� ��������.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public void Shoot(int layerMask = 0, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging && Time.time > _nextTimeShot)
            {
                if (AmmoTotalCurrent > 0)
                {
                    IsShooting = true;
                    _nextTimeShot = Time.time + _delayShot;
                    _audio.PlayOneShot(_audioFire);

                    GameObject currentPellet = Instantiate(_prefabProjectile, this.transform.GetChild(0).position, this.transform.GetChild(0).rotation); //����� �������.

                    var interim_projectile_component = currentPellet.GetComponent<ProjectileData>();
                    if (interim_projectile_component != null)
                    {
                        interim_projectile_component.Damage = this._damage;
                        interim_projectile_component.GunType = Type;
                    }

                    Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                    if (rg == null) throw new ArgumentNullException("MachineGun: _prefabProjectile hasn't got Rigidbody2D");
                    //rg.isKinematic = true; // �������������� ��������

                    currentPellet.layer = layerMask;

                    // ������ ������� ��� ���������� ��������� ����
                    var bulletController = currentPellet.AddComponent<BulletMovement>();
                    bulletController.SetSpeed(_speedProjectile);

                    AmmoTotalCurrent--;
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
        /// ��������� �������� �� ��������.<br/>
        /// �� �������� ����������.
        /// </summary>
        public void StopShoot() { }
        /// <summary>
        /// ����������� ��������.
        /// </summary>
        public void Recharge()
        {
            if (_ammoTotal > 0 && !IsRecharging)
            {
                IsRecharging = true;
                IsShooting = false;
                StartCoroutine(RechargeCoroutine()); //�� ����������� ��������� ��������� �����.
            }
        }
        /// <summary>
        /// �������� ��� ����������� ��������.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RechargeCoroutine()
        {
            yield return new WaitForSeconds(_timeRecharging);
            _audio.PlayOneShot(_audioRecharge);
            AmmoTotal -= AmmoCapacity - AmmoTotalCurrent;
            AmmoTotalCurrent = AmmoCapacity;
            IsRecharging = false;
        }
        /// <summary>
        /// ���������, ���� �� ��������.
        /// </summary>
        public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
    }
}