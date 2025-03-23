using System;
using UnityEngine;

public class SmokeGrenade : SimpleGrenade
{
    [SerializeField] private ParticleSystem _smokeParticle;
    [SerializeField] protected float _smokeDuraion = 5f;

    protected override void Awake()
    {
        base.Awake();
        _smokeParticle = this.GetComponentInChildren<ParticleSystem>();
        if (_smokeParticle == null) throw new ArgumentNullException("SmokeGrenade: _smokeParticle is null");
        if (_smokeDuraion < 0) throw new ArgumentOutOfRangeException("SmokeGrenade: _smokeDuraion < 0");
    }
    private void OnDestroy()
    {
        _smokeParticle.Stop();
    }

    public override void Explode()
    {
        _smokeParticle.Play();
        this.GetComponent<SpriteRenderer>().sprite = null;
        IsActivated = true;

        _animator.SetTrigger("Explosion");

        CircleCollider2D smokeField = this.GetComponent<CircleCollider2D>(); // Установка коллайдера, чтобы враги путались в дыме.
        smokeField.radius = _explosionRadius;
        // Враги не получают урон в дыму
        Destroy(this.gameObject, _smokeDuraion);
    }
}