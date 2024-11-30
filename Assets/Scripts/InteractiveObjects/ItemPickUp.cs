using UnityEngine;
using PlayerLogic;
using System;
using GunLogic;

namespace InteractiveObjects
{
    /// <summary>
    /// Класс, реализующий "предметы, которые можно взять в инвентарь".
    /// </summary>
    public class ItemPickUp : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// Возвращает компонент, отвечающий представление предмета в пространстве. 
        /// </summary>
        public Transform Transform { get; protected set; }
        /// <summary>
        /// Возвращает компонент, отвечающий за визуальное представление предмета.
        /// </summary>
        public Renderer Renderer { get; protected set; }
        /// <summary>
        /// Возвращает компонент, отвечающий за коллизию предмета.
        /// </summary>
        public Collider2D Collider { get; protected set; }
        /// <summary>
        /// Возвращает описание предмета.
        /// </summary>
        public string Description { get; set; } = "E";
        /// <summary>
        /// Иконка предмета в инвентаре.
        /// </summary>
        [SerializeField] private Sprite _icon;
        /// <summary>
        /// Возвращает иконку предмета в инвентаре.
        /// </summary>
        public Sprite Icon { get => _icon; }
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void Awake()
        {
            Transform = this.transform;
            Renderer = this.GetComponent<Renderer>();
            Collider = this.GetComponent<Collider2D>();

            if (Transform == null) throw new ArgumentNullException("ItemPickUp: Transform is null");
            if (Renderer == null) throw new ArgumentNullException("ItemPickUp: Renderer is null");
            if (Collider == null) throw new ArgumentNullException("ItemPickUp: Collider is null");
            if (Icon == null) throw new ArgumentNullException("ItemPickUp: Icon is null");
        }
        /// <summary>
        /// Взаимодействие с объектом.
        /// </summary>
        public virtual void Interact()
        {
            PlayerInventory._slots[PlayerInventory._currentSlot].Push(this); //Кладём предмет в слот инвентаря.
        }
        /// <summary>
        /// Более безопасный аналог метода SetActive(true).<br/>
        /// Необходимо для правильного управления полями и свойствами при активации предмета.
        /// </summary>
        public virtual void Active() => this.gameObject.SetActive(true);
        /// <summary>
        /// Более безопасный аналог метода SetActive(false).<br/>
        /// Необходимо для правильного управления полями и свойствами при деактивации предмета.
        /// </summary>
        public virtual void Deactive() => this.gameObject.SetActive(false);
    }
}