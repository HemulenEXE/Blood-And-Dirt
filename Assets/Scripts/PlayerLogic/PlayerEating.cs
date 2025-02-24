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
                Debug.Log("F");
                PlayerInfo.ExecuteSkill("LiveInNotVain", this.gameObject);
            }
        }
    }

}