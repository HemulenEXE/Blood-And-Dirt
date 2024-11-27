using System.Collections.Generic;
using UnityEngine;
using GunLogic;

namespace Guns
{
    /// <summary>
    /// Класс, реализующий "нож".
    /// </summary>
    public class Knife : MonoBehaviour
    {
        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Light;
        /// <summary>
        /// Наносимый урон.
        /// </summary>
        [SerializeField] protected float _damage = 2f;
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        public float Damage { get => _damage; }
        /// <summary>
        /// Радиус атаки.
        /// </summary>
        public float _attackAngle = 45f;
        /// <summary>
        /// Дистанция атаки
        /// </summary>
        public float _attackDistance = 3f;
        /// <summary>
        /// Нанесение урона одной нескольким сущностям.
        /// </summary>
        /// <param name="entity"></param>
        public void DealDamage()
        {
            foreach (var x in GetColliders2DSector())
            {
                //Логика получения урона сущностями.
                if (x.gameObject != this.gameObject)
                {
                    Destroy(x.gameObject);
                }
            }
        }
        /// <summary>
        /// Возвращает 
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
    }
}
