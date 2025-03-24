using System;
using UnityEngine;

public class ExplosionBullet : MonoBehaviour, IBullet
{
    public float Damage { get; set; }
    public GunType GunType { get; set; }
    public float Speed { get; set; } = 1f;

    private float _lifeTime = 5.5f;
    private Animator _animationController;

    private float GetAnimationLength(string animationName)
    {
        foreach (var x in _animationController.runtimeAnimatorController.animationClips)
            if (x.name.Equals(animationName)) return x.length;

        return 0f;
    }

    private void Awake()
    {
        _animationController = this.GetComponentInChildren<Animator>();

        if (_animationController == null) throw new ArgumentNullException("ExplosionBullet: _animationController is null");
    }

    private void Start()
    {
        Destroy(this.gameObject, _lifeTime);
    }
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("Gun"))
        {
            Debug.Log(other.gameObject.name);

            Speed = 0; // Остановка снаряда
            this.GetComponent<SpriteRenderer>().sprite = null;

            _animationController.SetTrigger("Explosion");

            Destroy(this.gameObject, GetAnimationLength("ExplosionBullet"));
        }
    }
    private void Update()
    {
        this.transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }
}
