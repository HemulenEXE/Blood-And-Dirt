using UnityEngine;

/// <summary>
/// Интерфейс, поддерживающий слот инвентаря игрока.
/// </summary>
public interface IInventorySlot
{
    /// <summary>
    /// Предмет, хранимый в данном слоте.
    /// Может равняться null.
    /// </summary>
    public GameObject StoredItem { get; set; }
    /// <summary>
    /// Заполненность слота.
    /// </summary>
    public bool IsFull { get; set; }
    /// <summary>
    /// Добавление предмета item в данный слот.
    /// При добавлении предмета поверх слота должна появляться иконка этого предмета.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item);
    /// <summary>
    /// Высвобождает (очищает) этот слот. Хранимый предмет сбрасывается.
    /// При сбрасывании предмета его иконка, расположенная поверх слота, уничтожается.
    /// </summary>
    public void RemoveItem();
}
