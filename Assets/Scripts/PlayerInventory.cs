using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс инвентаря игрока
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// Список инвентарных слотов игрока
    /// </summary>
    public static List<IInventorySlot> _slots; //В список, в отличие от массива, эффективнее добавляются новые элементы
    /// <summary>
    /// Текущий слот, выбранный игроком
    /// </summary>
    public static int _currSlot;
    /// <summary>
    /// Максимальное число слотов инвентаря
    /// </summary>
    public static int _maxCountSlots = 5;
    /// <summary>
    /// Минимальное число слотов инвентаря
    /// </summary>
    public static int _minCountSlots = 3;
    /// <summary>
    /// Максимальный размер слота
    /// </summary>
    public float _maxSizeSlot = 1f;
    /// <summary>
    /// Минимальный размер слота
    /// </summary>
    public float _minSizeSlot = 0.8f;
    private void Awake()
    {
        //Проверка на корректность данных _minCountSlots и _maxCountSlots
        if (_minCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minCountSlots < 0");
        if (_maxCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < 0");
        if (_maxCountSlots < _minCountSlots) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < _minCountSlots");
        
        //Проверка корректности 
        if (_maxSizeSlot < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _maxSizeSlot < 0");
        if (_minSizeSlot < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minSizeSlot < 0");
        if (_maxSizeSlot < _minSizeSlot) throw new ArgumentOutOfRangeException("PlayerInventory: _maxSizeSlot < _minSizeSlot");

        //Настройка списка слотов инвентаря игрока
        _slots = new List<IInventorySlot>(_maxCountSlots);
        for (int i = 0; i < _minCountSlots; i++)
        {
            AddSlot();
        }

        //По умолчанию, выбираем слот с нулевым индексом
        SelectSlot(0);
    }
    private void Update()
    {
        //Перемещение между слотами с помощью пронумерованных клавиш 1 2 3 ...
        for (int i = 49; i < 49 + _slots.Count; i++)
        {
            KeyCode key = (KeyCode)i;
            if (Input.GetKeyDown(key))
            {
                SelectSlot(i - 49);
            }
        }

        //Перемещение между слотами посредством компьютерной мыши
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SelectNextSlot();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SelectPrevSlot();
        }

        //Освобождение текущего слота
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _slots[_currSlot].RemoveItem();
        }

        //Добавление дополнительного слота
        if (Input.GetKeyDown(KeyCode.V))
            AddSlot();

    }
    /// <summary>
    /// Выбрать следующий слот после текущего
    /// </summary>
    public void SelectNextSlot()
    {
        InventorySlot currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _minSizeSlot;
        _currSlot = (_currSlot + 1) % _slots.Count;
        currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _maxSizeSlot;
    }
    /// <summary>
    /// Выбрать предыдущий слот перед текущий
    /// </summary>
    public void SelectPrevSlot()
    {
        InventorySlot currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _minSizeSlot;
        _currSlot = (_currSlot - 1 + _slots.Count) % _slots.Count;
        currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _maxSizeSlot;
    }
    /// <summary>
    /// Выбирает слот с указанным индексом index
    /// </summary>
    /// <param name="index"></param>
    public void SelectSlot(int index)
    {
        InventorySlot currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _minSizeSlot;
        _currSlot = index;
        currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _maxSizeSlot;
    }
    /// <summary>
    /// Добавление дополнительного слота
    /// </summary>
    public void AddSlot()
    {
        if (_slots.Count < _maxCountSlots)
        {
            //Создание нового слота
            GameObject slot = new GameObject($"Slot {_slots.Count}");
            slot.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.Find("InventoryUI"), false);
            UnityEngine.UI.Image image = slot.AddComponent<UnityEngine.UI.Image>();
            image.sprite = Resources.Load<Sprite>("Textures/EmptyInventorySlot");
            slot.transform.localScale = Vector3.one * _minSizeSlot;
            _slots.Add(slot.AddComponent<InventorySlot>());
        }
    }
}
