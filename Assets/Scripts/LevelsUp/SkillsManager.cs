using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace SkillLogic
{
    public class SkillsManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;
        public Button _startOfANewLife;
        public Button _musclesSecondSkeleton;
        public Button _increasedMetabolism;
        public Button _dropByDrop;
        public Button _musclesSecondSkeleton2;
        public Button _hatred;
        public Button _blindRage;
        public Button _liveInVain;
        public Button _reincarnation;


        private void Start()
        {
            _player = GameObject.FindWithTag("Player");


            var a1 = new StartOfANewLife();
            var a2 = new MusclesSecondSkeleton();
            var a3 = new IncreasedMetabolism();
            var a4 = new DropByDrop();
            var a5 = new MusclesSecondSkeleton2();
            var a6 = new Hatred();
            var a7 = new BlindRange();
            var a8 = new LiveInNotVain();
            var a9 = new Reincarnation();

            a2._previousSkills.Add(a1);

            a3._previousSkills.Add(a2);
            a4._previousSkills.Add(a2);
            a5._previousSkills.Add(a2);

            a6._previousSkills.Add(a4);

            a7._previousSkills.Add(a6);
            a8._previousSkills.Add(a6);
            a9._previousSkills.Add(a6);

            _startOfANewLife.onClick.AddListener(() => ActivateSkill(a1));
            _musclesSecondSkeleton.onClick.AddListener(() => ActivateSkill(a2));
            _increasedMetabolism.onClick.AddListener(() => ActivateSkill(a3));
            _dropByDrop.onClick.AddListener(() => ActivateSkill(a4));
            _musclesSecondSkeleton2.onClick.AddListener(() => ActivateSkill(a5));
            _hatred.onClick.AddListener(() => ActivateSkill(a6));
            _liveInVain.onClick.AddListener(() => AddSkill(a8));
            _reincarnation.onClick.AddListener(() => ActivateSkill(a9));
        }
        private void AddSkill(Skill skill)
        {
            skill.Unlock();
            if (skill._isUnlocked)
            {
                PlayerInfo.AddSkill(skill);
                Debug.Log($"{skill._name} is activated!");
            }
        }
        private void ActivateSkill(Skill skill)
        {
            skill.Unlock();
            if (skill._isUnlocked)
            {
                PlayerInfo.AddSkill(skill);
                skill.Execute(_player);
                Debug.Log($"{skill._name} is activated!");
            }
        }
    }
}
