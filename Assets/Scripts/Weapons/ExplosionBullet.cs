using System;
using UnityEngine;

public class ExplosionBullet : MonoBehaviour, IBullet
{
    public Side sideBullet { get; set; }
    public float Damage { get; set; }
    public GunType GunType { get; set; }
    public float Speed { get; set; } = 1f;
    public float ExplosionRadius { get; set; } = 2f;

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

            Collider2D[] enemies = Physics2D.OverlapCircleAll(this.transform.position, ExplosionRadius);
            foreach (var x in enemies)
            {
                if (!x.gameObject.CompareTag("Player")) x.GetComponent<AbstractHealth>()?.GetDamage(this);
            }

            Destroy(this.gameObject, GetAnimationLength("ExplosionBullet"));
        }
    }
    private void Update()
    {
        this.transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }
}
