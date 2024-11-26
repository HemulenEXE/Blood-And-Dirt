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
        private void Start()
        {
            Destroy(gameObject, _liveTime);
        }
        protected void FixedUpdate()
        {
            Destroy(gameObject, _liveTime);
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
            if (!other.gameObject.CompareTag("Projectile"))
            {
                Debug.Log(other.gameObject.name);
                Destroy(this.gameObject);
            }
        }

        protected void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Projectile"))
            {
                Debug.Log(other.gameObject.name);
                Destroy(this.gameObject);
            }
        }

    }

}