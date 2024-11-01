using System;
using UnityEngine;

/// <summary>
/// ����� ������������ ������ ����������� ������ WASD � ������������ ����
/// </summary>
public class PlayerMotion : MonoBehaviour
{
    /// <summary>
    /// ���������� �������
    /// </summary>
    private static float deltaTime;
    /// <summary>
    /// ������� ������
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// �������� ������������ ������ ������
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
    /// �������� ������������ ������ ��� ����
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
        //��������� ����� ��� ������� �����
        mainCamera = Camera.main;
        deltaTime = Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    /// <summary>
    /// ������������ ������ ����������� ������ WASD
    /// </summary>
    private void Move()
    {
        //������� �������� ������ � ����������� �� ��������� ������� ������� LeftShift
        float speedCurrent = Input.GetKey(KeyCode.LeftShift) ? SpeedRun : SpeedWalk;
        //������������ ������� ������
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
    /// ������� ������ �� ����������� �����
    /// </summary>
    private void Rotate()
    {
        //���������� ��������� ������������ ���� � ������� ������������
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        //���������� ���� �������� ������ �� �����
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //�������������� ������ � ������� - ����� 360 / (2 * pi)
        this.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
