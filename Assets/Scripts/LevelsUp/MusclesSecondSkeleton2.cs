using PlayerLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class MusclesSecondSkeleton2 : Skill
    {
        private int _newHitsToSurvive = 5;

        public MusclesSecondSkeleton2()
        {
            _name = "MusclesSecondSkeleton2";
            _isUnlocked = false;
            _type = SkillType.Activated;
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._hitsToSurvive = _newHitsToSurvive;
            PlayerInfo._runNoise *= 10;
        }
    }
}
