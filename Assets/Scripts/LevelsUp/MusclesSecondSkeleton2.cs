using PlayerLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class MusclesSecondSkeleton2 : Skill
    {
        [SerializeField]
        private int _newHitsToSurvive = 5;

        public MusclesSecondSkeleton2()
        {
            _name = "MusclesSecondSkeleton2";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._hitsToSurvive = _newHitsToSurvive;
            PlayerInfo._runNoise *= 10;
        }
    }
}
