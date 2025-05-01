using UnityEngine;

public class ShrapnelBullet : MonoBehaviour, IBullet
{
    public Side sideBullet { get; set; }
    public float Damage { get; set; }
    public GunType GunType { get; set; }
    public float Speed { get; set; } = 5f;

    private float _lifeTime = 5.5f;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        Destroy(this.gameObject, _lifeTime);
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.right * Speed;
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("Mine") && !other.gameObject.CompareTag("Gun"))
        {
            Debug.Log("DESTROYED");
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position - transform.right, transform.position, Color.red);
    }
}