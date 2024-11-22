﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Навешивается на Main Camera
/// </summary>
public class ShakeEffect : MonoBehaviour
{
    private Transform _camera;
    private void Start()
    {
        _camera = GetComponent<Transform>();
    }
    /// <summary>
    /// Эффект тряски камеры в течении указанного времени и с указанной амплитудой 
    /// </summary>
    /// <param name="time"></param>
    /// <param name="amplitude"></param>
    public void ShakeCamera(float time, float amplitude)
    {
        if (time <= 0) throw new ArgumentException("time should be > 0!");
        if (amplitude <= 0) throw new ArgumentException("amplitude should be > 0!");

        StartCoroutine(_ShakeCamera(time, amplitude));
    }
    private IEnumerator _ShakeCamera(float time, float amplitude)
    {
        Vector3 startPose = _camera.position;

        while (time > 0)
        {
            yield return new WaitForSeconds(0.025f);

            float x = UnityEngine.Random.Range(-amplitude, amplitude);
            float y = UnityEngine.Random.Range(-amplitude, amplitude);

            _camera.position = new Vector3(x, y, startPose.z);
            time -= Time.deltaTime;
        }
        _camera.position = startPose;
    }

}
