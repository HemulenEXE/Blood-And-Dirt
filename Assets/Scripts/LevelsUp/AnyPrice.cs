using PlayerLogic;
using SkillLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class AnyPrice : Skill
    {
        public AnyPrice()
        {
            _name = "AnyPrice";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }
        public override void Execute(GameObject point)
        {
            PlayerInfo._runNoise /= 2;
            PlayerInfo._walkNoise /= 2;
        }
    }

}