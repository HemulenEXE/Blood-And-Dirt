using UnityEngine;

public class AnyPrice : Skill
{
    public AnyPrice()
    {
        Name = "AnyPrice";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }
    public override void Execute(GameObject point)
    {
        PlayerData.RunNoise /= 2;
        PlayerData.WalkNoise /= 2;
    }
}