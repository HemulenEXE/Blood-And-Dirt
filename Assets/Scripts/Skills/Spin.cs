using SkillLogic;
using UnityEngine;

public class Spin : Skill
{
    public Spin()
    {
        Name = "Spin";
        IsUnlocked = false;
        Type = SkillType.Added;
    }

    public override void Execute(GameObject point)
    {
        ++PlayerData.BandageCount;
        ++PlayerData.FirstAidKitCount;
    }
}
