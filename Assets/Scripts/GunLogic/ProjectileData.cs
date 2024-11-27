using System;
using UnityEngine;

namespace GunLogic
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
        private void Start()
        {
            Destroy(gameObject, _liveTime);
        }
        protected void FixedUpdate()
        {
            Destroy(this.gameObject, _liveTime); //Это лучше, чем использование условного оператора if.
        }
        /// <summary>
        /// Уничтожение снаряда при столкновении с объектами.
        /// </summary>
        /// <param name="other"></param>
        protected void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("gun"))
            {
                //Debug.Log(other.gameObject.name);
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