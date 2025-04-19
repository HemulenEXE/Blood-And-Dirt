using SkillLogic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    private void Update()
    {
        var temp = PlayerData.GetSkill<Sound>();
        temp?.Execute(this.gameObject);
    }
}
