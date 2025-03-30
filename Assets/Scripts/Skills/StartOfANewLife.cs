using UnityEngine;

public class StartOfANewLife : Skill
{
    private int _newFullHealth = 200;
    private int _newDamageBleeding = 2;

    public StartOfANewLife()
    {
        Name = "StartOfANewLife";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point)
    {
        PlayerData.CurrentHealth += (_newFullHealth - PlayerData.MaxHealth);
        PlayerData.MaxHealth = _newFullHealth;
        PlayerData.BleedingDamage = _newDamageBleeding;
    }
}
