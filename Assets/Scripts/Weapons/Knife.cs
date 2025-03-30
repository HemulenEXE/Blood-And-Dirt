using System.Collections.Generic;
using UnityEngine;

namespace GunLogic
{
    /// <summary>
    /// Класс, реализующий "нож".
    /// </summary>
    public class Knife : MonoBehaviour
    {
        //Поля.

        /// <summary>
        /// Игнорируемый слой.
        /// </summary>
        [SerializeField] private LayerMask _ignoreLayer;

        //Автосвойства.

        /// <summary>
        /// Возвращает наносимый урон.
        /// </summary>
        [field: SerializeField] public float Damage { get; private set; } = 10f;
        /// <summary>
        /// Возвращает задержку аттаки.
        /// </summary>
        [field: SerializeField] public float AttackDelay { get; private set; } = 1f;
        /// <summary>
        /// Возвращает радиус аттаки.
        /// </summary>
        [field: SerializeField] public float AttackAngle { get; private set; } = 45f;
        /// <summary>
        /// Возвращает дистанцию аттаки.
        /// </summary>
        [field: SerializeField] public float AttackDistance { get; private set; } = 1.2f;
        /// <summary>
        /// Возвращает звук аттаки.
        /// </summary>
        [field: SerializeField] public AudioClip AttackSound { get; private set; }
        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; } = GunType.Light;

        //Встроенные методы.

        /// <summary>
        /// Рисовка площади поражения.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            float startAngle = -AttackAngle / 2f;
            float endAngle = AttackAngle / 2f;
            Vector3 startPoint = this.transform.position + this.transform.rotation * Quaternion.Euler(0, 0, startAngle) * Vector3.right * AttackDistance;
            Vector3 endPoint = this.transform.position + this.transform.rotation * Quaternion.Euler(0, 0, endAngle) * Vector3.right * AttackDistance;
            Gizmos.DrawLine(this.transform.position, startPoint);
            Gizmos.DrawLine(this.transform.position, endPoint);
            int segments = 20;
            float angleStep = AttackAngle / segments;
            Vector3 previousPoint = startPoint;
            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = startAngle + angleStep * i;
                Vector3 currentPoint = this.transform.position + this.transform.rotation * Quaternion.Euler(0, 0, currentAngle) * Vector3.right * AttackDistance;
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
            Gizmos.DrawLine(previousPoint, endPoint);
        }

        //Вспомогательные методы.

        public void DealDamage()
        {
            Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
            Debug.DrawRay(ray.origin, ray.direction * AttackDistance, Color.red); //Рисовка луча.

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, AttackDistance, ~_ignoreLayer);
            if (hit.collider != null)
            {
                foreach (var x in GetColliders2DSector())
                {
                    if (x.gameObject != this.gameObject)
                    {
                        Debug.Log(x.gameObject.name);
                        var healthBot = x.GetComponent<HealthBot>();
                        healthBot?.GetDamage(this);
                    }
                }
            }
        }
        public void InstantKill()
        {
            Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
            Debug.DrawRay(ray.origin, ray.direction * AttackDistance, Color.red); //Рисовка луча.

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, AttackDistance, ~_ignoreLayer);
            if (hit.collider == null)
            {
                foreach (var x in GetColliders2DSector())
                {
                    if (x.gameObject != this.gameObject)
                    {
                        var healthBot = x.GetComponent<HealthBot>();
                        healthBot.GetDamage(this);
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
            Collider2D[] interim_colliders = Physics2D.OverlapCircleAll(this.transform.position, AttackDistance);
            foreach (var x in interim_colliders)
            {
                float angle = Vector2.Angle(this.transform.right, x.transform.position - this.transform.position);
                if (angle <= AttackAngle)
                {
                    yield return x;
                }
            }
        }
    }
}
