using UnityEngine;

/// <summary>
/// Интерфейс, поддерживающий слот инвентаря игрока.
/// </summary>
public interface IInventorySlot
{
    /// <summary>
    /// Предмет, хранимый в слоте.
    /// Может равняться null.
    /// </summary>
    public GameObject StoredItem { get; set; }
    /// <summary>
    /// Флаг, указывающий, заполнен ли слот.
    /// </summary>
    public bool IsFull { get; set; }
    /// <summary>
    /// Добавление предмета item в слот.
    /// При добавлении предмета поверх слота должна появляться иконка этого предмета.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item);
    /// <summary>
    /// Очищение слота. Хранимый предмет сбрасывается.
    /// При сбрасывании предмета его иконка, расположенная поверх слота, уничтожается.
    /// </summary>
    public void RemoveItem();
}
