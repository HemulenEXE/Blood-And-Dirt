using InventoryLogic;
using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class SledGrenade : Skill
    {
        public SledGrenade()
        {
            _name = "SledGrenade";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }
        public override void Execute(GameObject point)
        {
            ConsumableCounter._simpleGrenadeCount += 2;
            ConsumableCounter._smokeGrenadeCount += 2;
        }
    }
}
