using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace SkillLogic
{
    public class Reincarnation : Skill
    {
        //private GameObject _body;
        //private AudioClip _audio;
        private int _newHitsToSurvive = 3;

        public Reincarnation()
        {
            _name = "Reincarnation";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
            //_body = Resources.Load<GameObject>("Skull");
            //_audio = Resources.Load<AudioClip>("Reinc");
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._hitsToSurvive = _newHitsToSurvive;
        }
        public virtual void SpawnBody(GameObject point)
        {
            if (PlayerInfo._hitsToSurvive > 0)
            {
                Debug.Log($"Body is spawned");
                //point.GetComponent<AudioSource>().PlayOneShot(_audio);
                //Object.Instantiate(_body, point.transform.position, Quaternion.identity);
            }
        }
    }
}

