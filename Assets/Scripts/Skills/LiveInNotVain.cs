using UnityEngine;

public class LiveInNotVain : Skill
{
    public LiveInNotVain()
    {
        Name = "LiveInNotVain";
        IsUnlocked = false;
        Type = SkillType.Added;
    }

    public override void Execute(GameObject point) { /* Не содержит реализацию - за поедание трупов отвечает класс Body */}
}
