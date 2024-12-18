using CameraLogic.CameraEffects;
using GunLogic;
using InteractiveObjects;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryLogic
{
    /// <summary>
    /// Класс, реализующий "инвентарь игрока".
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        //Поля.

        /// <summary>
        /// Единственный экземпляр инвентаря (одиночка).
        /// </summary>
        private static Inventory _instance;
        /// <summary>
        /// Название изображения пустого слота.
        /// </summary>
        [SerializeField] private string _emptySlotFileName = "Cell"; //Загрузка идёт из пути "Sprites/Interface/_emptySlotFileName"
        /// <summary>
        /// Изображение пустого слота.
        /// </summary>
        private Sprite _emptySlotImage = null;
        /// <summary>
        /// Список инвентарных слотов.
        /// </summary>
        private List<InventorySlot> _slots; //Вместо массива используется список для эффективного добавления новых слотов.
        /// <summary>
        /// Индекс текущего слота.
        /// </summary>
        private int _indexCurrentSlot = 0;
        /// <summary>
        /// Минимальное число слотов.
        /// </summary>
        [SerializeField] public int _minCountSlots = 3;
        /// <summary>
        /// Максимальное число слотов.
        /// </summary>
        [SerializeField] private int _maxCountSlots = 5;
        /// <summary>
        /// Размер слота.
        /// </summary>
        [SerializeField] private float _sizeSlot = 0.5f;

        //Свойства.

        /// <summary>
        /// Возвращает количество слотов.
        /// </summary>
        public int SlotCount
        {
            get
            {
                return _slots.Count;
            }
        }
        /// <summary>
        /// Возвращает объект текущего слота.
        /// </summary>
        public InventorySlot CurrentSlot
        {
            get
            {
                return _slots[IndexCurrentSlot];
            }
        }
        /// <summary>
        /// Возвращает индекс текущего слота.
        /// </summary>
        public int IndexCurrentSlot
        {
            get
            {
                return _indexCurrentSlot;
            }
            private set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Inventory: value < 0");
                if (value > SlotCount) throw new ArgumentOutOfRangeException("Inventory: value > SlotCount");
                _indexCurrentSlot = value;
            }
        }
        /// <summary>
        /// Возвращает единственный экземпляр инвентаря (объект одиночку).
        /// </summary>
        /// <returns></returns>
        public static Inventory GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<Inventory>();

                    if (_instance == null)
                    {
                        _instance = new GameObject("Inventory").AddComponent<Inventory>();
                    }
                }
                return _instance;
            }
        }

        //Встроенные методы.

        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Awake()
        {
            _emptySlotImage = Resources.Load<Sprite>($"Sprites/Interface/{_emptySlotFileName}");

            if (_emptySlotImage == null) throw new ArgumentNullException("Inventory: _emptySlotImage is null. Perhaps _emptySlotFileName is uncorrect.");
            if (_minCountSlots < 0) throw new ArgumentOutOfRangeException("Inventory: _minCountSlots < 0");
            if (_maxCountSlots < 0) throw new ArgumentOutOfRangeException("Inventory: _maxCountSlots < 0");
            if (_maxCountSlots < _minCountSlots) throw new ArgumentOutOfRangeException("Inventory: _maxCountSlots < _minCountSlots");
            if (_sizeSlot < 0) throw new ArgumentOutOfRangeException("Inventory: _sizeSlot < 0");

            _slots = new List<InventorySlot>();
            for (int i = 0; i < _minCountSlots; i++)
            {
                CreateSlot();
            }

            if (SlotCount > _maxCountSlots) throw new ArgumentOutOfRangeException("Inventory: SlotCount > _maxCountSlots");
            if (SlotCount < _minCountSlots) throw new ArgumentOutOfRangeException("Inventory: SlotCount < _minCountSlots");

            SelectSlot(0); //По умолчанию, выбираем нулевой слот.
        }

        //Вспомогательные методы.

        /// <summary>
        /// Перемещение на следующий слот.
        /// </summary>
        public void SelectNextSlot()
        {
            SelectSlot((IndexCurrentSlot + 1) % SlotCount);
        }
        /// <summary>
        /// Перемещение на предыдущий слот.
        /// </summary>
        public void SelectPreviousSlot()
        {
            SelectSlot((IndexCurrentSlot - 1 + SlotCount) % SlotCount);
        }
        /// <summary>
        /// Перемещение на слот с заданным индексом.
        /// </summary>
        /// <param name="index"></param>
        public void SelectSlot(int index)
        {
            //Настройка UI слота.
            CurrentSlot.StoredItem?.Deactive();
            CurrentSlot.gameObject.SetActive(false);

            IndexCurrentSlot = index;

            //Настройка UI слота.
            CurrentSlot.StoredItem?.Active();
            CurrentSlot.gameObject.SetActive(true);
        }
        /// <summary>
        /// Очищение слота с указанным индексом.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ClearSlot(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException("Inventory: index < 0");
            if (index > SlotCount) throw new ArgumentOutOfRangeException("Inventory: index > SlotCount");

            if (_slots[index].IsFull())
            {
                //Настройка UI слота.
                //Удаление счётчика патронов если хранимый предмет - оружие.
                if (_slots[index].StoredItem.GetComponent<IGun>() != null)
                    Destroy(_slots[index].ImageStoredItem.transform.GetChild(0).gameObject);
                _slots[index].ImageStoredItem.sprite = _emptySlotImage;

                _slots[index].RemoveItem();
            }
        }
        /// <summary>
        /// Очищение текущего слота.
        /// </summary>
        public void ClearCurrentSlot()
        {
            ClearSlot(IndexCurrentSlot);
        }
        /// <summary>
        /// Очистка всех слотов и расходников.
        /// </summary>
        public void Clear()
        {
            foreach (var slot in _slots)
            {
                //Destroy(slot?.StoredItem?.gameObject);
                slot.StoredItem = null;
                slot.RemoveItem();
                ConsumableCounter.SimpleGrenadeCount = 0;
                ConsumableCounter.SmokeGrenadeCount = 0;
                ConsumableCounter.FirstAidKitCount = 0;
                ConsumableCounter.BandageCount = 0;
            }
        }
        /// <summary>
        /// Добавление предмета в текущий слот инвентаря.
        /// </summary>
        /// <param name="item"></param>
        public bool PushItem(ItemPickUp item)
        {
            if (CurrentSlot.PushItem(item))
            {
                CurrentSlot.ImageStoredItem.sprite = item.Icon;

                //Если добавляемый объект является оружием - отобразить над ячейкой инвентаря кол-во патронов
                IGun gun = CurrentSlot.StoredItem?.GetComponent<IGun>();
                if (gun != null)
                {
                    //Настройка UI для оружия.
                    GameObject description = new GameObject("count", typeof(TextMeshProUGUI));
                    description.transform.SetParent(CurrentSlot.ImageStoredItem.transform, false);
                    TextMeshProUGUI txt = description.GetComponent<TextMeshProUGUI>();
                    txt.text = gun.AmmoTotalCurrent + "\\" + gun.AmmoTotal;
                    txt.font = Resources.Load<TMP_FontAsset>($"Fonts/PixelFont");
                    txt.fontSize = 30f;
                    txt.alignment = TextAlignmentOptions.Center;
                    Vector3 positionObject = CurrentSlot.ImageStoredItem.transform.position;
                    positionObject.y += CurrentSlot.ImageStoredItem.GetComponent<Image>().GetComponent<RectTransform>().rect.height / 4;
                    description.transform.position = positionObject;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Создание нового слота.<br/>
        /// Общее количество слотов не может превышать их максимальное число.
        /// </summary>
        public bool CreateSlot()
        {
            if (_slots.Count < _maxCountSlots)
            {
                GameObject newSlot = new GameObject($"Slot {_slots.Count}");
                InventorySlot inventorySlot = newSlot.AddComponent<InventorySlot>();
                _slots.Add(inventorySlot);
                newSlot.transform.SetParent(this.transform.Find("UIPanel")?.Find("Slots"), true); //Установление связи с канвасом.
                newSlot.transform.localScale = Vector3.one * _sizeSlot;
                inventorySlot.StoredItem = null;
                inventorySlot.ImageStoredItem = inventorySlot.gameObject.AddComponent<UnityEngine.UI.Image>();
                inventorySlot.ImageStoredItem.sprite = _emptySlotImage;
                if (SlotCount > 0) newSlot.SetActive(false);
                return true;
            }
            return false;
        }
    }
}