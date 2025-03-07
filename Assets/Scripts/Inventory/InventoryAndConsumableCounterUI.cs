using GunLogic;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAndConsumableCounterUI : MonoBehaviour
{
    public List<GameObject> slots;
    public Sprite emptySlotIcon;
    public Inventory inventory;
    public TextMeshProUGUI description; // Панелька для описания взятых в руку предметов

    public TextMeshProUGUI smokeGrenade;
    public TextMeshProUGUI simpleGrenade;
    public TextMeshProUGUI firstAidKit;
    public TextMeshProUGUI bandage;

    public int IndexCurrentSlot { get; private set; } = 0;
    public int Size { get { return slots.Count; } }

    public void SelectNextSlot()
    {
        SelectSlot((IndexCurrentSlot + 1) % PlayerData.InventoryCapacity);
    }
    public void SelectPreviousSlot()
    {
        SelectSlot((IndexCurrentSlot - 1 + PlayerData.InventoryCapacity) % PlayerData.InventoryCapacity);
    }
    public void SelectSlot(int index)
    {
        var temp = inventory.GetItem(IndexCurrentSlot);
        temp?.Deactive();
        temp?.gameObject?.SetActive(false);
        slots[IndexCurrentSlot].SetActive(false);

        IndexCurrentSlot = index;

        slots[IndexCurrentSlot].SetActive(true);
        temp = inventory.GetItem(IndexCurrentSlot);
        temp?.Active();
        temp?.gameObject?.SetActive(true);
    }
    public bool AddItem(Item item)
    {
        if (inventory.AddItem(item, IndexCurrentSlot))
        {
            slots[IndexCurrentSlot].GetComponent<Image>().sprite = item.Icon;
            item.Active();
            return true;
        }
        return false;
    }
    public void RemoveItem()
    {
        inventory.GetItem(IndexCurrentSlot)?.Deactive();
        inventory.RemoveItem(IndexCurrentSlot);
        slots[IndexCurrentSlot].GetComponent<Image>().sprite = emptySlotIcon;
    }
    public Item GetItem()
    {
        return inventory.GetItem(IndexCurrentSlot);
    }
    private void Start()
    {
        if (smokeGrenade == null) throw new ArgumentNullException("ConsumableCounter: smokeGrenadeIcon doesn't have TextMeshProUGUI");
        if (simpleGrenade == null) throw new ArgumentNullException("ConsumableCounter: simpleGrenadeIcon doesn't have TextMeshProUGUI");
        if (firstAidKit.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("ConsumableCounter: firstAidKitIcon doesn't have TextMeshProUGUI");
        if (bandage.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("ConsumableCounter: bandageIcon doesn't have TextMeshProUGUI");
    }
    private void FixedUpdate()
    {
        IGun gun = GetItem()?.gameObject?.GetComponent<IGun>();
        if (gun != null) description.text = $"{gun.AmmoTotalCurrent} \\ {gun.AmmoTotal}";
        else description.text = "";

        smokeGrenade.text = PlayerData.SmokeGrenadeCount.ToString();
        simpleGrenade.text = PlayerData.SimpleGrenadeCount.ToString();
        firstAidKit.text = PlayerData.FirstAidKitCount.ToString();
        bandage.text = PlayerData.BandageCount.ToString();
    }
}
