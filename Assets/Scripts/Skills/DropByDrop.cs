using UnityEngine;

namespace SkillLogic
{
    public class DropByDrop : Skill
    {
        public DropByDrop()
        {
            Name = "DropByDrop";
            IsUnlocked = false;
            Type = SkillType.Activated;
        }

        public override void Execute(GameObject point)
        {
            PlayerData.BleedingDamage /= 2;
        }
    }

}
