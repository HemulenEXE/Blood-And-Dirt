using UnityEngine;

/// <summary>
/// �����, ����������� ���������� ������ ��� �� ������� ��� ��������������� �������� �� ���������
/// </summary>
public class DataManager : MonoBehaviour
{
    private void Awake()
    {
        SettingData.Initialize();
        PlayerData.Initialize();
    }
}
