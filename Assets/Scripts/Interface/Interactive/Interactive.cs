using InteractiveObjects;
using InventoryLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace InteractiveLogic
{
    /// <summary>
    /// Класс, реализующий "интерактивный интерфейс".
    /// </summary>
    public class Interactive : MonoBehaviour
    {
        //Поля.

        /// <summary>
        /// Единственный экземпляр интерактивного интерфейса.
        /// </summary>
        private static Interactive _instance;
        /// <summary>
        /// Интерактивный текст.<br/>
        /// Содержится на канвасе.<br/>
        /// Не может равняться null.
        /// </summary>
        private TextMeshProUGUI _interactiveText;
        /// <summary>
        /// Вертикальное смещение интерактивного текста.<br/>
        /// Может принимать отрицательные значения.
        /// </summary>
        [SerializeField] private float _offSet = 0.5f;

        //Свойства.

        /// <summary>
        /// Возвращает единственный экземпляр интерактивного интерфейса.
        /// </summary>
        public static Interactive GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    var temp = GameObject.Find("Interactive")?.GetComponent<Interactive>();
                    if (temp == null) throw new ArgumentNullException("Interactive: Scene doesn't have the canvas \"Interactive\"");
                    _instance = temp;
                }
                return _instance;
            }
        }

        //Встроенные методы.

        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void Awake()
        {
            if (_interactiveText == null)
            {
                var temp = this.transform.Find("UIPanel")?.Find("InteractiveText")?.GetComponent<TextMeshProUGUI>();
                if (temp == null) throw new ArgumentNullException("Interactive: The canvas \"Interactive\" is uncorrect");
                _interactiveText = temp;
            }
        }

        public void TurnOnText(IInteractable item)
        {
            if (item == null) return;

            _interactiveText.gameObject.SetActive(true);
            //Корректировка интерактивного текста.
            _interactiveText.text = item.Description;
            Vector3 positionItem = item.Transform.position;
            positionItem.y = item.Renderer.bounds.max.y + _offSet; //Получение верхней границы визуального представления объекта.
            positionItem.x = (item.Renderer.bounds.max.x + item.Renderer.bounds.min.x) / 2;
            Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(Camera.main, positionItem);
            _interactiveText.transform.position = positionInWorld;
        }
        public void TurnOffText()
        {
            _interactiveText.text = "";
            _interactiveText.gameObject.SetActive(false);
        }
    }
}
