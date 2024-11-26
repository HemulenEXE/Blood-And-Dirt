using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gun;

public abstract class AbstractHealth : MonoBehaviour
{
    [SerializeField]
    public int maxHealth;

    protected int currentHealth;

    protected bool isInvulnerable;

    public virtual IEnumerator InvulnerabilityFrames(float seconds)
    {
        isInvulnerable = true; // Устанавливаем переменную в true
        yield return new WaitForSeconds(seconds); // Ждём указанное количество секунд
        isInvulnerable = false; // Возвращаем переменную в false
    }

    public abstract void GetDamge(ProjectileData bullet); 

    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
