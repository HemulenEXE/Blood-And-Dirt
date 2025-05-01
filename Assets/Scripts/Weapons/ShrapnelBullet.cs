using GunLogic;
using System;
using UnityEngine;

/// <summary>
/// Класс, реализующий "жизненный цикл снаряда".
/// </summary>
public class ShrapnelBullet : MonoBehaviour, IBullet
{
    public Side sideBullet { get; set; }
    public float Damage { get; set; }
    public GunType GunType { get; set; }
    public float Speed { get; set; } = 5f;

    private float _lifeTime = 5.5f;

    private Vector3 _previousPosition;
    private void Start()
    {
        Destroy(this.gameObject, _lifeTime);
        _previousPosition = transform.position;
    }
    private void FixedUpdate()
    {
        Vector3 newPosition = transform.position + transform.right * Speed * Time.fixedDeltaTime;

        // Проверяем наличие препятствий
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Speed * Time.fixedDeltaTime);
        if (hit.collider != null && !hit.collider.gameObject.CompareTag("Projectile") && !hit.collider.gameObject.CompareTag("Gun"))
        {
            Debug.Log("DESTROYED");
            Destroy(this.gameObject);
        }
        else
        {
            transform.position = newPosition;
        }

        Debug.DrawLine(_previousPosition, transform.position, Color.red);

        Debug.Log(hit.collider?.gameObject);

    }
}