using PlayerLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class StartOfANewLife : Skill
    {
        [SerializeField]
        private float _newFullHealth = 20f;
        [SerializeField]
        private int _newDamageBleeding;

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

