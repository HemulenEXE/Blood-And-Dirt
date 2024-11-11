using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Класс, реализующий "предметы, которые можно поднять в инвентарь игрока".
/// </summary>
public class ItemPickUp : AbstractInteractiveObject
{
    public static string _tagTarget = "Player";
    /// <summary>
    /// Иконка предмета в инвентаре.
    /// </summary>
    [SerializeField] private Image _icon;
    public Image Icon
    {
        get => _icon;
    }
    private void Awake()
    {
        if (Icon == null) throw new ArgumentNullException("ItemPickUp: Icon is null");
        if (this.GetComponent<Collider2D>() == null) throw new ArgumentNullException("ItemPickUp: Collider2D is null");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag(_tagTarget))
        {
            if (Input.GetKeyDown(KeyCode.E))
                Interact();
        }
    }
    /// <summary>
    /// Взаимодействие с объектом.
    /// </summary>
    public override void Interact()
    {
        PlayerInventory._slots[PlayerInventory._currSlot].AddItem(this.gameObject);
    }
}
