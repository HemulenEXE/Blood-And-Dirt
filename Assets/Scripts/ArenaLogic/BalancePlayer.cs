using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BalancePlayer : MonoBehaviour
{
    private static BalancePlayer _instance;
    public static BalancePlayer Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("BalancePlayer");
                _instance = obj.AddComponent<BalancePlayer>();
            }
            return _instance;
        }
    }

    private int countMoney = 0;

    public bool IsCanBuy(int price)
    {
        return countMoney >= price;
    }

    public void BuyObject(int price)
    {
        countMoney -= price;
    }

    public void GiveSallary(int sallary) 
    {
        countMoney += sallary;
    }
}
