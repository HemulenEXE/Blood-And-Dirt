using UnityEngine;

public class DropByDrop : Skill
{
    private int _newBleedingDamage = 3;

    public DropByDrop()
    {
        Name = "DropByDrop";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point) // Вызывается один раз при активации навыка
    {
        PlayerData.BleedingDamage = _newBleedingDamage;
        if (PlayerData.HasSkill<IncreasedMetabolism>()) { /* Отмена дебафа */ }
    }
}
