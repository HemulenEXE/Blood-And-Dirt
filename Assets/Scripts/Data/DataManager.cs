using UnityEngine;

/// <summary>
/// �����, ����������� ���������� ������ ��� �� ������� ��� ��������������� �������� �� ���������
/// </summary>
public class DataManager : MonoBehaviour
{
    private void Start()
    {
        SettingData.Initialize();
        PlayerData.Initialize();
    }
}
