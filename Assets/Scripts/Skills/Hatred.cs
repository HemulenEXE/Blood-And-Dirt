using UnityEngine;

public class Hatred : Skill
{
    private float _newRunSpeed = 4.5f;

    public Hatred()
    {
        Name = "Hatred";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point) // Вызывается в PlayerMotion
    {
        PlayerData.RunSpeed = _newRunSpeed;
        if (PlayerData.IsBleeding)
        {
            PlayerData.IsRunning = true;
            //var bodies = GameObject.FindGameObjectsWithTag("Body");
            // Уменьшение целостности трупов - можно вызвать отдельно метод
        }
    }
}