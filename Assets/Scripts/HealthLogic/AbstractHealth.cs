using System.Collections;
using UnityEngine;
using GunLogic;
using CameraLogic.CameraEffects;
using Grenades;

public abstract class AbstractHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;

    protected int currentHealth;

    protected bool isInvulnerable;

    public virtual IEnumerator InvulnerabilityFrames(float seconds)
    {
        isInvulnerable = true; // ������������� ���������� � true
        yield return new WaitForSeconds(seconds); // ��� ��������� ���������� ������
        isInvulnerable = false; // ���������� ���������� � false
    }
    protected abstract void GetDamage(int valueDamage);
    public abstract void GetDamage(ProjectileData bullet);


    public abstract void GetDamage(SimpleGrenade grenade);
    public abstract void GetDamage(ShrapnelGrenade grenade);

    public abstract void GetDamage(IBullet bullet);


    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
