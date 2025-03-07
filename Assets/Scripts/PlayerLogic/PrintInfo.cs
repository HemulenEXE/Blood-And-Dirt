using UnityEngine;

namespace PlayerLogic
{
    public class PrintInfo : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J)) PlayerInfo.SaveData();
        }
    }

}

