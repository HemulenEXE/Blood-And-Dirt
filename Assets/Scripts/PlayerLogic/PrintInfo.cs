using UnityEngine;

namespace PlayerLogic
{
    public class PrintInfo : MonoBehaviour
    {
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log($"Count of skills: {PlayerInfo._skills.Count}\n" +
                    $"Steal speed: {PlayerInfo._stealSpeed}\n" +
                    $"Walk speed: {PlayerInfo._walkSpeed}\n" +
                    $"Run speed: {PlayerInfo._runSpeed}\n" +
                    $"Full Health: {PlayerInfo._fullHealth}\n" +
                    $"Current Health: {PlayerInfo._currentHealth}\n" +
                    $"Damage of bleeding: {PlayerInfo._bleedingDamage}\n" +
                    $"Hits to survive: {PlayerInfo._hitsToSurvive}");
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                this.GetComponent<PlayerHealth>().Death();
            }
        }
    }

}

