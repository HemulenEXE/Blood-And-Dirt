using System;
using UnityEngine;

namespace Gun
{
    /// <summary>
    /// Класс, реализующий "жизненный цикл снаряда".
    /// </summary>
    public class ProjectileData : MonoBehaviour
    {
        /// <summary>
        /// Тип оружия, из которого вылетел снаряд.
        /// </summary>
        public GunType GunType;
        /// <summary>
        /// Время жизни.
        /// </summary>
        protected float _liveTime = 5.5f;
        /// <summary>
        /// Наносимый урон.
        /// </summary>
        private float _damage;
        /// <summary>
        /// Возвращает и изменяет величину наносимого урона.
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
        /// При столкновении с объектом снаряд уничтожается.
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