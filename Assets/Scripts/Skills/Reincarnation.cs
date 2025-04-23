using UnityEngine;

public class Reincarnation : Skill
{
    private GameObject _body;
    private int _newBodyCount = 3;



    public Reincarnation()
    {
        Name = "Reincarnation";
        IsUnlocked = false;
        _body = Resources.Load<GameObject>("Prefabs/PlayerBody");
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point) // Вызывается единственный раз при активации навыка
    {
        PlayerData.ResurrectionCount = _newBodyCount;
        PlayerData.CurrentResurrectionCount = _newBodyCount;
    }
    public virtual void SpawnBody(GameObject point)
    {
        if (PlayerData.CurrentResurrectionCount > 0)
        {
            GameObject.Instantiate(_body, point.transform.position, Quaternion.identity);
            Debug.Log(_body is null);
            --PlayerData.CurrentResurrectionCount;

            PlayerData.MaxHealth /= 2;
            PlayerData.CurrentHealth = PlayerData.MaxHealth;
        }
    }
}

