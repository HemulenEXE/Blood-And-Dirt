using UnityEngine;

/// <summary>
/// Тестовый класс предметов, которые можно взять в инвентарь
/// </summary>
public class ItemPickUp : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PlayerInventory._slots[PlayerInventory._currSlot].AddItem(this.gameObject);
    }
}
