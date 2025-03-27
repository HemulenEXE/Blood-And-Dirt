using UnityEngine;

public class MusclesSecondSkeleton2 : Skill
{
    private int _newHitsToSurvive = 5;
    private float _newStealNoise = 2f;
    private float _newWalkNoise = 7f;
    private float _newRunNoise = 10f;

    public MusclesSecondSkeleton2()
    {
        Name = "MusclesSecondSkeleton2";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point) // Вызывается один раз при активации навыка
    {
        PlayerData.HitsToSurvive = _newHitsToSurvive;
        PlayerData.StealNoise = _newStealNoise;
        PlayerData.WalkNoise = _newWalkNoise;
        PlayerData.RunNoise = _newRunNoise;
    }
}
