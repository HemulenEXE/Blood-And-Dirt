using System;
using System.Collections;
using UnityEngine;
using Grenades;
using InventoryLogic;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "управление броска гранаты игроком".
    /// </summary>
    public class PlayerGrenade : MonoBehaviour
    {
        /// <summary>
        /// Камера.
        /// </summary>
        private Camera _camera;
        /// <summary>
        /// Префаб простой гранаты.
        /// </summary>
        [SerializeField] private SimpleGrenade _prefabSimpleGrenade;
        /// <summary>
        /// Префаб дымовой гранаты.
        /// </summary>
        [SerializeField] private SmokeGrenade _prefabSmokeGrenade;
        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void Awake()
        {
            _camera = Camera.main;

            if (_camera == null) throw new ArgumentNullException("PlayerGrenade: _camera is null");
            if (_prefabSimpleGrenade == null) throw new ArgumentNullException("PlayerGrenade: _prefabSimpleGrenade is null");
            if (_prefabSmokeGrenade == null) throw new ArgumentNullException("PlayerGrenade: _prefabSmokeGrenade is null");
        }
        /// <summary>
        /// Управление броска гранаты игроком.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2) && ConsumableCounter._simpleGrenadeCount > 0)
            {
                ThrowGranade(_prefabSimpleGrenade);
                --ConsumableCounter._simpleGrenadeCount;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && ConsumableCounter._smokeGrenadeCount > 0)
            {
                ThrowGranade(_prefabSmokeGrenade);
                --ConsumableCounter._smokeGrenadeCount;
            }

        }
        /// <summary>
        /// Бросок гранаты.
        /// </summary>
        public void ThrowGranade(SimpleGrenade grenade)
        {
            StartCoroutine(CoroutineThrowGranade(grenade));
        }
        /// <summary>
        /// Корутина для броска гранаты
        /// </summary>
        private IEnumerator CoroutineThrowGranade(SimpleGrenade grenade)
        {
            var current_grenade = Instantiate(grenade, this.transform.position, Quaternion.identity);

            Vector2 cursor = Input.mousePosition;
            cursor = _camera.ScreenToWorldPoint(cursor);

            Vector2 startPoint = this.transform.position; //Стартовая позиция
            Vector2 endPoint = new Vector2(cursor.x, cursor.y); //Конечная позиция (Не сам курсор, а точка под ним на уровне игрока)
            //float amplitude = Math.Abs(this.transform.position.y - cursor.y); //Амплитуда броска (Высота на которой находиться курсор по отношению к ироку)

            float elapsedTime = 0f;

            while (elapsedTime < grenade.TimeToExplosion)
            {
                float t = elapsedTime / grenade.TimeToExplosion; // Нормализуем время (Для того, чтобы отмерить как должны были за этот промежуток измениться координаты)
                //float angle = Mathf.Lerp(0, Mathf.PI, t); // Линейная интерполяция угла от 0 до π (полукруг)

                // Вычисляем координаты
                float x = Mathf.Lerp(startPoint.x, endPoint.x, t);
                //float y = amplitude * Mathf.Sin(angle) + Mathf.Min(startPoint.y, endPoint.y); ; // По y передвигаемся по синусоиде (Минимальная координа по y для корректировки движения)
                float y = Mathf.Lerp(startPoint.y, endPoint.y, t);
                if (current_grenade != null)
                    current_grenade.transform.position = new Vector3(x, y, 0);
                else StopCoroutine(CoroutineThrowGranade(grenade));

                elapsedTime += UnityEngine.Time.deltaTime;

                yield return null; //Ждем следующего кадра.
            }
        }
    }
}