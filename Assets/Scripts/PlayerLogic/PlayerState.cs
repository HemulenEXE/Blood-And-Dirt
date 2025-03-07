using SkillLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public HashSet<Skill> _skills = new HashSet<Skill>();

    public float _stealSpeed = 2f;
    public float _walkSpeed = 4f;
    public float _runSpeed = 6f;

    public static bool _isStealing;
    public static bool _isWalking;
    public static bool _isRunnig;
    public static bool _isStaing;

    public static float _stealNoise = 0.3f;
    public static float _walkNoise = 2f;
    public static float _runNoise = 8f;

    public static int _fullHealth = 100;
    public static int _currentHealth = 100;

    public static int _bleedingDamage;

    public static bool _isBleeding;

    public static int _countArmor;
    public static int _bandageHealth;
    public static int _firstAidKitHealth;

    public static int _hitsToSurvive = 0;

    public static bool _isGod; // Неузвимость

    public static bool _isFighting;

    public static int _bodyCount; // Количество воскрешений
}
