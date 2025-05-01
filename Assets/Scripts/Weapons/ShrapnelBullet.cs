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
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("Gun"))
        {
            Debug.Log("DESTROYED");
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
        this.transform.position += this.transform.right * Speed * Time.fixedDeltaTime;

        Debug.DrawLine(_previousPosition, transform.position, Color.red);

    }
}