using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс слота инвентаря игрока
/// </summary>
public class InventorySlot : MonoBehaviour, IInventorySlot
{
    /// <summary>
    /// Предмет, хранимый в данном слоте
    /// </summary>
    [SerializeField] private GameObject _storedItem;
    public GameObject StoredItem
    {
        get => _storedItem;
        set => _storedItem = value;
    }
    /// <summary>
    /// Заполненность слота
    /// </summary>
    [SerializeField] private bool _isFull;
    public bool IsFull
    {
        get => _isFull;
        set => _isFull = value;
    }
    /// <summary>
    /// Добавление предмета item в данный слот
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
    /// Высвобождает (очищает) этот слот
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
