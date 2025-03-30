using System;
using UnityEngine;

public class ShrapnelMine : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float damage = 10f;
    public bool isActivated = false;

    private Animator _animator;

    private void Explode()
    {
        Debug.Log("The ShrapnelMine is exploded");

        isActivated = true;
        this.GetComponent<SpriteRenderer>().sprite = null;

        _animator.SetTrigger("Explosion");

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var x in enemies) x.GetComponent<AbstractHealth>()?.GetDamage((int)damage);

        Destroy(this.gameObject, GetAnimationLength("ShrapnelMineExplosion"));
    }
    private float GetAnimationLength(string animationName)
    {
        foreach(var x in _animator.runtimeAnimatorController.animationClips)
            if (x.name.Equals(animationName))
                return x.length;

        return 0f;
    }


    private void Awake()
    {
        var _collider = this.GetComponent<Collider2D>();
        _animator = this.GetComponentInChildren<Animator>();

        if (_animator == null) throw new ArgumentNullException("ShrapnelMine: _animator is null");
        if (_collider == null) throw new ArgumentNullException("ShrapnelMine: _collider is null");
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isActivated && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))) Explode();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
