using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, реализующий "передвижение игрока".
/// </summary>
public class PlayerMotion : MonoBehaviour
{
    /// <summary>
    /// Достаточно маленький промежуток времени.
    private float _deltaTime;
    /// Главная камера.
    /// </summary>
    private Camera _mainCamera;
    /// <summary>
    /// Компонент, управляющий анимациями игрока.
    /// </summary>
    private Animator _animator;
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
    [SerializeField] private float _runSpeed = 8f;
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
    private float _currentSpeed;

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

    public static event Action<Transform, float> makeNoise;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _deltaTime = Time.fixedDeltaTime;
        _animator = this.transform.GetChild(0).GetComponent<Animator>(); //0-ым компонентом (ребёнком) должно быть визуально представление игрока.
        noiseMapping = new Dictionary<float, float>
        {
            { _stealSpeed, _stealNoise },
            { _walkSpeed, _walkNoise },
            { _runSpeed, _runNoise }
        };

        if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
        if (_animator == null) throw new ArgumentNullException("PlayerMotion: _animator is mull");
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
        speedCurrent = Input.GetKey(KeyCode.LeftControl) ? _stealSpeed : speedCurrent;
        //Отслеживание нажатия клавиш.
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speedCurrent * _deltaTime;
            IsMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speedCurrent * _deltaTime;
            IsMoving = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * speedCurrent * _deltaTime;
            IsMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * speedCurrent * _deltaTime;
            IsMoving = true;
        }
        if (IsMoving)
        {
            IsRunning = speedCurrent.Equals(_runSpeed);
            makeNoise?.Invoke(transform, noiseMapping[speedCurrent]);
        }
        
        _animator.SetBool("IsMoving", IsMoving);
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
