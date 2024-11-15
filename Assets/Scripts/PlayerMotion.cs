using System;
using UnityEngine;

/// <summary>
/// Класс передвижения игрока посредством клавиш WASD и компьютерной мыши.
/// </summary>
public class PlayerMotion : MonoBehaviour
{
    /// <summary>
    /// Промежуток времени.
    /// </summary>
    private static float _deltaTime;
    /// <summary>
    /// Главная камера.
    /// </summary>
    private Camera _mainCamera;
    /// <summary>
    /// Пешая скорость.
    /// </summary>
    [SerializeField] private float _speedWalk = 4;
    /// <summary>
    /// Скорость бега.
    /// </summary>
    [SerializeField] private float _speedRun = 8;
    private void Awake()
    {
        //Настройка полей
        _mainCamera = Camera.main;
        _deltaTime = Time.deltaTime;

        if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
        if (_speedRun < 0) throw new ArgumentOutOfRangeException("PlayerMotion: _speedRun < 0");
        if (_speedWalk < 0) throw new ArgumentOutOfRangeException("PlayerMotion: _speedWalk < 0");
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
        //Текущая скорость игрока в зависимости от состояния нажатия клавиши LeftShift
        float speedCurrent = Input.GetKey(KeyCode.LeftShift) ? _speedRun : _speedWalk;

        //Отслеживание нажатия клавиш
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speedCurrent * _deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speedCurrent * _deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * speedCurrent * _deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * speedCurrent * _deltaTime;
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
        this.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
