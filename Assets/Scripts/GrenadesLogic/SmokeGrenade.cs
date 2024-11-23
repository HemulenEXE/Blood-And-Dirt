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
        /// <summary>
        /// Партикл дыма.
        /// </summary>
        [SerializeField] protected ParticleSystem _particle;
        /// <summary>
        /// Продолжительность активации партикла дыма.
        /// </summary>
        [SerializeField] protected float _smokeDuraion;
        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected override void Awake()
        {
            base.Awake();
            if (_particle == null) throw new ArgumentNullException("SmokeGrenade: _particle is null");
            if (_smokeDuraion < 0) throw new ArgumentOutOfRangeException("SmokeGrenade: _smokeDuraion < 0");
        }
        /// <summary>
        /// Взрыв гранаты.
        /// </summary>
        public override void Explode()
        {
            _particle.Play();
            Collider2D[] entity_colliders = Physics2D.OverlapCircleAll(this.transform.position, ExplosionRadius); //Получаем коллайдеры всех сущностей поблизости.
            foreach (var x in entity_colliders)
            {
                //Логика получения урона.
            }
            StartCoroutine(CorutineExplode());
        }
        /// <summary>
        /// Корутина для взрыва гранаты.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CorutineExplode()
        {
            yield return new WaitForSeconds(_smokeDuraion);
            _particle.Stop();
            Destroy(this.gameObject);
        }
    }
}
