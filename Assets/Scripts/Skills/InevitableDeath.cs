using UnityEngine;

public class InevitableDeath : Skill
{
    private int _limitHealth = 20;
    private float _baseRadius = 0.2f;
    private AudioClip _audio;

    public InevitableDeath()
    {
        Name = "InevitableDeath";
        IsUnlocked = false;
        _audio = Resources.Load<AudioClip>("Audios/explosion_sound");
    }

    public override void Execute(GameObject point)
    {
        if (PlayerData.CurrentHealth < _limitHealth && (PlayerData.SmokeGrenadeCount > 0 || PlayerData.SimpleGrenadeCount>0))
        {
            float radius = _baseRadius * (PlayerData.SimpleGrenadeCount + PlayerData.SmokeGrenadeCount); // Радиус взрыва зависит от количества гранат в инвентаре
            PlayerData.SmokeGrenadeCount = 0;
            PlayerData.SimpleGrenadeCount = 0;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(point.transform.position, radius);

            foreach (var x in enemies)
            {
                if (x.CompareTag("Enemy")) x.GetComponent<HealthBot>().Death();
            }
            point.GetComponent<AudioSource>().PlayOneShot(_audio);
        }
    }
}
