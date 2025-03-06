using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelArmy
{
    abstract public Tuple<BotController,int> GetRandomSolder(int maxStrength);
}
