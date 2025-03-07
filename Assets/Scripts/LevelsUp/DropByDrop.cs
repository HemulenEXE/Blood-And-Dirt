using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class DropByDrop : Skill
    {
        public DropByDrop()
        {
            _name = "DropByDrop";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._bleedingDamage /= 2;
        }
    }

}
