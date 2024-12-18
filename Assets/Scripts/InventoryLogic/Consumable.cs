using InventoryLogic;
using UnityEngine;

namespace InteractiveObjects{
    /// <summary>
    /// Класс, реализующий "расходник".<br/>
    /// Этот скрипт позволяет забирать расходники (аптечку, бинты, гранаты) со сцены и изменяет показатель их количества.
    /// </summary>
    public class Consumable : ClickedObject
    {
        /// <summary>
        /// Взаимодействие с расходником.
        /// </summary>
        public override void Interact()
        {

            switch (this.tag)
            {
                case "SmokeGrenade":
                    if (ConsumableCounter.SmokeGrenadeCount < 5)
                    {
                        Destroy(this.gameObject);
                        ConsumableCounter.SmokeGrenadeCount++;
                    }             
                    break;
                case "SimpleGrenade":
                    if (ConsumableCounter.SimpleGrenadeCount < 5)
                    { 
                        Destroy(this.gameObject);
                        ConsumableCounter.SimpleGrenadeCount++; 
                    } 
                    break;
                case "firstAidKit":
                    if (ConsumableCounter.FirstAidKitCount < 5)
                    {
                        Destroy(this.gameObject);
                        ConsumableCounter.FirstAidKitCount++;
                    }
                    break;
                case "bandage":
                    if (ConsumableCounter.BandageCount < 5)
                    {
                        Destroy(this.gameObject);
                        ConsumableCounter.BandageCount++; 
                    }
                    break;
            }
        }
    }
}