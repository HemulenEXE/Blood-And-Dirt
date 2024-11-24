using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Хранит кол-во каждого расходника, а также управляет его отображением на канвасе. 
/// Навешивается на элемент ConsumablesUI в канвасе
/// </summary>
public class Consumables : MonoBehaviour
{
    //Иконки каждого расходника на канвасе
    private Transform _smokeGrenade;
    private Transform _simpleGrenade;
    private Transform _firstAidKit;
    private Transform _bandage;

    //Кол-во каждого расходника
    private static int _smokeGrenadeCount = 0;
    private static int _simpleGrenadeCount = 0;
    private static int _firstAidKitCount = 0;
    private static int _bandageCount = 0;

    public static int smokeGrenadeCount
    {   get { return _smokeGrenadeCount; }
        set { if (value < 0 || value > 5)
                throw new ArgumentOutOfRangeException("Invalid value!");
            _smokeGrenadeCount = value; }
    }
    public static int simpleGrenadeCount
    {
        get { return _simpleGrenadeCount; }
        set { if (value < 0 || value > 5)
                throw new ArgumentOutOfRangeException("Invalid value!");
            _simpleGrenadeCount = value; }
    }
    public static int firstAidKitCount
    {
        get { return _firstAidKitCount; }
        set { if (value < 0 || value > 5)
                 throw new ArgumentOutOfRangeException("Invalid value!");
            _firstAidKitCount = value; }
    }
    public static int bandageCount
    {
        get { return _bandageCount; }
        set { if (value < 0 || value > 5)
                throw new ArgumentOutOfRangeException("Invalid value!");
            _bandageCount = value; }
    }
    private void Awake()
    {
        //Инициализация полей
        _smokeGrenade = transform.GetChild(0);
        _simpleGrenade = transform.GetChild(1);
        _firstAidKit = transform.GetChild(3);
        _bandage = transform.GetChild(4);
    }
    private void Update()
    {
        _smokeGrenade.GetComponentInChildren<TextMeshProUGUI>().text = _smokeGrenadeCount.ToString();
        _simpleGrenade.GetComponentInChildren<TextMeshProUGUI>().text = _simpleGrenadeCount.ToString();
        _firstAidKit.GetComponentInChildren<TextMeshProUGUI>().text = _firstAidKitCount.ToString();
        Debug.Log(_bandage);
        Debug.Log(_bandage.GetComponentInChildren<TextMeshProUGUI>());
        _bandage.GetComponentInChildren<TextMeshProUGUI>().text = _bandageCount.ToString();
    }
}
