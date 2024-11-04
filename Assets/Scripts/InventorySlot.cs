using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ����� ��������� ������
/// </summary>
public class InventorySlot : MonoBehaviour, IInventorySlot
{
    /// <summary>
    /// �������, �������� � ������ �����
    /// </summary>
    [SerializeField] private GameObject _storedItem;
    public GameObject StoredItem
    {
        get => _storedItem;
        set => _storedItem = value;
    }
    /// <summary>
    /// ������������� �����
    /// </summary>
    [SerializeField] private bool _isFull;
    public bool IsFull
    {
        get => _isFull;
        set => _isFull = value;
    }
    /// <summary>
    /// ���������� �������� item � ������ ����
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item)
    {
        if (!IsFull)
        {
            item.SetActive(false);
            StoredItem = item;
            IsFull = true;
        }
    }
    /// <summary>
    /// ������������ (�������) ���� ����
    /// </summary>
    public void RemoveItem()
    {
        if (IsFull)
        {
            StoredItem.SetActive(true);
            StoredItem = null;
            IsFull = false;
        }
    }
}
