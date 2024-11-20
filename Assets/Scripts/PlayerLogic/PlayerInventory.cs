using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, реализующий "инвентарь игрока".
/// </summary>
public class PlayerInventory : MonoBehaviour
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
    /// Список инвентарных слотов.
    /// </summary>
    public static List<AbstractInventorySlot> _slots;
    /// <summary>
    /// Выбранный слот.<br/>
    /// Нумерация идёт с нуля.
    /// </summary>
    public static int _currentSlot = 0;
    /// <summary>
    /// Минимальное число слотов.
    /// </summary>
    public int _minCountSlots = 3;
    /// <summary>
    /// Максимальное число слотов.
    /// </summary>
    public int _maxCountSlots = 5;
    /// <summary>
    /// Размер изображения слота.
    /// </summary>
    [SerializeField] private float _sizeSlot = 0.5f;
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void Awake()
    {
        //Проверка полей, настраиваемых в редакторе Unity.
        if (_minCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minCountSlots < 0");
        if (_maxCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < 0");
        if (_maxCountSlots < _minCountSlots) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < _minCountSlots");

        if (_sizeSlot < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minSizeSlot < 0");

        //Установление значений ненастраиваемых полей.
        _emptySlotImage = Resources.Load<Sprite>($"Sprites/Interface/{_emptySlotName}");

        _slots = new List<AbstractInventorySlot>(_maxCountSlots);
        for (int i = 0; i < _minCountSlots; i++)
            AddSlot();

        SelectSlot(0); //По умолчанию, выбираем нулевой слот.
    }
    private void Update()
    {
        //Перемещение между слотами с помощью пронумерованных клавиш 1 2 3 ...
        for (int i = 49; i < 49 + _slots.Count; i++)
        {
            KeyCode key = (KeyCode)i;
            if (Input.GetKey(key))
            {
                SelectSlot(i - 49);
            }
        }

        //Перемещение между слотами посредством колёсика компьютерной мыши.
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SelectNextSlot();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SelectPrevSlot();
        }

        //Освобождение выбранного слота.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _slots[_currentSlot].RemoveItem();
        }

        //Внедрение дополнительного слота.
        if (Input.GetKeyDown(KeyCode.P))
            AddSlot();
    }
    /// <summary>
    /// Добавление нового, дополнительного, слота.<br/>
    /// Общее количество слотов не может превышать _maxCountSlots.
    /// </summary>
    public void AddSlot()
    {
        if (_slots.Count < _maxCountSlots)
        {
            //Создание нового слота
            GameObject interim_slot = new GameObject($"Slot {_slots.Count}");
            AbstractInventorySlot interim_abstract_slot = interim_slot.AddComponent<InventorySlot>();
            interim_slot.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.Find("ConsumablesUI").Find("InventoryUI"), true); //Установление связи с канвасом.
            interim_slot.transform.localScale = Vector3.one * _sizeSlot;
            interim_abstract_slot.StoredItem = null;
            interim_abstract_slot.ImageStoredItem = interim_slot.AddComponent<UnityEngine.UI.Image>();
            interim_abstract_slot.ImageStoredItem.sprite = _emptySlotImage;
            if (_slots.Count > 0)
                interim_abstract_slot.gameObject.SetActive(false);

            _slots.Add(interim_abstract_slot);
        }
    }
    /// <summary>
    /// Перемещение на следующий слот (после текущего).
    /// </summary>
    public void SelectNextSlot()
    {
        //Перемещение на следующий слот.
        SelectSlot((_currentSlot + 1) % _slots.Count);
    }
    /// <summary>
    /// Перемещение на предыдущий слот (перед текущим).
    /// </summary>
    public void SelectPrevSlot()
    {
        SelectSlot((_currentSlot - 1 + _slots.Count) % _slots.Count);
    }
    /// <summary>
    /// Перемещение на слот с индексом index.
    /// </summary>
    /// <param name="index"></param>
    public void SelectSlot(int index)
    {
        //Настройка текущего слота.
        _slots[_currentSlot].StoredItem?.Deactive();
        _slots[_currentSlot].gameObject.SetActive(false);

        //Перемещаемся на слот с указанным индексом.
        _currentSlot = index;

        //Настройка текущего слота.
        _slots[_currentSlot].StoredItem?.Active();
        _slots[_currentSlot].gameObject.SetActive(true);
    }
}
