using UnityEngine;

/// <summary>
/// Класс руки игрока.
/// </summary>
public class PlayerHand : MonoBehaviour
{
    private void Update()
    {
        TakeSelectionSlotInHand();
    }
    /// <summary>
    /// Помещает предмет выбранного слота в руку игрока.
    /// </summary>
    public void TakeSelectionSlotInHand()
    {
        if (PlayerInventory._slots[PlayerInventory._currSlot].StoredItem != null)
        {
            Transform item_transform = PlayerInventory._slots[PlayerInventory._currSlot].StoredItem.transform;
            item_transform.position = this.transform.position + transform.right;
            item_transform.rotation = this.transform.rotation;
            PlayerInventory._slots[PlayerInventory._currSlot].StoredItem.gameObject.SetActive(true);
        }
    }
}