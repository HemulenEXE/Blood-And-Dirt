using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class Hatred : Skill
    {
        [SerializeField]
        private float _newRunSpeed;

        public Hatred()
        {
            _name = "Hatred";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {
            if (PlayerInfo._isBleeding)
            {
                PlayerInfo._isRunnig = true;
                PlayerInfo._runSpeed = _newRunSpeed;
                var bodies = GameObject.FindGameObjectsWithTag("Body");
                // Уменьшение целостности трупов - можно вызвать отдельно метод
            }
        }
    }
}
