using UnityEngine;

/// <summary>
/// �����, ����������� ���������� ������ ��� �� ������� ��� ��������������� �������� �� ���������
/// </summary>
public class DataManager : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("DataManager is awaked");
        SettingData.Initialize();
        PlayerData.Initialize();

        var player = GameObject.FindGameObjectWithTag("Player");
        foreach (var x in PlayerData.Skills)
            if (x.Type.Equals(SkillType.Activated)) x.Execute(player);
    }
}
