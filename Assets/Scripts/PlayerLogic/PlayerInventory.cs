using System;
using System.Collections.Generic;
using UnityEngine;
using InventoryLogic;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "управление инвентарём игрока".
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        private void Update()
        {
            //Перемещение между слотами с помощью пронумерованных клавиш 1 2 3 ...
            for (int i = 49; i < 49 + Inventory.GetInstance.SlotCount; i++)
            {
                KeyCode key = (KeyCode)i;
                if (Input.GetKey(key))
                {
                    Inventory.GetInstance.SelectSlot(i - 49);
                }
            }
            //Перемещение между слотами посредством колёсика компьютерной мыши.
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Inventory.GetInstance.SelectNextSlot();
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Inventory.GetInstance.SelectPreviousSlot();
            }
            //Освобождение выбранного слота.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Inventory.GetInstance.ClearCurrentSlot();
            }
        }
    }
}