using System.Collections;
using UnityEngine;

public abstract class AbstractHealth : MonoBehaviour
{
    [SerializeField]
    public int maxHealth;

    protected int currentHealth;

    protected bool isInvulnerable;

    public virtual IEnumerator InvulnerabilityFrames(float seconds)
    {
        isInvulnerable = true; // ������������� ���������� � true
        yield return new WaitForSeconds(seconds); // ��� ��������� ���������� ������
        isInvulnerable = false; // ���������� ���������� � false
    }

    public abstract void GetDamage(ProjectileData bullet);

    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
