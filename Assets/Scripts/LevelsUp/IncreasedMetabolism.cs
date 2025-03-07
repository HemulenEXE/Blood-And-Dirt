using PlayerLogic;
using SkillLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class IncreasedMetabolism : Skill
    {
        public IncreasedMetabolism()
        {
            _name = "IncreasedMetabolism";
            _isUnlocked = false;
            _type = SkillType.Activated;
        }

        public override void Execute(GameObject point)
        {
            if (!PlayerInfo.HasSkill<DropByDrop>()) --PlayerInfo._fullHealth;
        }
    }
}
