using System;
using UnityEngine;

/// <summary>
/// Класс, реализующий передвижение игрока.
/// </summary>
public class PlayerMotion : MonoBehaviour
{
    /// <summary>
    /// Достаточно маленький промежуток времени.
    /// </summary>
    private float _deltaTime;
    /// <summary>
    /// Главная камера.
    /// </summary>
    private Camera _mainCamera;
    /// <summary>
    /// Компонент, управляющий анимациями игрока.
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Пешая скорость.
    /// </summary>
    [SerializeField] private float _walkSpeed = 1f;
    /// <summary>
    /// Скорость бега.
    /// </summary>
    [SerializeField] private float _runspeed = 1.3f;
    /// <summary>
    /// Возвращает флаг, указывающий, движется ли игрок.
    /// </summary>
    public bool IsMoving { get; private set; }
    /// <summary>
    /// Возвращает флаг, указывающий, бежит ли игрок.
    /// </summary>
    public bool IsRunning { get; private set; }
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void Awake()
    {
        //Настройка полей.
        _mainCamera = Camera.main;
        _deltaTime = Time.deltaTime;
        _animator = this.transform.GetChild(0).GetComponent<Animator>(); //0-ым компонентом (ребёнком) должно быть визуально представление игрока.
        //Проверка полей.
        if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
        if (_animator == null) throw new ArgumentNullException("PlayerMotion: _animator is mull");
        if (_runspeed < 0) throw new ArgumentOutOfRangeException("PlayerMotion: _speedRun < 0");
        if (_walkSpeed < 0) throw new ArgumentOutOfRangeException("PlayerMotion: _speedWalk < 0");
    }
    private void Update()
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
        //Текущая скорость игрока в зависимости от состояния нажатия клавиши LeftShift
        float speedCurrent = Input.GetKey(KeyCode.LeftShift) ? _runspeed : _walkSpeed;
        //Отслеживание нажатия клавиш
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
        if (IsMoving) IsRunning = speedCurrent.Equals(_runspeed);
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
