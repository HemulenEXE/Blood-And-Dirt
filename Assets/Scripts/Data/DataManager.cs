using UnityEngine;

/// <summary>
///  ласс, загружающий сохранЄнные данные при их наличии или устанавливающий значени€ по умолчанию
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
