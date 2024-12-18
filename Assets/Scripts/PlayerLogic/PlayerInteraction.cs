using System;
using TMPro;
using UnityEngine;
using InteractiveObjects;
using InteractiveLogic;

namespace PlayerLogic
{
    /// <summary>
    /// �����, ����������� "���������� �������������� ������ � �������������� ���������".
    /// </summary>
    public class PlayerInteract : MonoBehaviour
    {
        //����.

        /// <summary>
        /// ��������� �������������� � ���������.<br/>
        /// �� ����� ���� �������������.
        /// </summary>
        public float _interactionDistance = 1f;
        /// <summary>
        /// ������ ��������������.
        /// </summary>
        public static KeyCode _key = KeyCode.E;
        /// <summary>
        /// ������, � ������� �� ������� ������ ��������������� �����.<br/>
        /// ����� ��������� null.
        /// </summary>
        private IInteractable _currentInteractiveObject;
        /// <summary>
        /// ������������ ����.
        /// </summary>
        [SerializeField] private LayerMask _ignoreLayer;

        //���������� ������.

        /// <summary>
        /// �������� � ��������� �����.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Awake()
        {
            if (_interactionDistance < 0) throw new ArgumentOutOfRangeException("PlayerInteract: _interactionDistance < 0");
        }
        void Update()
        {
            Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
            Debug.DrawRay(ray.origin, ray.direction * _interactionDistance, Color.red); //������� ����.

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _interactionDistance, ~_ignoreLayer);
            if (hit.collider != null) //�������� �� ����������� � �����-���� ��������, ���������� Collider2D.
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
