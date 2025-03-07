using InventoryLogic;
using UnityEngine;
using SkillLogic;
public class SledGrenade : Skill
{
    public SledGrenade()
    {
        Name = "SledGrenade";
        IsUnlocked = false;
        Type = SkillType.Added;
    }
    public override void Execute(GameObject point)
    {
        ConsumableCounter._simpleGrenadeCount += 2;
        ConsumableCounter._smokeGrenadeCount += 2;
    }
}
