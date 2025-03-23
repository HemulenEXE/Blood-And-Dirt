using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GunLogic
{
    /// <summary>
    /// �����, ����������� "�������".
    /// </summary>
    public class MachineGun : MonoBehaviour, IGun
    {
        /// <summary>
        /// ������ ����, ���������� �� ��������.
        /// </summary>
        [SerializeField] protected GameObject _prefabProjectile;
        /// <summary>
        /// ��������� ����.
        /// </summary>
        [SerializeField] protected float _damage = 6.5f;
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
        /// ��������� �������� ������ (��� �����)
        /// </summary>
        [SerializeField] protected float attackRange;
        /// <summary>
        /// �������� ��������� ����� (��� �����)
        /// </summary>
        public float AttackRange { get; set; }
        /// <summary>
        /// ���������� ��� ������.
        /// </summary>
        public GunType Type { get; } = GunType.Heavy;
        /// <summary>
        /// ���������� � �������� ������� ����� �������� � �������.
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
        /// ���������� �������� ���������� �����.
        /// </summary>
        public float Damage
        {
            get
            {
                return _damage;
            }
        }
        /// <summary>
        /// ���������� ����, �����������, ��� �� �����������.
        /// </summary>
        public bool IsRecharging { get; set; } = false;
        /// <summary>
        /// ���������� ����, �����������, ��� �� ��������.
        /// </summary>
        public bool IsShooting { get; set; } = false;
        /// <summary>
        /// ���������� ��������� ����� ��������.
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
        public void Shoot(Side sideShooter, bool IsPlayerShoot = false)
        {
            if (!IsShooting && !IsRecharging && Time.time > _nextTimeShot)
            {
                if (AmmoTotalCurrent > 0)
                {
                    IsShooting = true;
                    _nextTimeShot = Time.time + _delayShot;
                    _audio.PlayOneShot(_audioFire, 0.5f);

                    GameObject currentPellet = Instantiate(_prefabProjectile, this.transform.GetChild(0).position, this.transform.GetChild(0).rotation); //����� �������.

                    var interim_projectile_component = currentPellet.GetComponent<ProjectileData>();
                    if (interim_projectile_component != null)
                    {
                        interim_projectile_component.sideBullet = sideShooter.CreateSideBullet();
                        interim_projectile_component.Damage = this._damage;
                        interim_projectile_component.GunType = Type;
                    }

                    Rigidbody2D rg = currentPellet.GetComponent<Rigidbody2D>();
                    if (rg == null) throw new ArgumentNullException("MachineGun: _prefabProjectile hasn't got Rigidbody2D");
                    //rg.isKinematic = true; // �������������� ��������

                    currentPellet.layer = LayerMask.NameToLayer(sideShooter.GetOwnLayer());

                    // ������ ������� ��� ���������� ��������� ����
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
        /// ��������� �������� �� ��������.<br/>
        /// �� �������� ����������.
        /// </summary>
        public void StopShoot() { }
        /// <summary>
        /// ����������� ��������.
        /// </summary>
        public void Recharge()
        {
            if (AmmoTotal > 0 && !IsShooting && !IsRecharging)
            {
                IsRecharging = true; //�������� �����������.
                StartCoroutine(RechargeCoroutine());
            }
        }
        /// <summary>
        /// ���������, ���� �� ��������.
        /// </summary>
        public bool IsEmpty() => AmmoTotal == 0 && AmmoTotalCurrent == 0;
        /// <summary>
        /// �������� ��� ����������� ��������.<br/>
        /// �������� �� ������������� �������� � ������ �����.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RechargeCoroutine()
        {
            yield return new WaitForSeconds(_timeRecharging);
            _audio.PlayOneShot(_audioRecharge);
            int count_need_patrons = AmmoCapacity - AmmoTotalCurrent; //���������� ����������� ��������.
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
            IsRecharging = false; //����������� ��������.
        }
        /// <summary>
        /// ���������, ����������� �� ���������� �������� �� ����
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool IsInRange(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= attackRange;
        }
    }
}