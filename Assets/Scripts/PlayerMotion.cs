using System;
using UnityEngine;

/// <summary>
/// Класс передвижения игрока посредством клавиш WASD и компьютерной мыши
/// </summary>
public class PlayerMotion : MonoBehaviour
{
    /// <summary>
    /// Промежуток времени
    /// </summary>
    private static float deltaTime;
    /// <summary>
    /// Главная камера
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// Скорость передвижения игрока пешком
    /// </summary>
    [SerializeField] private float _speedWalk;
    public float SpeedWalk
    {
        get => _speedWalk;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException("PlayerMotion: SpeedWalk < 0");
            _speedWalk = value;
        }
    }
    /// <summary>
    /// Скорость передвижения игрока при беге
    /// </summary>
    [SerializeField] private float _speedRun;
    public float SpeedRun
    {
        get => _speedRun;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException("PlayerMotion: SpeedRun < 0");
            _speedRun = value;
        }
    }
    private void Awake()
    {
        //Настройка полей при запуске сцены
        mainCamera = Camera.main;
        deltaTime = Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    /// <summary>
    /// Передвижение игрока посредством клавиш WASD
    /// </summary>
    private void Move()
    {
        //Текущая скорость игрока в зависимости от состояния нажатия клавиши LeftShift
        float speedCurrent = Input.GetKey(KeyCode.LeftShift) ? SpeedRun : SpeedWalk;
        //Отслеживание нажатия клавиш
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speedCurrent * deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speedCurrent * deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * speedCurrent * deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * speedCurrent * deltaTime;
        }
    }
    /// <summary>
    /// Поворот игрока за компьтерной мышью
    /// </summary>
    private void Rotate()
    {
        //Вычисление положения компьютерной мыши в мировом пространстве
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        //Вычисление угла поворота игрока за мышью
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //Преобразование радиан в градусы - равен 360 / (2 * pi)
        this.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
