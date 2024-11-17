using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractInventorySlot : MonoBehaviour
{
    private static Image _imageEmptySlot;
    /// <summary>
    /// Предмет, хранимый в слоте.<br/>
    /// Может равняться null.
    /// </summary>
    private UnvisibleItemPickUp _storedItem;
    /// <summary>
    /// Возвращает и изменяет предмет, хранимый в слоте.<br/>
    /// Может возвращать null.
    /// </summary>
    public UnvisibleItemPickUp StoredItem { get => _storedItem; set => _storedItem = value; }
    /// <summary>
    /// Иконка предмета, хранимого в слоте.<br/>
    /// Может равняться null.
    /// </summary>
    private Image _imageStoredItem;
    /// <summary>
    /// Возвращает и изменяет иконку предмета, хранимого в слоте.<br/>
    /// Может возвращать null.
    /// </summary>
    public Image ImageStoredItem { get => _imageStoredItem; set => _imageStoredItem = value; }
    /// <summary>
    /// Указывает, заполнен ли слот.
    /// </summary>
    /// <returns></returns>
    public bool IsFull() => StoredItem != null;
    /// <summary>
    /// Добавление предмета в слот.
    /// </summary>
    /// <param name="item"></param>
    public abstract void AddItem(UnvisibleItemPickUp item);
    /// <summary>
    /// Очищение слота и сброс хранимого предмета.
    /// </summary>
    public abstract void RemoveItem();
}
