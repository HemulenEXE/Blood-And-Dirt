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
            temp.transform.position = this.transform.position - transform.up / 2;
            temp.transform.rotation = this.transform.rotation;
            temp.gameObject.layer = LayerMask.NameToLayer("Invisible");
        }
        if (temp.GetComponent<GrenadeLauncher>() != null) temp.transform.position = this.transform.position + transform.up / 2;
    }
}