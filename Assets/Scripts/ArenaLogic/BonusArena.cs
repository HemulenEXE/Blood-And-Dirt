using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusArena : MonoBehaviour
{
    [SerializeField] private List<GameObject> possibleBonuses;
    [SerializeField] private int timeLife = 120;
    private GameObject currentBonus;

    public void UpdateCurrentBonus()
    {
        Debug.Log("Bonus!");
        if (possibleBonuses.Count > 0)
        {
            currentBonus = possibleBonuses[UnityEngine.Random.Range(0, possibleBonuses.Count)];
            GameObject bonus = Instantiate(currentBonus, transform);
            Destroy(bonus, timeLife);
        }
    }
}
