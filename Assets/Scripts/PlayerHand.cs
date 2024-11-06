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
    /// Помещает предмет в выбранном слоте в руку игрока.
    /// </summary>
    public void TakeSelectionSlotInHand()
    {
        if (PlayerInventory._slots[PlayerInventory._currSlot].StoredItem != null)
        {
            Transform item_trans = PlayerInventory._slots[PlayerInventory._currSlot].StoredItem.transform;
            item_trans.position = this.transform.position + transform.right;
            item_trans.rotation = this.transform.rotation;
            PlayerInventory._slots[PlayerInventory._currSlot].StoredItem.gameObject.SetActive(true);
        }
    }
}