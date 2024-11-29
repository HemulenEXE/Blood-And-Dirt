using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GunLogic;
using InteractiveObjects;
using Unity.VisualScripting;

namespace InventoryLogic
{
    /// <summary>
    /// Класс, реализующий "инвентарный слот".
    /// </summary>
    public class InventorySlot : MonoBehaviour
    {
        /// <summary>
        /// Название изображения пустого слота.
        /// </summary>
        public static string _emptySlotName = "cell";
        /// <summary>
        /// Изображение пустого слота.
        /// </summary>
        public static Sprite _emptySlotImage = null;
        /// <summary>
        /// Слой хранимого предмета до его поднятия.
        /// </summary>
        private int _pastLayerItem;
        /// <summary>
        /// Возвращает и изменяет иконку предмета, хранимого в слоте.<br/>
        /// Может равняться null.
        /// </summary>
        public Image ImageStoredItem { get; set; } = null;
        /// <summary>
        /// Возвращает и изменяет предмет, хранимый в слоте.<br/>
        /// Может равняться null.
        /// </summary>
        public ItemPickUp StoredItem { get; set; } = null;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void Awake()
        {
            _emptySlotImage = Resources.Load<Sprite>($"Sprites/Interface/{_emptySlotName}");
            if (_emptySlotImage == null) throw new ArgumentNullException("InventorySlot: _emptySlotImage is null");
        }
        /// <summary>
        /// Добавление указанного предмета в слот.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Push(ItemPickUp item)
        {
            if (!IsFull())
            {
                _pastLayerItem = item.gameObject.layer;
                item.gameObject.layer = LayerMask.NameToLayer("Player");
                item.Deactive();
                StoredItem = item;

                ImageStoredItem.sprite = item.Icon;

                //Если добавляемый объект является оружием - отобразить над ячейкой инвентаря кол-во патронов
                if (StoredItem?.GetComponent<IGun>() != null)
                {
                    IGun gun = StoredItem?.GetComponent<IGun>();
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
        public virtual void RemoveItem()
        {
            if (IsFull())
            {
                //Удаление счётчика патронов если хранимый предмет - оружие.
                if (StoredItem.GetComponent<IGun>() != null)
                    Destroy(ImageStoredItem.transform.GetChild(0).gameObject);

                StoredItem.gameObject.layer = _pastLayerItem;
                StoredItem.Active();
                StoredItem = null;
                ImageStoredItem.GetComponent<Image>().sprite = InventorySlot._emptySlotImage;

            }
        }
        /// <summary>
        /// Указывает, заполнен ли слот.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFull() => StoredItem != null;
    }
}