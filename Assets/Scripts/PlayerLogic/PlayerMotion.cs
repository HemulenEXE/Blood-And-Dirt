using GunLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    private Camera _mainCamera;
    private Animator _animator;

    private InventoryAndConsumableCounterUI _inventoryAndConsumableCounterUI;

    private Dictionary<float, float> noiseMapping;
    public static event Action<Transform, float> makeNoise;

    private void AnimationControl()
    {
        _animator.SetBool("IsMoving", PlayerData.IsWalking || PlayerData.IsRunning || PlayerData.IsStealing);
        var currentItem = _inventoryAndConsumableCounterUI.GetItem();
        if (currentItem?.GetComponent<ShotGun>() != null)
        {
            _animator.SetBool("ShotGun", true);
        }
        else _animator.SetBool("ShotGun", false);
        if (currentItem?.GetComponent<MachineGun>() != null)
        {
            _animator.SetBool("MachineGun", true);
        }
        else _animator.SetBool("MachineGun", false);
        if (currentItem?.GetComponent<Pistol>() != null)
        {
            _animator.SetBool("Pistol", true);
        }
        else _animator.SetBool("Pistol", false);
        if (currentItem?.GetComponent<Knife>() != null)
        {
            _animator.SetBool("Knife", true);
        }
        else _animator.SetBool("Knife", false);
    }

    private void Move()
    {
        PlayerData.IsRunning = Input.GetKey(KeyCode.LeftShift);
        PlayerData.IsStealing = Input.GetKey(KeyCode.LeftControl);

        PlayerData.GetSkill<Hatred>()?.Execute(this.gameObject); // ~ PlayerData.IsRunning = PlayerData.IsBleeding ? true : PlayerData.IsRunning

        float currentSpeed = PlayerData.IsStealing ? PlayerData.StealSpeed : (PlayerData.IsRunning ? PlayerData.RunSpeed : PlayerData.WalkSpeed);
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(SettingData.Left))
            movement += Vector3.left;
        if (Input.GetKey(SettingData.Right))
            movement += Vector3.right;
        if (Input.GetKey(SettingData.Up))
            movement += Vector3.up;
        if (Input.GetKey(SettingData.Down))
            movement += Vector3.down;

        if (movement != Vector3.zero)
        {
            PlayerData.IsWalking = !PlayerData.IsRunning && !PlayerData.IsStealing;
            this.transform.position += movement.normalized * currentSpeed * Time.fixedDeltaTime;
            if (PlayerData.IsStealing) makeNoise?.Invoke(this.transform, PlayerData.StealNoise);
            else if (PlayerData.IsRunning) makeNoise?.Invoke(this.transform, PlayerData.RunNoise);
            else if (PlayerData.IsWalking) makeNoise?.Invoke(this.transform, PlayerData.WalkNoise);
        }
        else PlayerData.IsWalking = false;
    }
    private void Rotate()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Преобразование радиан в градусы - равен 360 / (2 * pi)
        float rotationSpeed = 5f * SettingData.Sensitivity;
        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * angle);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _animator = this.GetComponentInChildren<Animator>();
        _inventoryAndConsumableCounterUI = GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>();

        if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
        if (_animator == null) throw new ArgumentNullException("PlayerMotion: _animator is null");
        if (_inventoryAndConsumableCounterUI == null) throw new ArgumentNullException("PlayerMotion: _inventoryAndConsumableCounterUI is null");
    }
    private void Update()
    {
        AnimationControl();
    }
    private void FixedUpdate()
    {
        Move(); // WASD
        Rotate(); // Мышь
    }
}