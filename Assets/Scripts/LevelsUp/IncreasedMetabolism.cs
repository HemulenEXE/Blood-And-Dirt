using UnityEngine;

namespace SkillLogic
{
    public class IncreasedMetabolism : Skill
    {
        public IncreasedMetabolism()
        {
            Name = "IncreasedMetabolism";
            IsUnlocked = false;
            Type = SkillType.Activated;
        }

        public override void Execute(GameObject point)
        {
            if (!PlayerData.HasSkill<DropByDrop>()) --PlayerData.MaxHealth;
        }
    }
}
