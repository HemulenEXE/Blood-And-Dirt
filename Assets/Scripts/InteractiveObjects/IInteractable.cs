using UnityEngine;

namespace InteractiveObjects
{
    /// <summary>
    /// Интерфейс, поддерживающий "интерактивные объекты".
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Возвращает компонент, отвечающий представление объекта в пространстве.
        /// </summary>
        public Transform Transform { get; }
        /// <summary>
        /// Возвращает компонент, отвечающий за визуальное представление объекта.
        /// </summary>
        public Renderer Renderer { get; }
        /// <summary>
        /// Возвращает компонент, отвечающий за коллизию объекта.
        /// </summary>
        public Collider2D Collider { get; }
        /// <summary>
        /// Возвращает описание объекта.
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Взаимодействие с объектом.
        /// </summary>
        public void Interact();
    }
}
