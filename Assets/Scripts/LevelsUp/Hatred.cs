using PlayerLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class Hatred : Skill
    {
        private float _newRunSpeed = 20f;
        private float _oldRunSpeed;

        public Hatred()
        {
            _name = "Hatred";
            _isUnlocked = false;
            _oldRunSpeed = PlayerInfo._runSpeed;
            _type = SkillType.Activated;
        }

        public override void Execute(GameObject point)
        {
            if (PlayerInfo._isBleeding)
            {
                PlayerInfo._isRunning = true;
                PlayerInfo._runSpeed = _newRunSpeed;
                //var bodies = GameObject.FindGameObjectsWithTag("Body");
                // Уменьшение целостности трупов - можно вызвать отдельно метод
            }
            else PlayerInfo._runSpeed = _oldRunSpeed;
        }
    }
}
