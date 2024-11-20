using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, реализующий "инвентарный слот".
/// </summary>
public class InventorySlot : AbstractInventorySlot
{
    /// <summary>
    /// Добавление предмета в слот.
    /// </summary>
    /// <param name="item"></param>
    public override void AddItem(UnvisibleItemPickUp item)
    {
        if (!IsFull())
        {
            item.InHand = true;
            item.Deactive();
            StoredItem = item;

            ImageStoredItem.GetComponent<Image>().sprite = item.Icon;

            //Если добавляемый объект является оружием - отобразить над ячейкой инвентаря кол-во патронов
            if (item.GetComponent<IGun>() != null)
            {
                IGun gun = item.GetComponent<IGun>();
                GameObject description = new GameObject("count", typeof(TextMeshProUGUI));
                description.transform.SetParent(ImageStoredItem.transform, false);
                TextMeshProUGUI txt = description.GetComponent<TextMeshProUGUI>();
                txt.text = gun.AmmoTotalCurrent + "\\" + gun.AmmoTotal;
                txt.font = Resources.Load<TMP_FontAsset>($"Fonts/PixelFont");
                txt.fontSize = 30f;
                txt.alignment = TextAlignmentOptions.Center;
                
                Vector3 positionObject = ImageStoredItem.transform.position;
                positionObject.y += ImageStoredItem.GetComponent<Image>().GetComponent<RectTransform>().rect.height / 4; 
                description.transform.position = positionObject;
            }
        }
    }
    /// <summary>
    /// Очищение слота.<br/>
    /// Производится сброс хранимого предмета.
    /// </summary>
    public override void RemoveItem()
    {
        if (IsFull())
        {
            //Удаление счётчика патронов если хранимый предмет - оружие
            if (StoredItem.GetComponent<IGun>() != null)
                Destroy(ImageStoredItem.transform.GetChild(0).gameObject);

            StoredItem.InHand = false;
            StoredItem.Active();
            StoredItem = null;
            ImageStoredItem.GetComponent<Image>().sprite = PlayerInventory._emptySlotImage;
            
        }
    }
}
