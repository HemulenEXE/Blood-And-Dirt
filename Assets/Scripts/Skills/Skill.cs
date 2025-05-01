using UnityEngine;

public enum SkillType { Added, Activated };

public abstract class Skill
{
    public string Name { get; set; }
    public bool IsUnlocked { get; set; }
    public int Cost { get; set; } = 1;
    public SkillType Type { get; set; }

    public abstract void Execute(GameObject point);
}