using System;
using UnityEngine;

namespace GunLogic
{
    /// <summary>
    /// �����, ����������� "��������� ���� �������".
    /// </summary>
    public class ProjectileData : MonoBehaviour
    {
        //����.

        /// <summary>
        /// ��� ������, �� �������� ������� ������.
        /// </summary>
        public GunType GunType;
        /// <summary>
        /// ����� �����.
        /// </summary>
        protected float _liveTime = 5.5f;
        /// <summary>
        /// ��������� ����.
        /// </summary>
        private float _damage;

        //��������.

        /// <summary>
        /// ���������� � �������� �������� ���������� �����.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public float Damage
        {
            get => _damage;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("ProjectileData: value < 0");
                _damage = value;
            }
        }

        //������.

        private void Start()
        {
            Destroy(this.gameObject, _liveTime);
        }
        /// <summary>
        /// ����������� ������� ��� ������������ � ���������.
        /// </summary>
        /// <param name="other"></param>
        protected void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("Gun"))
            {
                Destroy(this.gameObject);
            }
        }

    }
}