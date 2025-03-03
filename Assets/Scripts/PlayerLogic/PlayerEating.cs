using SkillLogic;
using UnityEngine;

namespace PlayerLogic
{
    /// <summary>
    /// Скрипт, отвечающий за "поедание трупов"
    /// Навешивается на игрока
    /// </summary>
    public class PlayerEating : MonoBehaviour   
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerInfo.GetSkill<LiveInNotVain>()?.Execute(this.gameObject);
            }
        }
    }

}