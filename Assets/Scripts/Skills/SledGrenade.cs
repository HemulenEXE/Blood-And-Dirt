﻿using UnityEngine;

public class SledGrenade : Skill
{
    public SledGrenade()
    {
        Name = "SledGrenade";
        IsUnlocked = false;
        Type = SkillType.Added;
    }

    public override void Execute(GameObject point) // Вызывается в начале миссии или после воскрешения
    {
        PlayerData.SimpleGrenadeCount += 2;
        PlayerData.SmokeGrenadeCount += 2;
    }
}
