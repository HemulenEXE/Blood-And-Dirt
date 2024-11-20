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
        protected void FixedUpdate()
        {
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
            Debug.Log(other.gameObject.name);
            if (!other.gameObject.CompareTag("Projectile"))
            {
                Debug.Log(other.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }

}