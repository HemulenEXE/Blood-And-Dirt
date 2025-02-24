using SkillLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace PlayerLogic
{
    public static class PlayerInfo
    {
        public static List<Skill> _skills = new List<Skill>();

        public static float _stealSpeed = 2f;
        public static float _walkSpeed = 4f;
        public static float _runSpeed = 6f;


        public static bool _isStealing;
        public static bool _isWalking;
        public static bool _isRunnig;

        public static float _stealNoise = 0.3f;
        public static float _walkNoise = 2f;
        public static float _runNoise = 8f;

        public static float _fullHealth = 10f;
        public static float _currentHealth;

        public static int _bleedingDamage;

        public static bool _isBleeding;

        public static int _countArmor;
        public static int _bandageHealth;
        public static int _firstAidKitHealth;

        public static int _hitsToSurvive = 0;

        public static bool _isGod; // Неузвимость

        public static bool HasSkill<T>() where T : Skill
        {
            return _skills.OfType<T>().Any();
        }
        
        public static void AddSkill(Skill skill)
        {
            _skills.Add(skill);
        }
        public static bool RemoveSkill(Skill skill)
        {
            return _skills.Remove(skill);
        }
        public static Skill FindSkill(string name)
        {
            foreach (var x in _skills)
            {
                if (x._name.Equals(name)) return x;
            }
            return null;
        }
        public static void ExecuteSkill(string name, GameObject point)
        {
            foreach (var x in _skills)
            {
                if (x._name.Equals(name)) x.Execute(point);
            }
        }
        public static T GetSkill<T>() where T : Skill
        {
            foreach (Skill skill in _skills)
            {
                if (skill is T)
                {
                    return skill as T;
                }
            }
            return null;
        }
    }

}
