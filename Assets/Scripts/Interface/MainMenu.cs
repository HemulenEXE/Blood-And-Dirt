﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт управления главным меню. Навешивается на пустой объект на сецне MainMenu 
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Кнопка компания.
    /// </summary>
    private Button _company;
    /// <summary>
    /// Кнопка арена.
    /// </summary>
    private Button _arena;
    /// <summary>
    /// Кнопка настроек.
    /// </summary>
    private Button _settings;
    /// <summary>
    /// Кнопка выходы.
    /// </summary>
    private Button _exit;
    /// <summary>
    /// Всплывающее меню настроек
    /// </summary>
    private GameObject _settingsMenu;
    private void Awake()
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

        _settingsMenu.SetActive(true);
        SettingData.LoadData();
        _settingsMenu.SetActive(false);
    }
    /// <summary>
    /// Нажатие на кнопку компания
    /// </summary>
    private void OnCompaniClick()
    {
        ScenesManager.Instance.OnSelectedScene(2);
        //Debug.Log("Company");
        //Debug.Log(PlayerPrefs.GetInt("currentScene", 0));
        //if (PlayerPrefs.GetInt("currentScene", 0) == 0)
        //{
        //    PlayerPrefs.DeleteAll();
        //    ScenesManager.Instance.OnSelectedScene(2);
        //}
        //else
        //{
        //    ScenesManager.Instance.OnSelectedScene(PlayerPrefs.GetInt("currentScene"));
        //    //PlayerPrefs.DeleteAll(); //Строчка добавлена для проверки, потом удалить!
        //}
    }
    /// <summary>
    /// Нажатие на кнопку арена
    /// </summary>
    private void OnArenaClick()
    { 
        Debug.Log("Arena");
        ScenesManager.Instance.OnNextScene();
    }
    /// <summary>
    /// Нажатие на кнопку настроек
    /// </summary>
    private void OnSettingsClick() 
    { 
        Debug.Log("Settings");
        _settingsMenu.gameObject.SetActive(true);
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

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            _settingsMenu.gameObject.SetActive(false);
        }
    }
}
