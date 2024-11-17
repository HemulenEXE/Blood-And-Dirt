using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ������ ���������� ��������� (�������, �����, �������) �� ����� � ���������� ���������� ��� ���-�� �� �������
/// </summary>
public class ConsumablesVCO : VisibleClickedObject
{
    private GameObject _count; //������, ������������ ������ ���-�� ���������� � ������ 
    private void Start()
    {
        Transform panel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(2).transform; 
        Transform icon = panel.Find(this.tag); //������ ���������� �� �������
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
