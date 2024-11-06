using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс инвентаря игрока.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// Список слотов.
    /// </summary>
    public static List<IInventorySlot> _slots; //В список, в отличие от массива, эффективнее добавляются новые элементы
    /// <summary>
    /// Текущий, выбранный игроком слот.
    /// </summary>
    public static int _currSlot = 0;
    /// <summary>
    /// Максимальное число слотов.
    /// </summary>
    public int _maxCountSlots = 5;
    /// <summary>
    /// Минимальное число слотов.
    /// </summary>
    public int _minCountSlots = 3;
    /// <summary>
    /// Максимальный размер изображения слота.
    /// </summary>
    [SerializeField] private float _maxSizeSlot = 1f;
    /// <summary>
    /// Минимальный размер изображения слота.
    /// </summary>
    [SerializeField] private float _minSizeSlot = 0.8f;
    private void Awake()
    {
        //Проверка полей _minCountSlots и _maxCountSlots на корректность.
        if (_minCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minCountSlots < 0");
        if (_maxCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < 0");
        if (_maxCountSlots < _minCountSlots) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < _minCountSlots");

        //Проверка полей _minSizeSlot и _maxSizeSlot на корректность.
        if (_maxSizeSlot < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _maxSizeSlot < 0");
        if (_minSizeSlot < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minSizeSlot < 0");
        if (_maxSizeSlot < _minSizeSlot) throw new ArgumentOutOfRangeException("PlayerInventory: _maxSizeSlot < _minSizeSlot");

        //Настройка (заполнение) списка слотов.
        _slots = new List<IInventorySlot>(_maxCountSlots);
        for (int i = 0; i < _minCountSlots; i++)
            AddSlot();

        //По умолчанию, выбираем слот с нулевым индексом.
        SelectSlot(0);
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
            _slots[_currSlot].RemoveItem();
        }

        //Внедрение дополнительного слота.
        if (Input.GetKeyDown(KeyCode.V) && Input.GetKeyDown(KeyCode.LeftShift))
            AddSlot();
    }
    /// <summary>
    /// Перемещение на следующий слот (после текущего).
    /// </summary>
    public void SelectNextSlot()
    {
        //Настраиваем текущий слот.
        _slots[_currSlot].StoredItem?.SetActive(false);
        InventorySlot currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _minSizeSlot;

        //Перемещаемся на следующий слот.
        _currSlot = (_currSlot + 1) % _slots.Count;

        //Настраиваем текущий слот.
        _slots[_currSlot].StoredItem?.SetActive(true);
        currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _maxSizeSlot;


    }
    /// <summary>
    /// Перемещение на предыдущий слот (перед текущим).
    /// </summary>
    public void SelectPrevSlot()
    {
        //Настраиваем текущий слот.
        _slots[_currSlot].StoredItem?.SetActive(false);
        InventorySlot currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _minSizeSlot;

        //Перемещаемся на предыдущий слот.
        _currSlot = (_currSlot - 1 + _slots.Count) % _slots.Count;

        //Настраиваем текущий слот.
        _slots[_currSlot].StoredItem?.SetActive(true);
        currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _maxSizeSlot;
    }
    /// <summary>
    /// Перемещение на слот с индексом index.
    /// </summary>
    /// <param name="index"></param>
    public void SelectSlot(int index)
    {
        //Настраиваем текущий слот.
        _slots[_currSlot].StoredItem?.SetActive(false);
        InventorySlot currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _minSizeSlot;

        //Перемещаемся на слот с указанным индексом.
        _currSlot = index;

        //Настраиваем текущий слот.
        _slots[_currSlot].StoredItem?.SetActive(true);
        currentSlot = _slots[_currSlot] as InventorySlot;
        currentSlot.transform.localScale = Vector3.one * _maxSizeSlot;
    }
    /// <summary>
    /// Внедрение дополнительного слота.
    /// Общее количество слотов не может превышать _maxCountSlots
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
    /// <summary>
    /// Очищение выбранного слота.
    /// Производится сброс предмета в этом слоте.
    /// </summary>
    public void ClearSelectionSlot()
    {
        if (_slots[_currSlot].StoredItem != null)
        {
            _slots[_currSlot].StoredItem.gameObject.SetActive(true);
            _slots[_currSlot].RemoveItem(); //Сброс предмета
        }
    }
}
