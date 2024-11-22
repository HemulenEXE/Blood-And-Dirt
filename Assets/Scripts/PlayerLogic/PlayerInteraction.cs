using System;
using TMPro;
using UnityEngine;
using InteractiveObjects;

namespace PlayerLogic
{
    /// <summary>
    /// �����, ����������� "���������� �������������� ������ � �������������� ���������".
    /// </summary>
    public class PlayerInteract : MonoBehaviour
    {
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
        /// ������������ �������� �������������� ������.<br/>
        /// ����� ��������� ������������� ��������.
        /// </summary>
        public float OffSet = 0.5f;
        /// <summary>
        /// ������������� �����.<br/>
        /// ���������� �� �������.<br/>
        /// �� ����� ��������� null.
        /// </summary>
        public GameObject _interactionText;
        /// <summary>
        /// ������, � ������� �� ������� ������ ��������������� �����.<br/>
        /// ����� ��������� null.
        /// </summary>
        private IInteractable _currentInteractiveObject;
        /// <summary>
        /// ������������ ����.
        /// </summary>
        [SerializeField] private LayerMask _ignoreLayer;
        /// <summary>
        /// �������� � ��������� �����.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Awake()
        {
            if (_interactionDistance < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _interactionDistance < 0");
        }
        void Update()
        {
            Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
            Debug.DrawRay(ray.origin, ray.direction * _interactionDistance, Color.red); //������� ����.

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _interactionDistance, ~_ignoreLayer);
            if (hit.collider != null) //�������� �� ����������� � �����-���� ��������, ���������� Collider2D.
            {
                IInteractable interim_interactive_object = hit.transform.GetComponent<IInteractable>();
                if (interim_interactive_object != null)
                {
                    _interactionText.SetActive(true);
                    _currentInteractiveObject = interim_interactive_object;
                    TextMeshProUGUI interim_textMeshProUGUI = _interactionText.GetComponent<TextMeshProUGUI>();
                    if (interim_textMeshProUGUI != null)
                    {
                        //������������� �������������� ������.
                        interim_textMeshProUGUI.text = _currentInteractiveObject.Description;
                        Vector3 positionObject = _currentInteractiveObject.Transform.position;
                        positionObject.y = _currentInteractiveObject.Renderer.bounds.max.y + OffSet; //��������� ������� ������� ����������� ������������� �������.
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
