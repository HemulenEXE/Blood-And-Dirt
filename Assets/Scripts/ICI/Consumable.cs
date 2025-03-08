/// <summary>
/// Класс, реализующий "расходник".<br/>
/// Этот скрипт позволяет забирать расходники (аптечку, бинты, гранаты) со сцены и изменяет показатель их количества.
/// </summary>
public class Consumable : ClickedObject
{
    public override void Interact()
    {
        switch (this.tag)
        {
            case "SmokeGrenade":
                if (PlayerData.SmokeGrenadeCount < PlayerData.MaxSmokeGrenadeCount)
                {
                    Destroy(this.gameObject);
                    ++PlayerData.SmokeGrenadeCount;
                }
                break;
            case "SimpleGrenade":
                if (PlayerData.SimpleGrenadeCount < PlayerData.MaxSimpleGrenadeCount)
                {
                    Destroy(this.gameObject);
                    ++PlayerData.SimpleGrenadeCount;
                }
                break;
            case "firstAidKit":
                if (PlayerData.FirstAidKitCount < PlayerData.MaxFirstAidKitCount)
                {
                    Destroy(this.gameObject);
                    ++PlayerData.FirstAidKitCount;
                }
                break;
            case "bandage":
                if (PlayerData.BandageCount < PlayerData.MaxFirstAidKitCount)
                {
                    Destroy(this.gameObject);
                    ++PlayerData.BandageCount;
                }
                break;
        }
    }
}