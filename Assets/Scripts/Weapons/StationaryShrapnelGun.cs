using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StationaryShrapnelGun : ClickedObject
{
    [field: SerializeField] public int AmmoTotal { get; set; } = 100;
    [field: SerializeField] public float FireDelay { get; set; } = 0.2f;

    public bool IsFiring { get; set; } = false;
    public float BulletSpeed { get; set; } = 10f;

    private GameObject _bulletPrefab;
    private GameObject _player;
    private bool IsInStationaryGun { get; set; } = false;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _bulletPrefab = Resources.Load<GameObject>("Prefabs/Weapons/ShrapnelBullet");

        if (_bulletPrefab == null) throw new ArgumentNullException("StationaryShrapnelGun: _bulletPrefab is null");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && IsInStationaryGun && !IsFiring)
        {
            StartCoroutine(Fire());
        }
        if (IsInStationaryGun) Rotate();
        if (IsInStationaryGun && Input.GetKey(KeyCode.Q))
        {
            IsInStationaryGun = false;
            _player.SetActive(true);
        }
    }
    private bool Shoot()
    {
        if (AmmoTotal > 0)
        {
            IsFiring = true;
            GameObject bullet = Instantiate(_bulletPrefab, this.transform.Find("SpawnerProjectile").position, this.transform.Find("SpawnerProjectile").rotation);
            bullet.GetComponent<ShrapnelBullet>().Speed = BulletSpeed;
            --AmmoTotal;
            return true;
        }
        return false;
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
        Debug.Log("S");
        IsInStationaryGun = true;
        _player = GameObject.FindGameObjectWithTag("Player").gameObject;
        _player.SetActive(false);
    }
    private void Rotate()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ѕреобразование радиан в градусы - равен 360 / (2 * pi)
        float rotationSpeed = 5f * SettingData.Sensitivity;
        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * angle);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
