using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Unity.Mathematics;

public class LevelArmyEasy : MonoBehaviour, ILevelArmy
{
    private List<Unit> costSolders;

    private void Awake()
    {
        costSolders = new List<Unit>();
        costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemys/Pistoller"), 1));
        costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemys/machine gunner"), 2));
        costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemys/ShotGunner"), 2));

        foreach(Unit u in costSolders)
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
