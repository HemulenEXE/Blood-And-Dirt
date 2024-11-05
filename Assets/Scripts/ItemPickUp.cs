using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Класс предметов, которые можно поднять, взять в инвентарь.
/// </summary>
public class ItemPickUp : MonoBehaviour
{
    /// <summary>
    /// Иконка в инвентаре данного предмета.
    /// </summary>
    [SerializeField] private Image _icon;
    public Image Icon
    {
        get => _icon;
    }
    private void Awake()
    {
        if (Icon == null) throw new ArgumentNullException("ItemPickUp: Icon is null");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PlayerInventory._slots[PlayerInventory._currSlot].AddItem(this.gameObject);
    }
}
