using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public class LevelArmyEasy : ILevelArmy
{
    private Dictionary<int, BotController> costSolders = new Dictionary<int, BotController> { { 1, new BotController() } };
    private int[] costs = { 1 };

    public Tuple<BotController,int> GetRandomSolder(int maxCostSolder)
    {
        int[] correctOptions = costs.Where(x => x <= maxCostSolder).ToArray();
        int randomCorrectOption = correctOptions[UnityEngine.Random.Range(0, correctOptions.Length)];
        int remainder = maxCostSolder - randomCorrectOption;
        return new Tuple<BotController, int>(costSolders[randomCorrectOption], remainder);
    }
}
