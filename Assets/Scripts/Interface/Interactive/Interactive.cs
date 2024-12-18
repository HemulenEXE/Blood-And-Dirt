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
    /// �����, ����������� "������������� ���������".
    /// </summary>
    public class Interactive : MonoBehaviour
    {
        //����.

        /// <summary>
        /// ������������ ��������� �������������� ����������.
        /// </summary>
        private static Interactive _instance;
        /// <summary>
        /// ������������� �����.<br/>
        /// ���������� �� �������.<br/>
        /// �� ����� ��������� null.
        /// </summary>
        private TextMeshProUGUI _interactiveText;
        /// <summary>
        /// ������������ �������� �������������� ������.<br/>
        /// ����� ��������� ������������� ��������.
        /// </summary>
        [SerializeField] private float _offSet = 0.5f;

        //��������.

        /// <summary>
        /// ���������� ������������ ��������� �������������� ����������.
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

        //���������� ������.

        /// <summary>
        /// �������� � ��������� �����.
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
            //������������� �������������� ������.
            _interactiveText.text = item.Description;
            Vector3 positionItem = item.Transform.position;
            positionItem.y = item.Renderer.bounds.max.y + _offSet; //��������� ������� ������� ����������� ������������� �������.
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
