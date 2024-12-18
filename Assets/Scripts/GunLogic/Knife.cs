using System.Collections.Generic;
using UnityEngine;
using System;

namespace GunLogic
{
    /// <summary>
    /// Класс, реализующий "нож".
    /// </summary>
    public class Knife : MonoBehaviour
    {
        //Поля.

        /// <summary>
        /// Наносимый урон.
        /// </summary>
        [SerializeField] private float _damage = 100f;
        /// <summary>
        /// Задержка атаки.
        /// </summary>
        [SerializeField] private float _attackDelay = 1f;
        /// <summary>
        /// Время до следующей атаки.
        /// </summary>
        private float _nextAttackTime = 0f;
        /// <summary>
        /// Радиус атаки.
        /// </summary>
        [SerializeField] private float _attackAngle = 45f;
        /// <summary>
        /// Дистанция атаки
        /// </summary>
        [SerializeField] private float _attackDistance = 1.2f;
        /// <summary>
        /// Игнорируемый слой.
        /// </summary>
        [SerializeField] private LayerMask _ignoreLayer;
        /// <summary>
        /// Звук аттаки.
        /// </summary>
        [SerializeField] private AudioClip _attackSound;
        /// <summary>
        /// Компонент, отвечающий за управление аудио.
        /// </summary>
        private AudioSource _audio;

        //Свойства.

        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Light;
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        public float Damage
        {
            get
            {
                return _damage;
            }
        }

        //Встроенные методы.

        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        private void Awake()
        {
            _audio = this.GetComponent<AudioSource>();
            if (_audio == null) throw new ArgumentNullException("Knife: _audio is null");
        }

        /// <summary>
        /// Рисовка площади поражения.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            float startAngle = -_attackAngle / 2f;
            float endAngle = _attackAngle / 2f;
            Vector3 startPoint = this.transform.position + this.transform.rotation * Quaternion.Euler(0, 0, startAngle) * Vector3.right * _attackDistance;
            Vector3 endPoint = this.transform.position + this.transform.rotation * Quaternion.Euler(0, 0, endAngle) * Vector3.right * _attackDistance;
            Gizmos.DrawLine(this.transform.position, startPoint);
            Gizmos.DrawLine(this.transform.position, endPoint);
            int segments = 20;
            float angleStep = _attackAngle / segments;
            Vector3 previousPoint = startPoint;
            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = startAngle + angleStep * i;
                Vector3 currentPoint = this.transform.position + this.transform.rotation * Quaternion.Euler(0, 0, currentAngle) * Vector3.right * _attackDistance;
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
            Gizmos.DrawLine(previousPoint, endPoint);
        }

        //Вспомогательные методы.

        /// <summary>
        /// Нанесение урона нескольким сущностям.
        /// </summary>
        /// <param name="entity"></param>
        public void DealDamage()
        {
            if (_nextAttackTime <= 0)
            {
                Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
                Debug.DrawRay(ray.origin, ray.direction * _attackDistance, Color.red); //Рисовка луча.

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _attackDistance, ~_ignoreLayer);
                if (hit.collider == null)
                {
                    //_audio.PlayOneShot(_attackSound);
                    foreach (var x in GetColliders2DSector())
                    {
                        //Логика получения урона сущностями.
                        if (x.gameObject != this.gameObject)
                        {
                            //Destroy(x.gameObject);
                            var healthBot = x.GetComponent<HealthBot>();
                            healthBot?.GetDamage(this);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Возвращает коллайдеры 2D в заданном секторе.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Collider2D> GetColliders2DSector()
        {
            Collider2D[] interim_colliders = Physics2D.OverlapCircleAll(this.transform.position, _attackDistance);
            foreach (var x in interim_colliders)
            {
                float angle = Vector2.Angle(this.transform.right, x.transform.position - this.transform.position);
                if (angle <= _attackAngle / 2)
                {
                    yield return x;
                }
            }
        }
    }
}
