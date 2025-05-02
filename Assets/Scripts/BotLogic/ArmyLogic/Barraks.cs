using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Barraks : MonoBehaviour
{
    private ILevelArmy levelArmy ;
    [SerializeField] private List<GameObject> spawns;
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private int maxStrenghtWave = 10;

    private void Awake()
    {
        levelArmy = GetComponent<ILevelArmy>();
    }

    public void SetArmy(ILevelArmy levelArmy)
    {
        this.levelArmy = levelArmy;
    }

    public void SetMaxStrengthWave(int maxStrenghtWave)
    {
        this.maxStrenghtWave = maxStrenghtWave;
    }
    public void AddMaxStrengthWave(int maxStrenghtWave)
    {
        this.maxStrenghtWave += maxStrenghtWave;
    }

    public List<BotController> SpawnSolders(Side sideEnemy)
    {
        List<BotController> spawnedBot = new List<BotController>();

        int currentStrengthWave = 0;

        while (currentStrengthWave < maxStrenghtWave) 
        {
            int placeSpawn = UnityEngine.Random.Range(0,spawns.Count);
            int target = UnityEngine.Random.Range(0, targets.Count);
            Unit buyUnit = levelArmy.GetRandomSolder(maxStrenghtWave - currentStrengthWave);
            currentStrengthWave += buyUnit.costUnit;
            GameObject spawnBot = Instantiate(buyUnit.unit, spawns[placeSpawn].transform.position,buyUnit.unit.transform.rotation);
            spawnBot.GetComponent<BotController>().InitEnemy(sideEnemy);
            Helper.SetLayerRecursive(spawnBot, LayerMask.NameToLayer(sideEnemy.GetOwnLayer()));
            spawnBot.tag = sideEnemy.GetOwnLayer();
            spawnBot.GetComponent<BotController>().AddPatrolState(targets[target].transform);
            spawnedBot.Add(spawnBot.GetComponent<BotController>());
        }

        return spawnedBot;
    }

}
