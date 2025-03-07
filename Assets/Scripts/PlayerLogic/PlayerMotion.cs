using GunLogic;
using InventoryLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    private Camera _mainCamera = Camera.main;
    private Animator _animator;

    private Dictionary<float, float> noiseMapping;
    public static event Action<Transform, float> makeNoise;

    private void AnimationControl()
    {
        _animator.SetBool("IsMoving", PlayerData.IsWalking);
        var currentItem = Inventory.GetInstance?.CurrentSlot?.StoredItem;
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
        PlayerData.IsWalking = !PlayerData.IsRunning && !PlayerData.IsStealing;

        // PlayerData.GetSkill<Hatred>()?.Execute(this.gameObject);

        float currentSpeed = PlayerData.IsStealing ? PlayerData.StealSpeed : (PlayerData.IsRunning ? PlayerData.RunSpeed : PlayerData.WalkSpeed);
        Vector3 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
            movement += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            movement += Vector3.right;
        if (Input.GetKey(KeyCode.W))
            movement += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            movement += Vector3.down;

        if (movement != Vector3.zero)
        {
            this.transform.position += movement.normalized * currentSpeed * Time.fixedDeltaTime;
            if (PlayerData.IsStealing) makeNoise?.Invoke(this.transform, PlayerData.StealNoise);
            else if (PlayerData.IsRunning) makeNoise?.Invoke(this.transform, PlayerData.RunNoise);
            else if (PlayerData.IsWalking) makeNoise?.Invoke(this.transform, PlayerData.WalkNoise);
        }
    }
    private void Rotate()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Преобразование радиан в градусы - равен 360 / (2 * pi)
        this.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    private void Awake()
    {
        if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
        if (_animator == null) throw new ArgumentNullException("PlayerMotion: _animator is null");
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