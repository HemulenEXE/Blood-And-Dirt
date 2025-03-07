using UnityEngine;
using InventoryLogic;
using InteractiveObjects;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "руку игрока".
    /// </summary>
    public class PlayerHand : MonoBehaviour
    {
        private void Update()
        {
            if (Inventory.GetInstance.CurrentSlot.StoredItem != null)
            {
                Transform item_transform = Inventory.GetInstance.CurrentSlot.StoredItem.transform;
                item_transform.position = this.transform.position - transform.up / 2;
                item_transform.rotation = this.transform.rotation;
                Inventory.GetInstance.CurrentSlot.StoredItem.gameObject.layer = LayerMask.NameToLayer("Invisible");
            }
        }
    }
}