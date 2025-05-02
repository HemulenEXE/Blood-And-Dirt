using System;
using System.Collections;
using UnityEngine;

public class StationaryShrapnelGun : ClickedObject
{
    [field: SerializeField] public int AmmoTotal { get; set; } = 100;
    [field: SerializeField] public float FireDelay { get; set; } = 0.2f;
    [field: SerializeField] public float Damage { get; set; } = 1f;
    [field: SerializeField] public float ProjectileSpeed { get; set; } = 20f;
    public GunType Type { get; } = GunType.Heavy;

    public bool IsFiring { get; set; } = false;
    public float BulletSpeed { get; set; } = 10f;

    private GameObject _bulletPrefab;
    private GameObject _player;
    private bool IsInStationaryGun { get; set; } = false;
    private Camera _mainCamera;
    private Rigidbody2D _rigidBody;
    public float OffSet = 0.1f;

    public static event Action<Transform, string> AudioEvent;
    private Side _sideplayer;

    private GameObject _interactUI;
    private GameObject _inventUI;
    private GameObject _dW;


    private bool Shoot()
    {
        if (AmmoTotal > 0)
        {
            AudioEvent?.Invoke(this.transform, "stationary_choper_fire_audio");
            IsFiring = true;
            var bullet = Instantiate(_bulletPrefab, this.transform.Find("SpawnerProjectile").position, this.transform.Find("SpawnerProjectile").rotation).GetComponent<IBullet>();
            bullet.Speed = BulletSpeed;
            bullet.sideBullet = _sideplayer.CreateSideBullet();
            bullet.Damage = this.Damage;
            bullet.GunType = this.Type;
            bullet.Speed = this.ProjectileSpeed;
            --AmmoTotal;
            return true;
        }
        return false;
    }
    private void Exit()
    {
        IsInStationaryGun = false;

        _player.transform.SetParent(null);
        PlayerData.IsMotionless = false;

        _rigidBody.simulated = true;

        _interactUI?.SetActive(true);
        _inventUI?.SetActive(true);

        Debug.Log("StationaryShrapnelGun isn't used");
    }
    private IEnumerator Fire()
    {
        if (Shoot())
        {
            yield return new WaitForSeconds(FireDelay);
            IsFiring = false;
        }
    }
    public override void Interact()
    {
        IsInStationaryGun = true;
        _player.transform.SetParent(this.transform);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.bounds.size;

        _player.transform.localPosition = new Vector3(spriteSize.x / 2 + OffSet, 0, 0);
        _player.transform.rotation = this.transform.rotation * Quaternion.Euler(0, 0, 180);
        _rigidBody.simulated = false;
        PlayerData.IsMotionless = true;

        _interactUI = GameObject.Find("InteractiveUI");
        _inventUI = GameObject.Find("InventoryAndConsumableCounterUI");
        _dW = GameObject.Find("DialogueWindow");
        _interactUI?.SetActive(false);
        _inventUI?.SetActive(false);
        if (_dW != null && _dW.GetComponent<DialogueWndState>().currentState == DialogueWndState.WindowState.StartPrint)
            _dW.SetActive(false);

        Debug.Log("StationaryShrapnelGun is used");
    }
    private void Rotate()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ѕреобразование радиан в градусы - равен 360 / (2 * pi)
        float rotationSpeed = 5f * SettingData.Sensitivity;
        angle += 180;
        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * angle);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    private void Awake()
    {
        _mainCamera = Camera.main;
        _bulletPrefab = Resources.Load<GameObject>("Prefabs/Weapons/ShrapnelBullet");

        if (_bulletPrefab == null) throw new ArgumentNullException("StationaryShrapnelGun: _bulletPrefab is null");

        _player = GameObject.FindGameObjectWithTag("Player").gameObject;
        _sideplayer = _player.GetComponent<Side>();

        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && IsInStationaryGun && !IsFiring) StartCoroutine(Fire());

        if (IsInStationaryGun && Input.GetKey(KeyCode.Q)) Exit();

        if (IsInStationaryGun)
        {
            Rotate();
        }
    }
}
