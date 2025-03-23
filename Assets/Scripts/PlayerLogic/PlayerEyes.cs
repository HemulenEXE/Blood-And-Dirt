using SkillLogic;
using UnityEngine;
namespace PlayerLogic
{
    public class PlayerEyes : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerInfo.HasSkill<Sound>())
            {
                PlayerInfo.GetSkill<Sound>().Execute(this.gameObject);
            }
        }
    }
}
