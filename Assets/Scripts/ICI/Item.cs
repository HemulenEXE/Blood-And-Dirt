using System;
using UnityEngine;

/// <summary>
/// Класс, реализующий "предметы, которые можно взять в инвентарь".
/// </summary>
public class Item : MonoBehaviour, IInteractable
{
    public string Name { get; }
    public string Description { get; set; }
    public Sprite Icon;
    public LayerMask Layer { get; private set; }
    public GameObject GameObject { get; }

    public virtual void Interact()
    {
        GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>().AddItem(this);
    }
    /// <summary>
    /// Более безопасный аналог метода SetActive(true).<br/>
    /// Необходимо для правильного управления полями и свойствами при активации предмета.
    /// </summary>
    public virtual void Active()
    {
        //this.gameObject.SetActive(true);
    }
    /// <summary>
    /// Более безопасный аналог метода SetActive(false).<br/>
    /// Необходимо для правильного управления полями и свойствами при деактивации предмета.
    /// </summary>
    public virtual void Deactive()
    {
        //this.gameObject.SetActive(false);
    }
    protected virtual void Awake()
    {
        Layer = this.gameObject.layer;
        if (Icon == null) throw new ArgumentNullException("PickUpItem: Icon is null");
    }
    protected virtual void Start()
    {
        Description = SettingData.Interact.ToString();
    }
}