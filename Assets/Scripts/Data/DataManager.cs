using UnityEngine;

/// <summary>
/// �����, ����������� ���������� ������ ��� �� ������� ��� ��������������� �������� �� ���������
/// </summary>
public class DataManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Start");
        SettingData.LoadData();
        PlayerData.LoadData();
    }
}
