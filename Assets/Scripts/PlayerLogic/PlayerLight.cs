using SkillLogic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    private float interval = 0.5f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            var temp = PlayerData.GetSkill<Sound>();
            temp?.Execute(this.gameObject);
            timer = 0f;
        }
    }


}
