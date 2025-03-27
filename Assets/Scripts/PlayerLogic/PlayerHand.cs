using UnityEngine;
using System;

public class PlayerHand : MonoBehaviour
{
    private InventoryAndConsumableCounterUI inventoryAndConsumableCounterUI;

    private void Start()
    {
        inventoryAndConsumableCounterUI = GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>();

        if (inventoryAndConsumableCounterUI == null) throw new ArgumentNullException("PlayerHand: inventoryAndConsumableCounterUI is null");
    }
    private void Update()
    {
        var temp = inventoryAndConsumableCounterUI.GetItem();
        if (temp != null)
        {
            Transform item_transform = temp.transform;
            item_transform.position = this.transform.position - transform.up / 2;
            item_transform.rotation = this.transform.rotation;
            temp.gameObject.layer = LayerMask.NameToLayer("Invisible");
        }
    }
}