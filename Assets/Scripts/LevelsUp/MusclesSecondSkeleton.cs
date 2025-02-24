using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class MusclesSecondSkeleton : Skill
    {
        [SerializeField]
        private int _newHitsToSurvive = 3;

        public MusclesSecondSkeleton()
        {
            _name = "MusclesSecondSkeleton";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._hitsToSurvive = _newHitsToSurvive;
            PlayerInfo._walkNoise *= 2;
        }
    }
}

