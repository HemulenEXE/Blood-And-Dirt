using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Barraks : MonoBehaviour
{
    private ILevelArmy levelArmy ;
    [SerializeField] private List<GameObject> spawns;
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private int maxStrenghtArmy;
    [SerializeField] private int maxStrenghtWave = 10;

    public List<BotController> SpawnSolders(Side sideEnemy)
    {
        List<BotController> spawnedBot = new List<BotController>();

        int currentStrengthWave = 0;

        while (currentStrengthWave < maxStrenghtWave) 
        {
            int placeSpawn = UnityEngine.Random.Range(0,spawns.Count);
            int target = UnityEngine.Random.Range(0, targets.Count);
            Tuple<BotController,int> buyUnit = levelArmy.GetRandomSolder(maxStrenghtWave - currentStrengthWave);
            currentStrengthWave += buyUnit.Item2;
            buyUnit.Item1.InitEnemy(sideEnemy);
            buyUnit.Item1.transform.position = spawns[placeSpawn].transform.position;
            buyUnit.Item1.GetComponent<NavMeshAgent>().SetDestination(targets[target].transform.position);
            spawnedBot.Add(buyUnit.Item1);
        }

        return spawnedBot;
    }

}
