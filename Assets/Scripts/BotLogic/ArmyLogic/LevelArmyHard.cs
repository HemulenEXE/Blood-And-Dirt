using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class LevelArmyHard : MonoBehaviour, ILevelArmy
{
    
    public EnemySides sideArmy;
    private List<Unit> costSolders;

    private void Start()
    {
        costSolders = new List<Unit>();
        if (sideArmy == EnemySides.believers)
        {
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithMachineGun"), 1));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithGrenadeLauncher"), 1)); 
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/BossComponent/Prophet"), 10));
        }
        else if (sideArmy == EnemySides.falcons)
        {
            throw new Exception("Inccorect side for hard arny!");
        }



        foreach (Unit u in costSolders)
        {
            Debug.Log(u.unit);
        }
    }

    public Unit GetRandomSolder(int maxCostSolder)
    {
        var filteredUnit = costSolders.Where(e => e.costUnit <= maxCostSolder).ToList();
        return filteredUnit[UnityEngine.Random.Range(0, filteredUnit.Count)];
    }
}
