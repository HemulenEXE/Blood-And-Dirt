using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт управления главным меню. Навешивается на пустой объект на сецне MainMenu 
/// </summary>
public class MainMenu : MonoBehaviour
{
    //Кнопки меню
    private Button _company;
    private Button _arena;
    private Button _settings;
    private Button _exit;

    /// <summary>
    /// Всплывающее меню настроек
    /// </summary>
    private GameObject _settingsMenu;
    private void Start()
    {
        GameObject panel = GameObject.Find("Panel");
        _settingsMenu = GameObject.Find("MainMenu").transform.GetChild(2).gameObject;
        _company = panel.transform.GetChild(0).GetComponent<Button>();
        _arena = panel.transform.GetChild(1).GetComponent<Button>();
        _settings = panel.transform.GetChild(2).GetComponent<Button>();
        _exit = panel.transform.GetChild(3).GetComponent<Button>();

        //Добавления обработчиков нажатия кнопки
        _company.onClick.AddListener(OnCompaniClick);
        _arena.onClick.AddListener(OnArenaClick);
        _settings.onClick.AddListener(OnSettingsClick);
        _exit.onClick.AddListener(OnExitClick);
    }
    /// <summary>
    /// Нажатие на кнопку компания
    /// </summary>
    private void OnCompaniClick()
    { 
        Debug.Log("Company");
        //ScenesManager.Instance.OnNextScene();
    }
    /// <summary>
    /// Нажатие на кнопку арена
    /// </summary>
    private void OnArenaClick()
    { 
        Debug.Log("Arena");
        //ScenesManager.Instance.OnSelectedScene(PlayerPrefs.GetInt("currentScene"));
    }
    /// <summary>
    /// Нажатие на кнопку настроек
    /// </summary>
    private void OnSettingsClick() 
    { 
        Debug.Log("Settings");
        _settingsMenu.gameObject.SetActive(true);

        Button exit = _settingsMenu.transform.Find("Exit").GetComponent<Button>();
        Button save = _settingsMenu.transform.Find("Save").GetComponent<Button>();

        exit.onClick.RemoveAllListeners();
        save.onClick.RemoveAllListeners();
        exit.onClick.AddListener(OnSettingsExitClick);
        save.onClick.AddListener(OnSettingsSaveClick);
    }
    /// <summary>
    /// Закрытие меню настроек без сохранения
    /// </summary>
    private void OnSettingsExitClick()
    {
        Debug.Log("SettingsExit");
        _settingsMenu.SetActive(false);
    }
    /// <summary>
    /// Закрытие настроек с сохранением изменений
    /// </summary>
    private void OnSettingsSaveClick()
    {
        Debug.Log("SettingsSave");
        _settingsMenu.SetActive(false);
    }
    /// <summary>
    /// Выход из игры по нажатию кнопки
    /// </summary>
    private void OnExitClick() 
    {
        Debug.Log("Exit");
        PlayerPrefs.Save();
        Application.Quit();
    }
}
