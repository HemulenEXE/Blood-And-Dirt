using UnityEngine;

/// <summary>
/// Интерфейс, реализующий слот инвентаря игрока
/// </summary>
public interface IInventorySlot
{
    /// <summary>
    /// Предмет, хранимый в данном слоте
    /// </summary>
    public GameObject StoredItem { get; set; }
    /// <summary>
    /// Заполненность слота
    /// </summary>
    public bool IsFull { get; set; }
    /// <summary>
    /// Добавление предмета item в данный слот
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item);
    /// <summary>
    /// Высвобождает (очищает) этот слот
    /// </summary>
    public void RemoveItem();
}
