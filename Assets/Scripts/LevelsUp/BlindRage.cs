using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class BlindRange : Skill
    {
        public BlindRange()
        {
            _name = "BlindRange";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {

        }
    }

}