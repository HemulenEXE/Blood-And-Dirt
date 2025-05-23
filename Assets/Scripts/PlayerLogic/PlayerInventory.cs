﻿using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    private InventoryAndConsumableCounterUI inventoryAndConsumableCounterUI;

    private void Start()
    {
        inventoryAndConsumableCounterUI = GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>();

        if (inventoryAndConsumableCounterUI == null) throw new ArgumentNullException("PlayerInventory: inventoryAndConsumableCounterUI is null");
    }

    private void Update()
    {
        //// Перемещение между слотами с помощью пронумерованных клавиш 1 2 3 ...
        //for (int i = 49; i < 49 + inventoryAndConsumableCounterUI.Size; ++i)
        //{
        //    KeyCode key = (KeyCode)i;
        //    if (Input.GetKeyDown(key))
        //    {
        //        inventoryAndConsumableCounterUI.SelectSlot(i - 49);
        //    }
        //}
        // Перемещение между слотами посредством колёсика компьютерной мыши
        if (Input.GetAxis("Mouse ScrollWheel") > 0) inventoryAndConsumableCounterUI.SelectNextSlot();
        if (Input.GetAxis("Mouse ScrollWheel") < 0) inventoryAndConsumableCounterUI.SelectPreviousSlot();
        // Освобождение выбранного слота
        
        if (Input.GetKeyDown(SettingData.Bonus)) inventoryAndConsumableCounterUI.RemoveItem();
    }
}