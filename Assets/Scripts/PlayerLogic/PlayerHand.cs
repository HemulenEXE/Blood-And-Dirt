using UnityEngine;

/// <summary>
/// Класс, реализующий "руку игрока".
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
        if (PlayerInventory._slots[PlayerInventory._currentSlot]?.StoredItem != null)
        {
            Transform item_transform = PlayerInventory._slots[PlayerInventory._currentSlot].StoredItem.transform;
            item_transform.position = this.transform.position + transform.right;
            item_transform.rotation = this.transform.rotation;
            PlayerInventory._slots[PlayerInventory._currentSlot].StoredItem.gameObject.SetActive(true);
        }
    }
}