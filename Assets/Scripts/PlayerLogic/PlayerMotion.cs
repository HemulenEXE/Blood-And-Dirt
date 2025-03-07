using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerLogic
{

    public class PlayerMotion : MonoBehaviour
    {
        /// <summary>
        /// Достаточно маленький промежуток времени.
        private float _deltaTime;
        /// Главная камера.
        /// </summary>
        private Camera _mainCamera;
        /// <summary>
        /// скорость ползком
        /// </summary>
        [SerializeField] private float _stealSpeed = 2f;
        /// <summary>
        /// Пешая скорость.
        /// </summary>
        [SerializeField] private float _walkSpeed = 4f;
        /// <summary>
        /// Скорость бега.
        /// </summary>
        [SerializeField] private float _runSpeed = 6f;
        /// <summary>
        /// шум ползком
        /// </summary>
        [SerializeField] private float _stealNoise = 0.3f;
        /// <summary>
        /// Пеший шум.
        /// </summary>
        [SerializeField] private float _walkNoise = 2f;
        /// <summary>
        /// шум бега.
        /// </summary>
        [SerializeField] private float _runNoise = 8f;

        public static event Action<Transform, float> makeNoise;

        private Dictionary<float, float> noiseMapping;
        /// <summary>
        /// Возвращает и приватно изменяет флаг, указывающий, движется ли игрок.
        /// </summary>
        public bool IsMoving { get; private set; }
        /// <summary>
        /// Возвращает и приватно изменяет флаг, указывающий, бежит ли игрок.
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Awake()
        {
            _mainCamera = Camera.main;
            _deltaTime = Time.fixedDeltaTime;
            noiseMapping = new Dictionary<float, float>
        {
            { _stealSpeed, _stealNoise },
            { _walkSpeed, _walkNoise },
            { _runSpeed, _runNoise }
        };

            if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
            if (_runSpeed < 0) throw new ArgumentOutOfRangeException("PlayerMotion: _speedRun < 0");
            if (_walkSpeed < 0) throw new ArgumentOutOfRangeException("PlayerMotion: _speedWalk < 0");
        }
        private void FixedUpdate()
        {
            Move();
            Rotate();
        }
        /// <summary>
        /// Передвижение игрока посредством клавиш WASD.
        /// </summary>
        private void Move()
        {
            IsMoving = false;
            IsRunning = false;
            //Текущая скорость игрока в зависимости от состояния нажатия клавиши LeftShift.
            float speedCurrent = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
            Vector3 movement = Vector2.zero;
            //Отслеживание нажатия клавиш.
            if (Input.GetKey(KeyCode.A))
            {
                movement += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                movement += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += Vector3.down;
            }
            if (movement != Vector3.zero)
            {
                this.transform.position += movement.normalized * speedCurrent * _deltaTime;
                IsMoving = true;
                IsRunning = speedCurrent.Equals(_runSpeed);
                makeNoise?.Invoke(transform, noiseMapping[speedCurrent]);
            }
        }
        /// <summary>
        /// Поворот игрока за компьтерной мышью.
        /// </summary>
        private void Rotate()
        {
            //Вычисление положения компьютерной мыши в мировом пространстве
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

            //Вычисление угла поворота игрока за мышью
            Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //Преобразование радиан в градусы - равен 360 / (2 * pi)
            this.transform.rotation = Quaternion.Euler(Vector3.forward* angle);
        }

    }
}
    