using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAndConsumableCounterUI : MonoBehaviour
{
    public List<GameObject> slots;
    public Sprite emptySlotIcon;
    public Inventory inventory;
    public TextMeshProUGUI description;

    public TextMeshProUGUI smokeGrenade;
    public TextMeshProUGUI simpleGrenade;
    public TextMeshProUGUI firstAidKit;
    public TextMeshProUGUI bandage;

    public int IndexCurrentSlot { get; private set; } = 0;
    public int Size { get { return slots.Count; } }

    public static event Action<Transform, string> AudioEvent;

    public void AddSlot()
    {
        GameObject new_slot = new GameObject($"Slot {Size}");
        new_slot.transform.position = slots.Last().transform.position;
        new_slot.gameObject.GetComponent<Image>().sprite = emptySlotIcon;
        slots.Add(new_slot);
        inventory.items.Add(null);
        new_slot.transform.SetParent(this.transform.Find("Slots"));
    }
    public void SelectNextSlot()
    {
        int nextIndex = (IndexCurrentSlot + 1) % Size;
        while (nextIndex != IndexCurrentSlot)
        {
            if (inventory.GetItem(nextIndex) != null)
            {
                SelectSlot(nextIndex);
                return;
            }
            nextIndex = (nextIndex + 1) % Size;
        }
    }
    public void SelectPreviousSlot()
    {
        int prevIndex = (IndexCurrentSlot - 1 + Size) % Size;
        while (prevIndex != IndexCurrentSlot)
        {
            if (inventory.GetItem(prevIndex) != null)
            {
                SelectSlot(prevIndex);
                return;
            }
            prevIndex = (prevIndex - 1 + Size) % Size;
        }
    }
    public void SelectSlot(int index)
    {
        if (index == IndexCurrentSlot) return;

        var temp = inventory.GetItem(IndexCurrentSlot);
        temp?.Deactive();
        temp?.gameObject?.SetActive(false);
        slots[IndexCurrentSlot].SetActive(false);

        IndexCurrentSlot = index;

        slots[IndexCurrentSlot].SetActive(true);
        temp = inventory.GetItem(IndexCurrentSlot);
        temp?.gameObject?.SetActive(true);
        temp?.Active();
    }
    public bool AddItem(Item item)
    {
        for(int i = 0; i < slots.Count; ++i)
        {
            if (inventory.AddItem(item, i))
            {
                slots[i].GetComponent<Image>().sprite = item.Icon;
                item.Active();
                item.gameObject.layer = LayerMask.NameToLayer("Invisible");
                SelectSlot(i);
                return true;
            }
        }
        return false;
    }
    public void RemoveItem()
    {
        var temp = inventory.GetItem(IndexCurrentSlot);
        if (temp != null)
        {
            temp.Deactive();
            temp.gameObject.layer = temp.Layer;
            inventory.RemoveItem(IndexCurrentSlot);
            slots[IndexCurrentSlot].GetComponent<Image>().sprite = emptySlotIcon;
            int nextIndex = (IndexCurrentSlot + 1) % Size;

            while (nextIndex != IndexCurrentSlot)
            {
                if (inventory.GetItem(nextIndex) != null)
                {
                    SelectSlot(nextIndex);
                    return;
                }
                nextIndex = (nextIndex + 1) % Size;
            }

        }
    }
    public Item GetItem()
    {
        return inventory.GetItem(IndexCurrentSlot);
    }
    private void Start()
    {
        emptySlotIcon = Resources.Load<Sprite>("Sprites/Interface/cell");

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
