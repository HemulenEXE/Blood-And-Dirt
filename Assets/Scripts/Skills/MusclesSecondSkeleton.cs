using SkillLogic;
using UnityEngine;

public class MusclesSecondSkeleton : Skill
{
    private int _newHitsToSurvive = 3;

    public MusclesSecondSkeleton()
    {
        Name = "MusclesSecondSkeleton";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point)
    {
        PlayerData.HitsToSurvive = _newHitsToSurvive;
        PlayerData.WalkNoise *= 2;
    }
}

