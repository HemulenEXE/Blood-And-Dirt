using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryLogic
{
    /// <summary>
    /// �����, ����������� ��������� ������.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// ������ ����������� ������.
        /// </summary>
        public static List<InventorySlot> _slots; //������ ������� ������������ ������ ��� ������������ ���������� ����� ������.
        /// <summary>
        /// ������� ����.<br/>
        /// ��������� ��� � ����.
        /// </summary>
        public static int _currentSlot = 0;
        /// <summary>
        /// ����������� ����� ������.
        /// </summary>
        public int _minCountSlots = 3;
        /// <summary>
        /// ������������ ����� ������.
        /// </summary>
        public int _maxCountSlots = 5;
        /// <summary>
        /// ������ ����������� �����.
        /// </summary>
        [SerializeField] private float _sizeSlot = 0.5f;
        /// <summary>
        /// �������� � ��������� �����.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Awake()
        {
            if (_minCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minCountSlots < 0");
            if (_maxCountSlots < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < 0");
            if (_maxCountSlots < _minCountSlots) throw new ArgumentOutOfRangeException("PlayerInventory: _maxCountSlots < _minCountSlots");
            if (_sizeSlot < 0) throw new ArgumentOutOfRangeException("PlayerInventory: _minSizeSlot < 0");

            _slots = new List<InventorySlot>(_maxCountSlots);
            for (int i = 0; i < _minCountSlots; i++)
                AddSlot();

            SelectSlot(0); //�� ���������, �������� ������� ����.
        }
        private void Update()
        {
            //����������� ����� ������� � ������� ��������������� ������ 1 2 3 ...
            for (int i = 49; i < 49 + _slots.Count; i++)
            {
                KeyCode key = (KeyCode)i;
                if (Input.GetKey(key))
                {
                    SelectSlot(i - 49);
                }
            }

            //����������� ����� ������� ����������� ������� ������������ ����.
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                SelectNextSlot();
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                SelectPrevSlot();
            }

            //������������ ���������� �����.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _slots[_currentSlot].RemoveItem();
            }

            //��������� ��������������� �����.
            if (Input.GetKeyDown(KeyCode.P))
                AddSlot();
        }
        /// <summary>
        /// ���������� ������, ���������������, �����.<br/>
        /// ����� ���������� ������ �� ����� ��������� _maxCountSlots.
        /// </summary>
        public void AddSlot()
        {
            if (_slots.Count < _maxCountSlots)
            {
                //�������� ������ �����
                GameObject interim_slot = new GameObject($"Slot {_slots.Count}");
                InventorySlot interim_abstract_slot = interim_slot.AddComponent<InventorySlot>();
                interim_slot.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.Find("ConsumablesUI").Find("InventoryUI"), true); //������������ ����� � ��������.
                interim_slot.transform.localScale = Vector3.one * _sizeSlot;
                interim_abstract_slot.StoredItem = null;
                interim_abstract_slot.ImageStoredItem = interim_slot.AddComponent<UnityEngine.UI.Image>();
                interim_abstract_slot.ImageStoredItem.sprite = InventorySlot._emptySlotImage;
                if (_slots.Count > 0)
                    interim_abstract_slot.gameObject.SetActive(false);

                _slots.Add(interim_abstract_slot);
            }
        }
        /// <summary>
        /// ����������� �� ��������� ����.
        /// </summary>
        public void SelectNextSlot()
        {
            SelectSlot((_currentSlot + 1) % _slots.Count);
        }
        /// <summary>
        /// ����������� �� ���������� ����.
        /// </summary>
        public void SelectPrevSlot()
        {
            SelectSlot((_currentSlot - 1 + _slots.Count) % _slots.Count);
        }
        /// <summary>
        /// ����������� �� ���� � �������� ��������.
        /// </summary>
        /// <param name="index"></param>
        public void SelectSlot(int index)
        {
            if (index < 0 || index >= _slots.Count) throw new ArgumentOutOfRangeException("PlayerInventory: index < 0");
            if (index >= _slots.Count) throw new ArgumentOutOfRangeException("PlayerInventory: _slots.Count");

            //��������� �������� �����.
            _slots[_currentSlot].StoredItem?.Deactive();
            _slots[_currentSlot].gameObject.SetActive(false);

            //������������ �� ���� � ��������� ��������.
            _currentSlot = index;

            //��������� �������� �����.
            _slots[_currentSlot].StoredItem?.Active();
            _slots[_currentSlot].gameObject.SetActive(true);
        }
    }
}
