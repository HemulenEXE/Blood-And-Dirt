using PlayerLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class StartOfANewLife : Skill
    {
        [SerializeField]
        private int _newFullHealth = 200;
        [SerializeField]
        private int _newDamageBleeding = 2;

        public StartOfANewLife()
        {
            _name = "StartOfANewLife";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._fullHealth = _newFullHealth;
            PlayerInfo._bleedingDamage = _newDamageBleeding;
        }
    }
}

