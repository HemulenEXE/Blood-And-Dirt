using GunLogic;
using System;
using UnityEngine;

/// <summary>
/// Класс, реализующий "жизненный цикл снаряда".
/// </summary>
public class ShrapnelBullet : MonoBehaviour, IBullet
{
    public float Damage { get; set; }
    public GunType GunType { get; set; }
    public float Speed { get; set; } = 5f;

    private float _lifeTime = 5.5f;

    private void Start()
    {
        Destroy(this.gameObject, _lifeTime);
    }
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("Gun"))
        {
            Speed = 0; // Остановка снаряда
            this.GetComponent<SpriteRenderer>().sprite = null;

            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        this.transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }
}