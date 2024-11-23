using Grenades;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "управление броска гранаты игроком".
    /// </summary>
    public class PlayerGrenade : MonoBehaviour
    {
        /// <summary>
        /// Префаб простой гранаты.
        /// </summary>
        [SerializeField] private SimpleGrenade _prefabSimpleGrenade;
        /// <summary>
        /// Префаб дымовой гранаты.
        /// </summary>
        [SerializeField] private SmokeGrenade _prefabSmokeGrenade;
        /// <summary>
        /// Камера.
        /// </summary>
        private Camera _camera;
        private float _time = 1.3f;
        public float Time { get => _time; }
        private void Awake()
        {
            _camera = Camera.main;
        }
        private void Update()
        {
            var counterGrenade = int.Parse(GameObject.Find("simpleGraned")?.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            Debug.Log("1 <=>" + counterGrenade);
            if (Input.GetKeyDown(KeyCode.Z) && counterGrenade > 0)
            {
                Debug.Log("Throw");
                ThrowGranade(_prefabSimpleGrenade);
            }
            counterGrenade = int.Parse(GameObject.Find("smokeGraned")?.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            Debug.Log("2 <=>" + counterGrenade);
            if (Input.GetKeyDown(KeyCode.X) && counterGrenade > 0)
            {
                Debug.Log("Throw");
                ThrowGranade(_prefabSmokeGrenade);
            }

        }
        /// <summary>
        /// Бросок гранаты.
        /// </summary>
        public void ThrowGranade(SimpleGrenade grenade)
        {
            StartCoroutine(_ThrowGranade(grenade));
        }
        /// <summary>
        /// Бросок гранаты
        /// </summary>
        public IEnumerator _ThrowGranade(SimpleGrenade grenade)
        {
            var current_grenade = Instantiate(grenade, this.transform.position, Quaternion.identity);

            Vector2 cursor = Input.mousePosition;
            cursor = _camera.ScreenToWorldPoint(cursor);

            Vector2 startPoint = this.transform.position; //Стартовая позиция
            Vector2 endPoint = new Vector2(cursor.x, this.transform.position.y); //Конечная позиция (Не сам курсор, а точка под ним на уровне игрока)
            float amplitude = Math.Abs(this.transform.position.y - cursor.y); //Амплитуда броска (Высота на которой находиться курсор по отношению к ироку)

            grenade.GetComponent<SimpleGrenade>().TimeToExplosion = Time * 0.65f;

            float elapsedTime = 0f;

            while (elapsedTime < Time)
            {
                float t = elapsedTime / Time; // Нормализуем время (Для того, чтобы отмерить как должны были за этот промежуток измениться координаты)
                float angle = Mathf.Lerp(0, Mathf.PI, t); // Линейная интерполяция угла от 0 до π (полукруг)

                // Вычисляем координаты
                float x = Mathf.Lerp(startPoint.x, endPoint.x, t);
                float y = amplitude * Mathf.Sin(angle) + Mathf.Min(startPoint.y, endPoint.y); ; // По y передвигаемся по синусоиде (Минимальная координа по y для корректировки движения)

                if (current_grenade != null)
                    current_grenade.transform.position = new Vector3(x, y, 0);
                else StopCoroutine(_ThrowGranade(grenade));

                elapsedTime += UnityEngine.Time.deltaTime;

                yield return null; // Ждем следующего кадра
            }
        }
    }
}