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
                    if (ConsumableCounter._smokeGrenadeCount < ConsumableCounter._maxCountSmokeGrenade)
                    {
                        Destroy(this.gameObject);
                        ++ConsumableCounter._smokeGrenadeCount;
                    }
                    break;
                case "SimpleGrenade":
                    if (ConsumableCounter._simpleGrenadeCount < ConsumableCounter._maxCountSimpleGrenade)
                    {
                        Destroy(this.gameObject);
                        ++ConsumableCounter._simpleGrenadeCount; 
                    }
                    break;
                case "firstAidKit":
                    if (ConsumableCounter._firstAidKitCount < ConsumableCounter._maxCountFirstAidKit)
                    {
                        Destroy(this.gameObject);
                        ++ConsumableCounter._firstAidKitCount;
                    }
                    break;
                case "bandage":
                    if (ConsumableCounter._bandageCount < ConsumableCounter._maxCountBandage)
                    {
                        Destroy(this.gameObject);
                        ++ConsumableCounter._bandageCount;
                    }
                    break;
            }
        }
    }
}