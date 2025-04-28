using UnityEngine;
using System;

public class InventoryDistribution : MonoBehaviour
{
    [SerializeField]
    private int BandageStartCount = 0;
    [SerializeField]
    private int FirstAidKitStartCount = 0;
    [SerializeField]
    private int ShrapnelGrenadeStartCount = 0;
    [SerializeField]
    private int SmokeGrenadeStartCount = 0;

    [SerializeField]
    private bool HasKnife = false;
    [SerializeField]
    private bool HasMachineGun = false;
    [SerializeField]
    private bool HasShotGun = false;
    [SerializeField]
    private bool HasPistol = false;
    [SerializeField]
    private bool HasGrenadeLauncher = false;

    [SerializeField]
    private GameObject Knife;
    [SerializeField]
    private GameObject MachineGun;
    [SerializeField]
    private GameObject ShotGun;
    [SerializeField]
    private GameObject Pistol;
    [SerializeField]
    private GameObject GrenadeLauncher;


    private void Start()
    {
        if (BandageStartCount < 0 || BandageStartCount > PlayerData.MaxBandageCount)
            throw new ArgumentOutOfRangeException("BandageStartCount < 0 || BandageStartCount > PlayerData.MaxBandageCount");
        if (FirstAidKitStartCount < 0 || FirstAidKitStartCount > PlayerData.MaxFirstAidKitCount)
            throw new ArgumentOutOfRangeException("FirstAidKitStartCount < 0 || FirstAidKitStartCount > PlayerData.MaxFirstAidKitCount");
        if (ShrapnelGrenadeStartCount < 0 || ShrapnelGrenadeStartCount > PlayerData.MaxSimpleGrenadeCount)
            throw new ArgumentOutOfRangeException("ShrapnelGrenadeStartCount < 0 || ShrapnelGrenadeStartCount > PlayerData.MaxSimpleGrenadeCount");
        if (SmokeGrenadeStartCount < 0 || SmokeGrenadeStartCount > PlayerData.SmokeGrenadeCount)
            throw new ArgumentOutOfRangeException("SmokeGrenadeStartCount < 0 || SmokeGrenadeStartCount > PlayerData.SmokeGrenadeCount");

        PlayerData.BandageCount = BandageStartCount;
        PlayerData.FirstAidKitCount = FirstAidKitStartCount;
        PlayerData.SimpleGrenadeCount = ShrapnelGrenadeStartCount;
        PlayerData.SmokeGrenadeCount = SmokeGrenadeStartCount;

        var iac = GameObject.Find("InventoryAndConsumableCounterUI").GetComponent<InventoryAndConsumableCounterUI>();
        if (HasKnife) iac.AddItem(GameObject.Instantiate(Knife).GetComponent<Item>());
        if (HasMachineGun) iac.AddItem(GameObject.Instantiate(MachineGun).GetComponent<Item>());
        if (HasShotGun) iac.AddItem(GameObject.Instantiate(ShotGun).GetComponent<Item>());
        if (HasPistol) iac.AddItem(GameObject.Instantiate(Pistol).GetComponent<Item>());
        if (HasGrenadeLauncher) iac.AddItem(GameObject.Instantiate(GrenadeLauncher).GetComponent<Item>());
    }
}
