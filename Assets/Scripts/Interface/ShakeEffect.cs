using System;
using System.Collections;
using UnityEngine;

namespace CameraLogic.CameraEffects
{
    /// <summary>
    /// Класс, реализующий "тряску камеры".
    /// </summary>
    public class ShakeEffect : MonoBehaviour
    {
        /// <summary>
        /// Возвращает компонент, отвечающий представление камеры в пространстве. 
        /// </summary>
        private Transform _transformCamera;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void Awake()
        {
            _transformCamera = this.GetComponent<Transform>();

            if (_transformCamera == null) throw new ArgumentNullException("ShakeEffect: _transformCamera is null");
        }
        /// <summary>
        /// Вызов эффекта тряски камеры в течении указанного времени и с указанной амплитудой.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="amplitude"></param>
        public void ShakeCamera(float time, float amplitude)
        {
            if (time <= 0) throw new ArgumentException("ShakeEffect: time <= 0");
            if (amplitude <= 0) throw new ArgumentException("ShakeEffect: amplitude <= 0");

            StartCoroutine(_ShakeCamera(time, amplitude));
        }
        /// <summary>
        /// Корутина тряски камеры.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="amplitude"></param>
        /// <returns></returns>
        private IEnumerator _ShakeCamera(float time, float amplitude)
        {
            if (time <= 0) throw new ArgumentException("ShakeEffect: time <= 0");
            if (amplitude <= 0) throw new ArgumentException("ShakeEffect: amplitude <= 0");

            Vector3 startPose = _transformCamera.position;
            while (time > 0)
            {
                yield return new WaitForSeconds(0.025f);

                float x = UnityEngine.Random.Range(-amplitude, amplitude);
                float y = UnityEngine.Random.Range(-amplitude, amplitude);
                _transformCamera.position = new Vector3(x, y, startPose.z);
                time -= Time.deltaTime;
            }
            _transformCamera.position = startPose;
        }
    }
}