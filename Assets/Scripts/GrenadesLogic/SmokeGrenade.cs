using System;
using System.Collections;
using UnityEngine;

namespace Grenades
{
    /// <summary>
    /// Класс, реализующий "дымовую гранату".
    /// </summary>
    public class SmokeGrenade : SimpleGrenade
    {
        //Поля.

        /// <summary>
        /// Партикл дыма.
        /// </summary>
        [SerializeField] protected ParticleSystem _particle;
        /// <summary>
        /// Продолжительность работы партикла дыма.
        /// </summary>
        [SerializeField] private float _smokeDuraion = 5f;

        //Свойства.

        /// <summary>
        /// Возвращает продолжительность работы партикла дыма.
        /// </summary>
        public float SmokeDuraion
        {
            get
            {
                return _smokeDuraion;
            }
        }

        //Методы.

        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected override void Awake()
        {
            base.Awake();
            if ( _particle == null)
            {
                _particle = this.GetComponentInChildren<ParticleSystem>();
            }

            if (_particle == null) throw new ArgumentNullException("SmokeGrenade: _particle is null");
            if (SmokeDuraion < 0) throw new ArgumentOutOfRangeException("SmokeGrenade: _smokeDuraion < 0");
        }
        private void OnDestroy()
        {
            _particle.Stop();
        }
        /// <summary>
        /// Взрыв гранаты.
        /// </summary>
        public override void Explode()
        {
            _particle.Play();
            IsActivated = true;

            CircleCollider2D smokeField = this.GetComponent<CircleCollider2D>(); //Установка коллайдера, чтобы враги путались в дыме.
            smokeField.radius = ExplosionRadius;
            //Враги не получают урон в дыму.
            Destroy(this.gameObject, _smokeDuraion);
        }
    }
}
