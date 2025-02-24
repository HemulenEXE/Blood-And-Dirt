using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillLogic
{
    public class LiveInNotVain : Skill
    {
        private float _searchRadius = 100f;

        public LiveInNotVain()
        {
            _name = "LiveInNotVain";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
        }

        public override void Execute(GameObject point)
        {
            var hitColliders = Physics.OverlapSphere(point.transform.position, _searchRadius);
            Debug.Log(hitColliders.Length);
            foreach(var x in hitColliders)
            {
                x.GetComponent<Body>().GetDamage(1);
                Debug.Log($"{x.name} is eated");
            }

        }
    }
}
