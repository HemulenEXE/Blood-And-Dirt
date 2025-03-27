using CameraLogic.CameraEffects;
using System;
using UnityEngine;

public class ShrapnelGrenade : MonoBehaviour
{
    public float timeToExplosion = 2f;
    public float explosionRadius = 0.3f;
    public float damageExplosion = 3f;
    public bool IsActivated { get; set; } = false;

    private Animator _animator;
    private Camera _camera;
    private GameObject _player;

    private float GetAnimationLength(string animationName)
    {
        foreach (var x in _animator.runtimeAnimatorController.animationClips)
            if (x.name.Equals(animationName)) return x.length;

        return 0f;
    }
    public virtual void Explode()
    {
        IsActivated = true;
        this.GetComponent<SpriteRenderer>().sprite = null;
        // Прячем гранату от пользовательских глаз

        _animator.SetTrigger("Explosion");

        Collider2D[] entity_colliders = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius); //Получаем коллайдеры всех сущностей поблизости.
        foreach (var x in entity_colliders)
        {
            var health = x.GetComponent<AbstractHealth>();
            if(health != null)
            {
                health.GetDamage(this);
            }
            
        }
        Destroy(this.gameObject, GetAnimationLength("ShrapnelGrenadeExplosion"));
    }
    protected virtual void Crash() // Тряска камеры во время взрыва
    {
        float distance = Vector3.Distance(_player.transform.position, transform.position); // Тряска тем больше, чем ближе к игроку упала граната
        if (distance <= 5) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.6f);
        else if (distance <= 10) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.3f);
        else _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.08f);
        // Что за магические числа?
    }

    protected virtual void Awake()
    {
        _camera = Camera.main;
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = this.GetComponent<Animator>();

        if (timeToExplosion < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: timeToExplosion < 0");
        if (explosionRadius < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: explosionRadius < 0");
        if (damageExplosion < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: damageExplosion < 0");
        if (_camera == null) throw new ArgumentNullException("SimpleGrenade: _camera is null");
        if (_player == null) throw new ArgumentNullException("SimpleGrenade: _player is null");
        if (_animator == null) throw new ArgumentNullException("SimpleGrenade: _animator is null");
    }
    protected virtual void Update()
    {
        timeToExplosion -= Time.deltaTime;

        if (timeToExplosion <= 0 && !IsActivated) // Взрыв по окончанию таймера
        {
            Explode();
            Crash();
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null && !other.gameObject.CompareTag("Player")) // Взрыв при соприкосновении с любым объектом, за исключением игрока
        {
            Debug.Log(other.gameObject.name);
            Explode();
            Crash();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}