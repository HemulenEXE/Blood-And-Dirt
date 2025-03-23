using PlayerLogic;
using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class Reincarnation : Skill
    {
        private GameObject _body;
        private AudioClip _audio;
        private int _newBodyCount = 3;

        public Reincarnation()
        {
            _name = "Reincarnation";
            _isUnlocked = false;
            _body = Resources.Load<GameObject>("Prefabs/Ghost");
            _audio = Resources.Load<AudioClip>("Audios/Reinc");
            _type = SkillType.Activated;
        }

        public override void Execute(GameObject point)
        {
            PlayerInfo._bodyCount = _newBodyCount;
        }
        public virtual void SpawnBody(GameObject point)
        {
            if (PlayerInfo._bodyCount > 0)
            {
                point.GetComponent<AudioSource>().PlayOneShot(_audio);
                GameObject.Instantiate(_body, point.transform.position, Quaternion.identity);
                --PlayerInfo._bodyCount;
            }
        }
    }
}

