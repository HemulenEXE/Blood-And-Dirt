using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Класс слота инвентаря игрока.
/// </summary>
public class InventorySlot : MonoBehaviour, IInventorySlot
{
    /// <summary>
    /// Предмет, хранимый в слоте.
    /// </summary>
    [SerializeField] private GameObject _storedItem;
    public GameObject StoredItem { get => _storedItem; set => _storedItem = value; }
    /// <summary>
    /// Иконка предмета, хранимого в слоте.
    /// </summary>
    private GameObject _imageStoredItem;
    public GameObject ImageStoredItem { get => _imageStoredItem; set => _imageStoredItem = value; }
    /// <summary>
    /// Флаг, указывающий, заполнен ли слот.
    /// </summary>
    [SerializeField] private bool _isFull;
    public bool IsFull { get => _isFull; set => _isFull = value; }
    /// <summary>
    /// Добавление предмета item в слот.
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
    /// Очистка слота.
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
    /// Установка иконки image поверх слота.
    /// Этот метод используется при вызове процедуры AddItem().
    /// </summary>
    /// <param name="image"></param>
    private void FullSlotImage(GameObject image)
    {
        if (ImageStoredItem == null)
        {
            ImageStoredItem = Instantiate(image, this.transform); //Установка иконки.
        }
    }
    /// <summary>
    /// Уничтожение иконки, расположенной поверх слота.
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
