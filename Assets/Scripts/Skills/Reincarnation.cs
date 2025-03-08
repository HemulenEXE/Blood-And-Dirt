using UnityEngine;
using SkillLogic;

public class Reincarnation : Skill
{
    private GameObject _body;
    private AudioClip _audio;
    private int _newBodyCount = 3;

    public Reincarnation()
    {
        Name = "Reincarnation";
        IsUnlocked = false;
        _body = Resources.Load<GameObject>("Prefabs/Ghost");
        _audio = Resources.Load<AudioClip>("Audios/Reinc");
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point)
    {
        PlayerData.ResurrectionCount = _newBodyCount;
    }
    public virtual void SpawnBody(GameObject point)
    {
        if (PlayerData.ResurrectionCount > 0)
        {
            point.GetComponent<AudioSource>().PlayOneShot(_audio);
            GameObject.Instantiate(_body, point.transform.position, Quaternion.identity);
            --PlayerData.ResurrectionCount;
        }
    }
}

