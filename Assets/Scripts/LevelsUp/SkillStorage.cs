using System.Collections.Generic;
using UnityEngine;

namespace SkillLogic
{
    public class SkillStorage : MonoBehaviour
    {
        public static Dictionary<string, Skill> _skills = new Dictionary<string, Skill>{
        { "AnyPrice", new AnyPrice() },
        { "BlindRange", new BlindRange() },
        { "DropByDrop", new DropByDrop() },
        { "Hatred", new Hatred() },
        { "IncreasedMetabolism", new IncreasedMetabolism() },
        { "LiveInNotVain", new LiveInNotVain() },
        { "MusclesSecondSkeleton", new MusclesSecondSkeleton() },
        { "MusclesSecondSkeleton2", new MusclesSecondSkeleton2() },
        { "Reincarnation", new Reincarnation() },
        { "SledGrenade", new SledGrenade() },
        { "Sound", new Sound() },
        { "Spin", new Spin() },
        { "StartOfANewLife", new StartOfANewLife() }};
    }
}
