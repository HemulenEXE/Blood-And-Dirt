using UnityEngine;

/// <summary>
/// ���������, ����������� ���� ��������� ������
/// </summary>
public interface IInventorySlot
{
    /// <summary>
    /// �������, �������� � ������ �����
    /// </summary>
    public GameObject StoredItem { get; set; }
    /// <summary>
    /// ������������� �����
    /// </summary>
    public bool IsFull { get; set; }
    /// <summary>
    /// ���������� �������� item � ������ ����
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item);
    /// <summary>
    /// ������������ (�������) ���� ����
    /// </summary>
    public void RemoveItem();
}
