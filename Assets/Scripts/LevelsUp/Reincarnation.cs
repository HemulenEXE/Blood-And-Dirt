using PlayerLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class Reincarnation : Skill
    {
        private GameObject _body;
        private AudioClip _audio;
        private int _newHitsToSurvive = 3;

        public Reincarnation()
        {
            _name = "Reincarnation";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
            _body = Resources.Load<GameObject>("Prefabs/Ghost");
            _audio = Resources.Load<AudioClip>("Audios/Reinc");
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._hitsToSurvive = _newHitsToSurvive;
        }
        public virtual void SpawnBody(GameObject point)
        {
            if (PlayerInfo._hitsToSurvive > 0)
            {
                // «вук не работает, потому что игрока быстро убивают
                Debug.Log(point.GetComponent<AudioSource>() is null);
                point.GetComponent<AudioSource>().PlayOneShot(_audio);
                GameObject.Instantiate(_body, point.transform.position, Quaternion.identity);
            }
        }
    }
}

