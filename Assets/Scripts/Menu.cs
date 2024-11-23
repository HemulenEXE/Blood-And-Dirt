using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт управления главным меню. Навешивается на пустой объект на сецне MainMenu 
/// </summary>
public class Menu : MonoBehaviour
{
    //Кнопки меню
    private Button _company;
    private Button _arena;
    private Button _settings;
    private Button _exit;

    private void Start()
    {
        GameObject panel = GameObject.Find("Panel").gameObject;
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
    private void OnCompaniClick()
    { }
    private void OnArenaClick()
    { }
    private void OnSettingsClick() 
    { Debug.Log("Settings"); }
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
