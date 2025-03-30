using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Unity.Mathematics;

public class LevelArmyEasy : MonoBehaviour, ILevelArmy
{
    public EnemySides sideArmy;
    private List<Unit> costSolders;

    private void Awake()
    {
        costSolders = new List<Unit>();
        if (sideArmy == EnemySides.believers)
        {
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithPistol"), 1));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithMachineGun"), 2));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithMachineGun"), 2)); // Поправить говно с префабом дробовика
        }
        else if(sideArmy == EnemySides.falcons)
        {
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/PurpleSoldierWithPistol"), 1));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/PurpleSoldierWithMachineGun"), 2));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/PurpleSoldierWithShotGun"), 2));
        }
        
        

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
