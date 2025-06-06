using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelArmyMedium : MonoBehaviour, ILevelArmy
{
    public EnemySides sideArmy;
    private List<Unit> costSolders;
    public void Init()
    {
        if (costSolders != null) return;

        costSolders = new List<Unit>();
        if (sideArmy == EnemySides.believers)
        {
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithMachineGun"), 1));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithMachineGun"), 1)); // ��������� ����� � �������� ���������
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/GreenSoldierWithGrenadeLauncher"), 2));
        }
        else if (sideArmy == EnemySides.falcons)
        {
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/PurpleSoldierWithMachineGun"), 1));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/PurpleSoldierWithShotGun"), 1));
            costSolders.Add(new Unit(Resources.Load<GameObject>("Prefabs/Enemies/PurpleSoldierWithLauncher"), 2));
        }



        foreach (Unit u in costSolders)
        {
            Debug.Log(u.unit);
        }
    }
    public void Start()
    {
        Init();
    }

    public Unit GetRandomSolder(int maxCostSolder)
    {
        var filteredUnit = costSolders.Where(e => e.costUnit <= maxCostSolder).ToList();
        return filteredUnit[UnityEngine.Random.Range(0, filteredUnit.Count)];
    }
}
