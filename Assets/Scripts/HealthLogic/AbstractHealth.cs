using GunLogic;
using System.Collections;
using UnityEngine;

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
    public abstract void GetDamage(int valueDamage);
    public abstract void GetDamage(ProjectileData bullet);


    public abstract void GetDamage(ShrapnelGrenade grenade);

    public abstract void GetDamage(IBullet bullet);


    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
