using CameraLogic.CameraEffects;
using GunLogic;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс здоровья игрока. Скрипт навешивается на игрока
/// </summary>
public class PlayerHealth : AbstractHealth
{

    private float _frameDuration = 0.5f; // Сколько длится кадр, во время которого игрок не получает урон

    private BloodEffect bloodController;

    public static event Action<Transform, string> AudioEvent;

    public override void GetDamage(int value)
    {
        if (PlayerData.IsGod || isInvulnerable) return;

        AudioEvent?.Invoke(this.transform, "taking_damage" + UnityEngine.Random.Range(0, 11));

        PlayerData.IsBleeding = true;
        PlayerData.CurrentHealth -= value;

        PlayerData.GetSkill<IncreasedMetabolism>()?.Execute(this.gameObject);
        PlayerData.GetSkill<IncreasedMetabolism>()?.RebootTimer();

        if (PlayerData.CurrentHealth <= 0) HandleDeath();

        StartCoroutine(InvulnerabilityFrames(_frameDuration));
    }
    public override void GetDamage(IBullet bullet)
    {
        GetDamage((int)bullet.Damage);
    }
    public override void GetDamage(ProjectileData bullet)
    {
        GetDamage((int)bullet.Damage);
    }


    public override void GetDamage(ShrapnelGrenade granade)
    {
        GetDamage((int)granade.damageExplosion);
    }
    private void HandleDeath()
    {
        var temp = PlayerData.GetSkill<Reincarnation>();
        if (temp != null && PlayerData.CurrentResurrectionCount > 0)
        {
            AudioEvent?.Invoke(this.transform, "Reincarnation");
            temp.SpawnBody(this.gameObject); // Спавн трупа
            return;
        }
        else Death();
    }

    private IEnumerator BleedDamage()
    {
        while (true)
        {
            if (PlayerData.IsBleeding && !PlayerData.IsGod)
                PlayerData.CurrentHealth -= PlayerData.BleedingDamage;
            yield return new WaitForSeconds(60f); // Что за магическое число?
        }
    }
    /// <summary>
    /// Высчитывает текущее состояние эффекта крови
    /// </summary>
    /// <returns></returns>
    private StateBloodEffect CalculateStateDamaged()
    {
        return (StateBloodEffect)Math.Floor((PlayerData.MaxHealth - PlayerData.CurrentHealth) / (PlayerData.MaxHealth / 5.0));
    }

    public void Awake()
    {
        bloodController = GetComponent<BloodEffect>();
        StartCoroutine(BleedDamage());
    }

    private void Update()
    {
        if (Input.GetKeyDown(SettingData.FirstAidKit) && PlayerData.FirstAidKitCount > 0)
        {
            if (PlayerData.CurrentHealth + PlayerData.FirstAidKitHealth > PlayerData.MaxHealth)
                PlayerData.CurrentHealth = PlayerData.MaxHealth;
            else PlayerData.CurrentHealth += PlayerData.FirstAidKitHealth;

            --PlayerData.FirstAidKitCount;
            AudioEvent?.Invoke(this.transform, "fistaidkit");
        }

        if (Input.GetKeyDown(SettingData.Bandage) && PlayerData.BandageCount > 0)
        {
            if (PlayerData.CurrentHealth + PlayerData.BandageHealth > PlayerData.MaxHealth)
                PlayerData.CurrentHealth = PlayerData.MaxHealth;
            else PlayerData.CurrentHealth += PlayerData.BandageHealth;

            --PlayerData.BandageCount;
            AudioEvent?.Invoke(this.transform, "bint");
        }

        PlayerData.GetSkill<InevitableDeath>()?.Execute(this.gameObject);
    }
    private void FixedUpdate()
    {
        bloodController.SetBloodEffect(CalculateStateDamaged());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            var dataBullet = collision.gameObject.GetComponent<IBullet>();
            GetDamage(dataBullet);
            PlayAnimationHit(collision.gameObject.transform);
        }
    }

    public override void PlayAnimationHit(Transform sourceDamage)
    {
        string namePrefab = "Prefabs/HitAnimation/blood" + (UnityEngine.Random.Range(0, 5) + 1);
        Debug.Log(namePrefab);
        GameObject prefamHitAnimation = Resources.Load<GameObject>(namePrefab);
        float offset = 0.1f;
        // 1. Получаем точку входа и направление "пули"
        Vector3 entryPoint = sourceDamage.position;
        Vector3 direction = (transform.position - sourceDamage.position).normalized;

        // 2. Определяем центр объекта
        var bounds = GetComponent<BoxCollider2D>().bounds;
        Vector3 center = bounds.center;

        // 3. Вектор от центра к точке входа (направление попадания внутрь)
        Vector3 localHitDirection = (entryPoint - center).normalized;

        // 4. Вычисляем точку выхода (противоположная сторона от центра)
        Vector3 exitPoint = center - localHitDirection * bounds.extents.magnitude;

        // 5. Смещаем немного от поверхности наружу
        exitPoint += localHitDirection * offset;

        float angle = Mathf.Atan2(localHitDirection.y, localHitDirection.x) * Mathf.Rad2Deg;

        // Создаём вращение вокруг Z
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // 6. Создаём эффект выхода
        GameObject effect = Instantiate(prefamHitAnimation, exitPoint, rotation);

        // 7. Воспроизводим анимацию (если есть Animator)
        Animator anim = effect.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Play");
            // Удалим через длительность клипа, если нужна автоматическая очистка
            Destroy(effect, anim.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        }
        else
        {
            // Удалим через фиксированное время, если Animator не найден
            Destroy(effect, 2f);
        }
    }
}
