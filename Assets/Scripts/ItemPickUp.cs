using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Класс предметов, которые можно взять в инвентарь.
/// </summary>
public class ItemPickUp : MonoBehaviour
{
    /// <summary>
    /// Иконка предмета в инвентаре.
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
    /// <summary>
    /// TODO
    /// </summary>
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //    PlayerInventory._slots[PlayerInventory._currSlot].AddItem(this.gameObject);
    }
}
