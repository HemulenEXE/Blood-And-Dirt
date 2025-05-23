﻿using System;
using UnityEngine;

namespace CameraLogic.CameraMotion
{
    /// <summary>
    /// Класс, реализующий "следование камеры за игроком".
    /// </summary>
    public class CameraMove : MonoBehaviour
    {
        /// <summary>
        /// Возвращает компонент, отвечающий представление игрока 
        /// </summary>
        private GameObject _transformPlayer;
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
        private void Awake()
        {
            _transformPlayer = GameObject.FindGameObjectWithTag("Player");

            if (_transformPlayer == null) throw new ArgumentNullException("CameraMove: _transformPlayer is null");
            if (_speed < 0) throw new ArgumentOutOfRangeException("CameraMove: _speed < 0");
        }
        private void Update()
        {
            if (_transformPlayer == null) return;
            if (_transformPlayer.activeSelf)
                Move();
            else _transformPlayer = GameObject.FindGameObjectWithTag("Player");
        }
        /// <summary>
        /// Плавное перемещение камеры.
        /// </summary>
        private void Move()
        {
            if(_transformPlayer.transform != null )
            {
                Vector3 distance = _transformPlayer.transform.position + _offset;
                this.transform.position = Vector3.Lerp(this.transform.position, distance, _speed * Time.deltaTime); //Плавное перемещение камеры.
            }
            
        }
    }
}