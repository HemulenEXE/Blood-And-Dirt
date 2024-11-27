using InventoryLogic;

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
                case "smokeGrenade":
                    if (ConsumablesCounter.SmokeGrenadeCount < 5)
                    {
                        Destroy(this.gameObject);
                        ConsumablesCounter.SmokeGrenadeCount++;
                    }             
                    break;
                case "simpleGrenade":
                    if (ConsumablesCounter.SimpleGrenadeCount < 5)
                    { 
                        Destroy(this.gameObject);
                        ConsumablesCounter.SimpleGrenadeCount++; 
                    } 
                    break;
                case "firstAidKit":
                    if (ConsumablesCounter.FirstAidKitCount < 5)
                    {
                        Destroy(this.gameObject);
                        ConsumablesCounter.FirstAidKitCount++;
                    }
                    break;
                case "bandage":
                    if (ConsumablesCounter.BandageCount < 5)
                    {
                        Destroy(this.gameObject);
                        ConsumablesCounter.BandageCount++; 
                    }
                    break;
            }
        }
    }
}