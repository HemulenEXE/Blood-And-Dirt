using UnityEngine.UI;

/// <summary>
/// Класс, реализующий "инвентарный слот".
/// </summary>
public class InventorySlot : AbstractInventorySlot
{
    /// <summary>
    /// Добавление предмета в слот.
    /// </summary>
    /// <param name="item"></param>
    public override void AddItem(UnvisibleItemPickUp item)
    {
        if (!IsFull())
        {
            item.InHand = true;
            item.Deactive();
            StoredItem = item;
            ImageStoredItem.GetComponent<Image>().sprite = item.Icon;
        }
    }
    /// <summary>
    /// Очищение слота.<br/>
    /// Производится сброс хранимого предмета.
    /// </summary>
    public override void RemoveItem()
    {
        if (IsFull())
        {
            StoredItem.InHand = false;
            StoredItem.Active();
            StoredItem = null;
            ImageStoredItem.GetComponent<Image>().sprite = PlayerInventory._emptySlotImage;
        }
    }
}
