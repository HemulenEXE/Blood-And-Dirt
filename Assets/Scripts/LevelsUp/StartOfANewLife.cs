using SkillLogic;
using UnityEngine;

public class StartOfANewLife : Skill
{
    [SerializeField]
    private int _newFullHealth = 200;
    [SerializeField]
    private int _newDamageBleeding = 2;

    public StartOfANewLife()
    {
        Name = "StartOfANewLife";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point)
    {
        PlayerData.MaxHealth = _newFullHealth;
        PlayerData.BleedingDamage = _newDamageBleeding;
    }
}
