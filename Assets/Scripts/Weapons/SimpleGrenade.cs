using CameraLogic.CameraEffects;
using System;
using UnityEngine;

public class SimpleGrenade : MonoBehaviour
{
    public float _timeToExplosion = 2f;
    public float _explosionRadius = 0.3f;
    public float _expolosionDuration;
    public float _damageExplosion = 3f;
    public bool IsActivated { get; set; } = false;

    protected Animator _animator;
    protected Camera _camera;
    protected GameObject _player;

    protected virtual void Awake()
    {
        _camera = Camera.main;
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = this.GetComponent<Animator>();

        if (_timeToExplosion < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: _timeToExplosion < 0");
        if (_explosionRadius < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: _explosionRadius < 0");
        if (_damageExplosion < 0) throw new ArgumentOutOfRangeException("SimpleGrenade: _damageExplosion < 0");
        if (_camera == null) throw new ArgumentNullException("SimpleGrenade: _camera is null");
        if (_player == null) throw new ArgumentNullException("SimpleGrenade: _player is null");
        if (_animator == null) throw new ArgumentNullException("SimpleGrenade: _animator is null");

        foreach (var x in _animator.runtimeAnimatorController.animationClips)
            if (x.name.Equals("GrenadeExplosion_Clip")) _expolosionDuration = x.length;
    }
    protected virtual void Update()
    {
        _timeToExplosion -= Time.deltaTime;
        if (_timeToExplosion <= 0 && !IsActivated) // Взрыв по окончанию таймера
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
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    public virtual void Explode()
    {
        IsActivated = true;
        this.GetComponent<SpriteRenderer>().sprite = null;
        // Прячем гранату от пользовательских глаз

        _animator.SetTrigger("Explosion");

        Collider2D[] entity_colliders = Physics2D.OverlapCircleAll(this.transform.position, _explosionRadius); //Получаем коллайдеры всех сущностей поблизости.
        foreach (var x in entity_colliders)
        {
            // Логика получения урона
        }
        Destroy(this.gameObject, _expolosionDuration);
    }
    protected virtual void Crash() // Тряска камеры во время взрыва
    {
        float distance = Vector3.Distance(_player.transform.position, transform.position);
        // Тряска тем больше, чем ближе к игроку упала граната
        if (distance <= 5) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.6f);
        else if (distance <= 10) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.3f);
        else _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.08f);
        // Что за магические числа?
    }
}