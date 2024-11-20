using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Скрипт забирающий расходник (аптечка, бинты, гранаты) со сцены и изменяющий показатель его кол-ва на канвасе. Навешивается на расходник на сцене
/// </summary>
public class ConsumablesVCO : VisibleClickedObject
{
    private GameObject _count; //Объект, показывающий текщее кол-во расходника у игрока 
    private void Start()
    {
        Transform panel = GameObject.FindGameObjectWithTag("Canvas").transform.Find("ConsumablesUI"); 
        Transform icon = panel.Find(this.tag); //Иконка расходника на канвасе
        _count = icon.GetChild(0).gameObject;
    }
    public override void Interact()
    {
        if (_count.GetComponent<TextMeshProUGUI>().text != "5")
        {
            Destroy(this.gameObject);
            _count.GetComponent<TextMeshProUGUI>().text = (int.Parse(_count.GetComponent<TextMeshProUGUI>().text) + 1).ToString();
        }
    }
}
