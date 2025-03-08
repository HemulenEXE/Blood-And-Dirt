using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerData.Skills.Add(PlayerData.SkillsStorage["InevitableDeath"]);
        }
    }
}
