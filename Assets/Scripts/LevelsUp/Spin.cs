using InventoryLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class Spin : Skill
    {
        public Spin()
        {
            _name = "Spin";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {
            ++ConsumableCounter._bandageCount;
            ++ConsumableCounter._firstAidKitCount;
        }
    }
}
