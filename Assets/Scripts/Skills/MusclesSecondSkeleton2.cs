using SkillLogic;
using UnityEngine;

public class MusclesSecondSkeleton2 : Skill
{
    private int _newHitsToSurvive = 5;

    public MusclesSecondSkeleton2()
    {
        Name = "MusclesSecondSkeleton2";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point)
    {
        PlayerData.HitsToSurvive = _newHitsToSurvive;
        PlayerData.RunNoise *= 10;
    }
}
