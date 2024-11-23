using System;
using UnityEngine;

namespace CameraLogic.CameraMotion
{
    /// <summary>
    /// Класс, реализующий "следование камеры за игроком".
    /// </summary>
    public class CameraMove : MonoBehaviour
    {
        /// <summary>
        /// Возвращает компонент, отвечающий представление игрока в пространстве.
        /// </summary>
        private Transform _transformPlayer;
        /// <summary>
        /// Возвращает компонент, отвечающий представление камеры в пространстве. 
        /// </summary>
        private Transform _transformCamera;
        /// <summary>
        /// Скорость перемещения камеры.
        /// </summary>
        [SerializeField] private float _speed = 2_000f;
        /// <summary>
        /// Позиция камеры относительно игрока.
        /// </summary>
        public Vector3 _offset;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void Start()
        {
            _transformCamera = this.transform;
            _transformPlayer = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (_transformCamera == null) throw new ArgumentNullException("CameraMove: _transformCamera is null");
            if (_transformPlayer == null) throw new ArgumentNullException("CameraMove: _transformPlayer is null");
            if (_speed < 0) throw new ArgumentException("CameraMove: _speed < 0");
        }
        private void Update()
        {
            Vector3 distance = _transformPlayer.position + _offset;
            Vector3 newPos = Vector3.Lerp(_transformCamera.position, distance, _speed * Time.deltaTime); //Плавное перемещение камеры.
            _transformCamera.position = newPos;
        }
    }
}