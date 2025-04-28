using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMotion : MonoBehaviour
{
    private Camera _mainCamera;
    private Animator _animator;

    private InventoryAndConsumableCounterUI _inventoryAndConsumableCounterUI;

    private Dictionary<float, float> noiseMapping;
    public static event Action<Transform, float> makeNoise;
    public bool IsInStationaryGun { get; set; } = false;

    private string _currentSurface;

    public static event Action<Transform, string> AudioEvent;

    private Tilemap[] _tilemaps;

    private Coroutine _audioCoroutine;

    private void AnimationControl()
    {
        _animator.SetBool("IsMoving", !PlayerData.IsMotionless && (PlayerData.IsWalking || PlayerData.IsRunning || PlayerData.IsStealing));
        var currentItem = _inventoryAndConsumableCounterUI?.GetItem();
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
        if (currentItem?.GetComponent<GrenadeLauncher>() != null)
        {
            _animator.SetBool("GrenadeLauncher", true);
        }
        else _animator.SetBool("GrenadeLauncher", false);
    }

    public void EnterInStationaryGun()
    {
        IsInStationaryGun = true;
    }
    public void ExitInStationaryGun()
    {
        IsInStationaryGun = false;
    }

    private void Move()
    {
        if (PlayerData.IsMotionless)
        {
            PlayerData.IsWalking = false;
            PlayerData.IsRunning = false;
            PlayerData.IsStealing = false;
            return;
        }

        PlayerData.GetSkill<Hatred>()?.Execute(this.gameObject); // ~ PlayerData.IsRunning = PlayerData.IsBleeding ? true : PlayerData.IsRunning

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(SettingData.Left))
            movement += Vector3.left;
        if (Input.GetKey(SettingData.Right))
            movement += Vector3.right;
        if (Input.GetKey(SettingData.Up))
            movement += Vector3.up;
        if (Input.GetKey(SettingData.Down))
            movement += Vector3.down;

        PlayerData.IsRunning = Input.GetKey(KeyCode.LeftShift);
        PlayerData.IsStealing = Input.GetKey(KeyCode.LeftControl);
        PlayerData.IsWalking = !PlayerData.IsRunning && !PlayerData.IsStealing;

        if (movement != Vector3.zero)
        {

            float currentSpeed = PlayerData.IsStealing ? PlayerData.StealSpeed : (PlayerData.IsRunning ? PlayerData.RunSpeed : PlayerData.WalkSpeed);

            this.transform.position += movement.normalized * currentSpeed * Time.fixedDeltaTime;

            if (_currentSurface != "None" && _audioCoroutine == null)
            {
                _audioCoroutine = StartCoroutine(PlaySoundWithDelay(_currentSurface + "_moving"));
            }

            if (PlayerData.IsStealing) makeNoise?.Invoke(this.transform, PlayerData.StealNoise);
            else if (PlayerData.IsRunning) makeNoise?.Invoke(this.transform, PlayerData.RunNoise);
            else if (PlayerData.IsWalking) makeNoise?.Invoke(this.transform, PlayerData.WalkNoise);
        }
        else PlayerData.IsWalking = false;
    }
    private void Rotate()
    {
        if (PlayerData.IsMotionless)
        {
            return;
        }
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Преобразование радиан в градусы - равен 360 / (2 * pi)
        float rotationSpeed = 5f * SettingData.Sensitivity;
        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * angle);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    public void CheckSurface()
    {
        Vector3Int playerPosition = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);

        foreach (var tilemap in _tilemaps)
        {
            if (tilemap.HasTile(playerPosition))
            {
                _currentSurface = tilemap.name;
                return;
            }
        }
        _currentSurface = "None";
    }
    private IEnumerator PlaySoundWithDelay(string soundName)
    {
        AudioEvent?.Invoke(this.transform, soundName);
        Debug.Log(soundName);
        yield return new WaitForSeconds(0.35f);
        _audioCoroutine = null;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _animator = this.GetComponentInChildren<Animator>();
        _inventoryAndConsumableCounterUI = GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>();
        _tilemaps = GameObject.FindObjectsOfType<Tilemap>();

        if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
        if (_animator == null) throw new ArgumentNullException("PlayerMotion: _animator is null");
    }
    private void Update()
    {
        AnimationControl();
        CheckSurface();
    }
    private void FixedUpdate()
    {
        Move(); // WASD
        Rotate(); // Мышь
    }
}