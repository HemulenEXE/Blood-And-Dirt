using UnityEngine;

/// <summary>
/// �����, ����������� ���������� ������ ��� �� ������� ��� ��������������� �������� �� ���������
/// </summary>
public class DataManager : MonoBehaviour
{
    private void Start()
    {
        SettingData.LoadData();
        PlayerData.LoadData();
    }
}
