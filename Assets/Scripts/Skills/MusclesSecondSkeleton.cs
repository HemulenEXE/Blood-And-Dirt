using UnityEngine;

public class MusclesSecondSkeleton : Skill
{
    private int _newHitsToSurvive = 3;
    private float _newStealNoise = 1f;
    private float _newWalkNoise = 4f;
    private float _newRunNoise = 7f;


    public MusclesSecondSkeleton()
    {
        Name = "MusclesSecondSkeleton";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point) // Вызывается один раз при активации навыка
    {
        PlayerData.HitsToSurvive = _newHitsToSurvive;
        PlayerData.CurrentHitsToSurvive = _newHitsToSurvive;
        PlayerData.StealNoise = _newStealNoise;
        PlayerData.WalkNoise = _newWalkNoise;
        PlayerData.RunNoise = _newRunNoise;
    }
}

