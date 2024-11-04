using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс инвентаря игрока
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// Список инвентарных слотов игрока
    /// </summary>
    public static List<GameObject> _slots; //В список, в отличие от массива, эффективнее добавляются новые элементы
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
    private void Start()
    {
        //Настройка списка инвентарных слотов игрока
        _slots = new List<GameObject>(_maxCountSlots);
        for (int i = 0; i < _minCountSlots; i++)
        {
            GameObject slot = new GameObject($"Slot {i}");
            slot.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.Find("InventoryUI"), false);
            UnityEngine.UI.Image image = slot.AddComponent<UnityEngine.UI.Image>();
            image.sprite = Resources.Load<Sprite>("Textures/EmptyInventorySlot");
            _slots.Add(slot);
        }
        SelectSlot(0);
    }

    private void Update()
    {
        //Перемещение между слотами посредством пронумерованных клавиш 1 2 3 ...
        for(int i = 49; i <= 49 + _slots.Count; i++)
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

    }
    /// <summary>
    /// Выбрать следующий слот после текущего
    /// </summary>
    public void SelectNextSlot()
    {
        _slots[_currSlot].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        _currSlot = (_currSlot + 1) % _slots.Count;
        _slots[_currSlot].GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }
    /// <summary>
    /// Выбрать предыдущий слот перед текущий
    /// </summary>
    public void SelectPrevSlot()
    {
        _slots[_currSlot].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        _currSlot = (_currSlot - 1 + _slots.Count) % _slots.Count;
        _slots[_currSlot].GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }
    /// <summary>
    /// Выбирает слот с указанным индексом index
    /// </summary>
    /// <param name="index"></param>
    public void SelectSlot(int index)
    {
        _slots[_currSlot].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        _currSlot = index;
        _slots[_currSlot].GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }

    /// <summary>
    /// Добавление инвентарного слота
    /// </summary>
    public void AddSlot()
    {
        if (_slots.Count < _maxCountSlots)
            _slots.Add(new GameObject($"Slot {_slots.Count}"));
    }
}
