using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// —крипт забирающий расходник (аптечка, бинты, гранаты) со сцены и измен€ющий показатель его кол-ва на канвасе
/// </summary>
public class ConsumablesVCO : VisibleClickedObject
{
    private GameObject _count; //ќбъект, показывающий текщее кол-во расходника у игрока 
    private void Start()
    {
        Transform panel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(2).transform; 
        Transform icon = panel.Find(this.tag); //»конка расходника на канвасе
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
