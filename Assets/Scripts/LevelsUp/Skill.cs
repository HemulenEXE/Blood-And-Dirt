using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkillType { Added, Activated };

namespace SkillLogic
{
    public abstract class Skill
    {
        
        public string _name;
        public bool _isUnlocked;
        public SkillType _type;

        public abstract void Execute(GameObject point);

    }
}
