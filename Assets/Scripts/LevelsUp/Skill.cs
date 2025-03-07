using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillLogic
{
    public abstract class Skill
    {
        public string _name;
        public bool _isUnlocked;
        public List<Skill> _previousSkills;

        public abstract void Execute(GameObject point);
        public void Unlock()
        {
            if (!_isUnlocked)
            {
                _isUnlocked = _previousSkills.All(x => x._isUnlocked);
            }
        }
    }
}
