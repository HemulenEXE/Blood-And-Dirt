using System;
using UnityEngine;

namespace InteractiveObjects
{
    /// <summary>
    /// Класс, реализующий "интерактивные объекты, которые нельзя взять в инвентарь".
    /// </summary>
    public class ClickedObject : MonoBehaviour, IInteractable
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
        /// Возвращает компонент, отвечающий за коллизию объекта.
        /// </summary>
        public Collider2D Collider { get; protected set; }
        /// <summary>
        /// Возвращает описание объекта.
        /// </summary>
        public string Description { get; set; } = "E";
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void Awake()
        {
            Transform = this.transform;
            Renderer = this.GetComponent<Renderer>();
            Collider = this.GetComponent<Collider2D>();

            if (Transform == null) throw new ArgumentNullException("ClickedObject: Transform is null");
            if (Renderer == null) throw new ArgumentNullException("ClickedObject: Renderer is null");
            if (Collider == null) throw new ArgumentNullException("ClickedObject: Collider is null");
        }
        /// <summary>
        /// Взаимодействие с объектом.
        /// </summary>
        public virtual void Interact()
        {
            //Логика взаимодействия.
            Debug.Log("ClickedObject");
        }
    }
}