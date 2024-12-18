using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GunLogic;
using InteractiveObjects;

namespace InventoryLogic
{
    /// <summary>
    /// Класс, реализующий "инвентарный слот".
    /// </summary>
    public class InventorySlot : MonoBehaviour
    {
        //Поля.

        /// <summary>
        /// Слой хранимого предмета (до его поднятия).
        /// </summary>
        private int _pastLayerItem;

        //Свойства.

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

        //Вспомогательные методы.

        /// <summary>
        /// Добавление подаваемого предмета в слот.
        /// </summary>
        /// <param name="item"></param>
        public bool PushItem(ItemPickUp item)
        {
            if (!IsFull())
            {
                _pastLayerItem = item.gameObject.layer;
                //item.gameObject.layer = LayerMask.NameToLayer("GunGrenade");
                item.Deactive();
                StoredItem = item;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Очищение слота.<br/>
        /// Производится сброс хранимого предмета.
        /// </summary>
        public bool RemoveItem()
        {
            if (IsFull())
            {
                StoredItem.gameObject.layer = _pastLayerItem;
                StoredItem.Active();
                StoredItem = null;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Указывает, заполнен ли слот.
        /// </summary>
        /// <returns></returns>
        public bool IsFull() => StoredItem != null;
    }
}