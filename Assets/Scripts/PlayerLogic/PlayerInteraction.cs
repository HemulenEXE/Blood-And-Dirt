using System;
using TMPro;
using UnityEngine;
using InteractiveObjects;
using InteractiveLogic;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "управление взаимодействия игрока с интерактивными объектами".
    /// </summary>
    public class PlayerInteract : MonoBehaviour
    {
        //Поля.

        /// <summary>
        /// Дистанция взаимодействия с объектами.<br/>
        /// Не может быть отрицательной.
        /// </summary>
        public float _interactionDistance = 1f;
        /// <summary>
        /// Кнопка взаимодействия.
        /// </summary>
        public static KeyCode _key = KeyCode.E;
        /// <summary>
        /// Объект, с которым на текущий момент взаимодействует игрок.<br/>
        /// Может равняться null.
        /// </summary>
        private IInteractable _currentInteractiveObject;
        /// <summary>
        /// Игнорируемый слой.
        /// </summary>
        [SerializeField] private LayerMask _ignoreLayer;

        //Встроенные методы.

        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Awake()
        {
            if (_interactionDistance < 0) throw new ArgumentOutOfRangeException("PlayerInteract: _interactionDistance < 0");
        }
        void Update()
        {
            Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
            Debug.DrawRay(ray.origin, ray.direction * _interactionDistance, Color.red); //Рисовка луча.

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _interactionDistance, ~_ignoreLayer);
            if (hit.collider != null) //Проверка на пересечение с каким-либо объектом, содержащим Collider2D.
            {
                _currentInteractiveObject = hit.transform.GetComponent<IInteractable>();

                Interactive.GetInstance.TurnOnText(_currentInteractiveObject);
            }
            else
            {
                _currentInteractiveObject = null;
                Interactive.GetInstance.TurnOffText();
            }
            if (Input.GetKeyDown(_key))
            {
                _currentInteractiveObject?.Interact();
            }
        }
    }
}
