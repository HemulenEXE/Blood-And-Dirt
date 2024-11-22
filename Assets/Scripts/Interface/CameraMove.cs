using System;
using UnityEngine;

namespace CameraLogic.CameraMotion
{
    /// <summary>
    /// Осуществляет следование камеры за игроком. Скрипт навешивается на Main camera
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
        public float _speed;
        /// <summary>
        /// Позиция на которой камера держится относительно игрока
        /// </summary>
        public Vector3 _offset;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void Awake()
        {
            _transformCamera = this.GetComponent<Transform>();
            _transformPlayer = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Transform>();

            if (_transformCamera == null) throw new ArgumentNullException("CameraMove: _transformCamera is null");
            if (_transformPlayer == null) throw new ArgumentNullException("CameraMove: _transformPlayer is null");
            if (_speed < 0) throw new ArgumentException("CameraMove: _speed < 0!");
        }
        private void LateUpdate()
        {
            Vector3 distance = _transformPlayer.position + _offset;
            Vector3 newPos = Vector3.Lerp(_transformCamera.position, distance, _speed * Time.deltaTime);
            _transformCamera.position = newPos;
        }
    }

}