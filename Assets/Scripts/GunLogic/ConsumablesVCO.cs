using TMPro;
using UnityEngine;

namespace InteractiveObjects{
    /// <summary>
    /// Класс, реализующий "расходники".<br/>
    /// Этот скрипт позволяет забирать расходники (аптечку, бинты, гранаты) со сцены и изменяет показатель их количества.
    /// </summary>
    public class ConsumablesVCO : ClickedObject
    {
        /// <summary>
        /// Взаимодействие с расходником.
        /// </summary>
        public override void Interact()
        {
            switch (this.tag)
            {
                case "firstAidKit":
                    if (Consumables.firstAidKitCount < 5)
                    {
                        Destroy(this.gameObject);
                        Consumables.firstAidKitCount++;
                    }             
                    break;
                case "simpleGraned":
                    if (Consumables.simpleGrenadeCount < 5)
                    { 
                        Destroy(this.gameObject);
                        Consumables.simpleGrenadeCount++; 
                    } 
                    break;
                case "smokeGraned":
                    if (Consumables.firstAidKitCount < 5)
                    {
                        Destroy(this.gameObject);
                        Consumables.smokeGrenadeCount++;
                    }
                    break;
                case "bandage":
                    if (Consumables.bandageCount < 5)
                    {
                        Destroy(this.gameObject);
                        Consumables.bandageCount++; 
                    }
                    break;
            }
        }
    }
}