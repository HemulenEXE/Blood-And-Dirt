using System;
using UnityEngine;

/// <summary>
/// Осуществляет следование камеры за игроком. Скрипт навешивается на Main camera
/// </summary>
public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Transform _trans;
    public float speed;
    /// <summary>
    /// Позиция на которой камера держится относительно игрока
    /// </summary>
    public Vector3 offset;

    private void Start()
    {
        if (speed < 0) throw new ArgumentException("speed should be >= 0!");

        _trans = GetComponent<Transform>();
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (_player != null)
        {
            Vector3 distance = _player.position + offset;
            Vector3 newPos = Vector3.Lerp(_trans.position, distance, speed * Time.deltaTime);
            newPos.z = _trans.position.z;
            _trans.position = newPos;
        }
    }
}
