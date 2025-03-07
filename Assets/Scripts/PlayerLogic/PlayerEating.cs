using SkillLogic;
using UnityEngine;

public class PlayerEating : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerData.GetSkill<LiveInNotVain>()?.Execute(this.gameObject);
        }
    }
}