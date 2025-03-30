using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    public Item GetItem(int index)
    {
        return items[index];
    }
    public bool AddItem(Item item, int index)
    {
        if (items[index] == null)
        {
            items[index] = item;
            return true;
        }
        return false;
    }
    public void RemoveItem(int index)
    {
        items[index] = null;
    }
    //public bool CreateSlot()
    //{
    //    if (_slots.Count < _maxCountSlots)
    //    {
    //        GameObject newSlot = new GameObject($"Slot {_slots.Count}");
    //        InventorySlot inventorySlot = newSlot.AddComponent<InventorySlot>();
    //        _slots.Add(inventorySlot);
    //        newSlot.transform.SetParent(this.transform.Find("UIPanel")?.Find("Slots"), true); //Установление связи с канвасом.
    //        newSlot.transform.localScale = Vector3.one * _sizeSlot;
    //        inventorySlot.StoredItem = null;
    //        inventorySlot.ImageStoredItem = inventorySlot.gameObject.AddComponent<UnityEngine.UI.Image>();
    //        inventorySlot.ImageStoredItem.sprite = _emptySlotImage;
    //        if (SlotCount > 0) newSlot.SetActive(false);
    //        return true;
    //    }
    //    return false;
    //}
}