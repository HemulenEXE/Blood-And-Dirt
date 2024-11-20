using System;
using UnityEngine;

namespace Gun
{
    /// <summary>
    /// �����, ����������� "��������� ���� �������".
    /// </summary>
    public class ProjectileData : MonoBehaviour
    {
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
        /// <summary>
        /// ���������� � �������� �������� ���������� �����.
        /// </summary>
        public float Damage
        {
            get => _damage;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("ProjectileData: value < 0");
                _damage = value;
            }
        }
        protected void FixedUpdate()
        {
            _liveTime -= Time.fixedDeltaTime;
            if (_liveTime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        /// <summary>
        /// ��� ������������ � �������� ������ ������������.
        /// </summary>
        /// <param name="other"></param>
        protected void OnCollisionStay2D(Collision2D other)
        {
            Debug.Log(other.gameObject.name);
            if (!other.gameObject.CompareTag("Projectile"))
            {
                Debug.Log(other.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }

}