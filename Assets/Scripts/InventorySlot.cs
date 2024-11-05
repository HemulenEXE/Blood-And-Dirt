using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Класс слота инвентаря игрока.
/// </summary>
public class InventorySlot : MonoBehaviour, IInventorySlot
{
    /// <summary>
    /// Предмет, хранимый в этом слоте.
    /// </summary>
    [SerializeField] private GameObject _storedItem;
    public GameObject StoredItem { get => _storedItem; set => _storedItem = value; }
    /// <summary>
    /// Изображение предмета, хранимого в данном слоте.
    /// </summary>
    private GameObject _imageStoredItem;
    public GameObject ImageStoredItem { get => _imageStoredItem; set => _imageStoredItem = value; }
    /// <summary>
    /// Заполненность этого слота.
    /// </summary>
    [SerializeField] private bool _isFull;
    public bool IsFull { get => _isFull; set => _isFull = value; }
    /// <summary>
    /// Добавление предмета item в данный слот.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item)
    {
        if (!IsFull)
        {
            FullSlotImage(item.GetComponent<ItemPickUp>().Icon.gameObject);
            item.SetActive(false);
            StoredItem = item;
            IsFull = true;
        }
    }
    /// <summary>
    /// Очищение (высвобождение) данного слота.
    /// </summary>
    public void RemoveItem()
    {
        if (IsFull)
        {
            ClearSlotImage();
            StoredItem.SetActive(true);
            StoredItem = null;
            IsFull = false;
        }
    }
    /// <summary>
    /// Установка иконки image поверх данного слота.
    /// Этот метод используется при вызове процедуры AddItem().
    /// </summary>
    /// <param name="image"></param>
    private void FullSlotImage(GameObject image)
    {
        if (ImageStoredItem == null)
        {
            ImageStoredItem = Instantiate(image, this.transform); //Установка иконки
        }
    }
    /// <summary>
    /// Иконка, расположенная поверх данного слота, уничтожается.
    /// Этот метод используется при вызове процедуры RemoveItem().
    /// </summary>
    private void ClearSlotImage()
    {
        if (ImageStoredItem != null)
        {
            Destroy(ImageStoredItem); //Удаление иконки.
            ImageStoredItem = null;
        }
    }
}
