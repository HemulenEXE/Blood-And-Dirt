using UnityEngine;

public class ShrapnelBullet : MonoBehaviour, IBullet
{
    public Side sideBullet { get; set; }
    public float Damage { get; set; }
    public GunType GunType { get; set; }
    public float Speed { get; set; } = 5f;

    private float _lifeTime = 5.5f;
    private Rigidbody2D _rigidbody;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = this.transform.position;
        _rigidbody = this.GetComponent<Rigidbody2D>();
        _rigidbody.velocity = this.transform.right * Speed;
        Destroy(this.gameObject, _lifeTime);
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
        Debug.DrawLine(_startPosition, this.transform.position, Color.red);
    }
}