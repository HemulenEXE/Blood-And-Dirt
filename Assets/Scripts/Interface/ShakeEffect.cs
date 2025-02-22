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
        /// Вызов эффекта тряски камеры в течении указанного времени и с указанной амплитудой.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="amplitude"></param>
        public void ShakeCamera(float time, float amplitude)
        {
            if (time <= 0) throw new ArgumentException("ShakeEffect: time <= 0");
            if (amplitude <= 0) throw new ArgumentException("ShakeEffect: amplitude <= 0");

            StartCoroutine(CoroutineShakeCamera(time, amplitude));
        }
        /// <summary>
        /// Корутина для тряски камеры.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="amplitude"></param>
        /// <returns></returns>
        private IEnumerator CoroutineShakeCamera(float time, float amplitude)
        {
            if (time <= 0) throw new ArgumentException("ShakeEffect: time <= 0");
            if (amplitude <= 0) throw new ArgumentException("ShakeEffect: amplitude <= 0");

            Vector3 startPose = this.transform.position;
            while (time > 0)
            {
                yield return new WaitForSeconds(0.025f);

                float x = UnityEngine.Random.Range(-amplitude, amplitude);
                float y = UnityEngine.Random.Range(-amplitude, amplitude);
                this.transform.position = new Vector3(startPose.x + x, startPose.y + y, startPose.z);
                time -= Time.deltaTime;
            }
            this.transform.position = startPose;
        }
    }
}