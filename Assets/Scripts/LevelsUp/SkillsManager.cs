using PlayerLogic;
using UnityEngine;
using UnityEngine.UI;

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
        public Button _spin;
        public Button _anyPrice;
        public Button _sledGrenade;
        public Button _sound;

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
            var a10 = new Spin();
            var a11 = new AnyPrice();
            var a12 = new SledGrenade();
            var a13 = new Sound();

            a2._previousSkills.Add(a1);
            a10._previousSkills.Add(a1);
            a11._previousSkills.Add(a1);

            a3._previousSkills.Add(a2);
            a4._previousSkills.Add(a2);
            a5._previousSkills.Add(a2);
            a12._previousSkills.Add(a10);
            a13._previousSkills.Add(a11);

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
            _spin.onClick.AddListener(() => AddSkill(a10));
            _anyPrice.onClick.AddListener(() => ActivateSkill(a11));
            _sledGrenade.onClick.AddListener(() => AddSkill(a12));
            _sound.onClick.AddListener(() => AddSkill(a13));
        }
        private void AddSkill(Skill skill)
        {
            skill.Unlock();
            if (skill._isUnlocked && !PlayerInfo.HasSkill(skill))
            {
                PlayerInfo.AddSkill(skill);
                Debug.Log($"{skill._name} is added!");
            }
        }
        private void ActivateSkill(Skill skill)
        {
            skill.Unlock();
            if (skill._isUnlocked && !PlayerInfo.HasSkill(skill))
            {
                PlayerInfo.AddSkill(skill);
                skill.Execute(_player);
                Debug.Log($"{skill._name} is used!");
            }
        }
    }
}
