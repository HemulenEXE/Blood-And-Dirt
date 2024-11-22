using System;
using TMPro;
using UnityEngine;
using InteractiveObjects;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "управление взаимодействия игрока с интерактивными объектами".
    /// </summary>
    public class PlayerInteract : MonoBehaviour
    {
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
        /// Вертикальное смещение интерактивного текста.<br/>
        /// Может принимать отрицательные значения.
        /// </summary>
        public float OffSet = 0.5f;
        /// <summary>
        /// Интерактивный текст.<br/>
        /// Содержится на канвасе.<br/>
        /// Не может равняться null.
        /// </summary>
        public GameObject _interactionText;
        /// <summary>
        /// Объект, с которым на текущий момент взаимодействует игрок.<br/>
        /// Может равняться null.
        /// </summary>
        private IInteractable _currentInteractiveObject;
        /// <summary>
        /// Игнорируемый слой.
        /// </summary>
        [SerializeField] private LayerMask _ignoreLayer;
        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Awake()
        {
            if (_interactionDistance < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _interactionDistance < 0");
        }
        void Update()
        {
            Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
            Debug.DrawRay(ray.origin, ray.direction * _interactionDistance, Color.red); //Рисовка луча.

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _interactionDistance, ~_ignoreLayer);
            if (hit.collider != null) //Проверка на пересечение с каким-либо объектом, содержащим Collider2D.
            {
                IInteractable interim_interactive_object = hit.transform.GetComponent<IInteractable>();
                if (interim_interactive_object != null)
                {
                    _interactionText.SetActive(true);
                    _currentInteractiveObject = interim_interactive_object;
                    TextMeshProUGUI interim_textMeshProUGUI = _interactionText.GetComponent<TextMeshProUGUI>();
                    if (interim_textMeshProUGUI != null)
                    {
                        //Корректировка интерактивного текста.
                        interim_textMeshProUGUI.text = _currentInteractiveObject.Description;
                        Vector3 positionObject = _currentInteractiveObject.Transform.position;
                        positionObject.y = _currentInteractiveObject.Renderer.bounds.max.y + OffSet; //Получение верхней границы визуального представления объекта.
                        positionObject.x = (_currentInteractiveObject.Renderer.bounds.max.x + _currentInteractiveObject.Renderer.bounds.min.x) / 2;
                        Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(Camera.main, positionObject);
                        interim_textMeshProUGUI.transform.position = positionInWorld;
                    }
                }
            }
            else
            {
                _currentInteractiveObject = null;
                _interactionText.SetActive(false);
            }

            if (Input.GetKeyDown(_key))
            {
                _currentInteractiveObject?.Interact();
            }
        }
    }

}
