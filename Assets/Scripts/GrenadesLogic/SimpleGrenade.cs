using CameraLogic.CameraEffects;
using System;
using UnityEngine;

namespace Grenades
{
    /// <summary>
    /// Класс, реализующий "простую гранату".
    /// </summary>
    public class SimpleGrenade : MonoBehaviour
    {
        /// <summary>
        /// Время до взрыва.
        /// </summary>
        [SerializeField] private float _timeToExplosion = 0;
        /// <summary>
        /// Возвращает и изменяет время до взрыва.
        /// </summary>
        public float TimeToExplosion
        {
            get => _timeToExplosion;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: TimeToExplosion < 0");
                _timeToExplosion = value;
            }
        }
        /// <summary>
        ///  Радиус взрыва.
        /// </summary>
        [SerializeField] private float _explosionRadius = 0;
        /// <summary>
        /// Возвращает радиус взрыва.
        /// </summary>
        public float ExplosionRadius { get => _explosionRadius; }
        /// <summary>
        /// Урон от взрыва.
        /// </summary>
        [SerializeField] private float _damageExplosion = 0;
        /// <summary>
        /// Возвращает урон от взрыва.
        /// </summary>
        public float DamageExplosion { get => _damageExplosion; }
        /// <summary>
        /// Возвращает флаг, указывающий активирована ли граната.
        /// </summary>
        public bool IsActive { get; protected set; } = false;
        /// <summary>
        /// Камера.
        /// </summary>
        private Camera _camera;
        /// <summary>
        /// Игрок.
        /// </summary>
        private GameObject _player;
        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual void Awake()
        {
            _camera = Camera.main;
            _player = GameObject.FindGameObjectWithTag("Player");

            if (TimeToExplosion < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: TimeToExplosion < 0");
            if (ExplosionRadius < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: ExplosionRadius < 0");
            if (DamageExplosion < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: DamageExplosion < 0");
        }
        protected virtual void Update()
        {
            _timeToExplosion -= Time.deltaTime;
            if (_timeToExplosion <= 0 && !IsActive)
            {
                IsActive = true;
                Explode();
                Crash();
            }
        }
        /// <summary>
        /// Отслеживание входа коллизии.
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (other != null && !other.gameObject.CompareTag("Player"))
            {
                Explode();
                Crash();
            }
        }
        /// <summary>
        /// Взрыв гранаты.
        /// </summary>
        public virtual void Explode()
        {
            IsActive = true;
            Collider2D[] entity_colliders = Physics2D.OverlapCircleAll(this.transform.position, ExplosionRadius); //Получаем коллайдеры всех сущностей поблизости.
            foreach (var x in entity_colliders)
            {
                //Логика получения урона.
                if (x.gameObject != this.gameObject)
                {
                    //Destroy(x.gameObject);
                }
            }
            Destroy(this.gameObject);
        }
        /// <summary>
        /// Вызов тряски камеры после взрыва гранаты. 
        /// </summary>
        protected virtual void Crash()
        {
            float distance = Vector3.Distance(_player.transform.position, transform.position);

            //Тряска тем больше, чем ближе к игроку упала граната 
            if (distance <= 5) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.6f);
            else if (distance <= 10) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.3f);
            else _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.08f);
        }
        /// <summary>
        /// Рисовка площади поражения.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Устанавливаем цвет Gizmos
            Gizmos.color = Color.red;
            // Рисуем круг поражения
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}