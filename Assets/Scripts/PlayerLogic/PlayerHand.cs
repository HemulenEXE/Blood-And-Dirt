using UnityEngine;
using InventoryLogic;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "руку игрока".
    /// </summary>
    public class PlayerHand : MonoBehaviour
    {
        private void Update()
        {
            if (PlayerInventory._slots[PlayerInventory._currentSlot]?.StoredItem != null)
            {
                Transform item_transform = PlayerInventory._slots[PlayerInventory._currentSlot].StoredItem.transform;
                item_transform.position = this.transform.position - transform.up / 2;
                item_transform.rotation = this.transform.rotation;
                PlayerInventory._slots[PlayerInventory._currentSlot].StoredItem.Active();
                PlayerInventory._slots[PlayerInventory._currentSlot].StoredItem.gameObject.layer = LayerMask.NameToLayer("Invisible");
            }
        }
    }
}