using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Скрипт управления внутриигровым меню. Навешивается на GameMenu
/// </summary>
public class GameMenu : MonoBehaviour
{
    /// <summary>
    /// Управляет аниацией открытия меню
    /// </summary>
    [SerializeField] private Animator _animator;
    ///  <summary>
    /// Кнопка сохранения настроек
    /// </summary>
    private Button _save;
    /// <summary>
    /// Кнопка выхода в главное меню
    /// </summary>
    private Button _inMainMenu;
    /// <summary>
    /// Кнопка регулирования аудио.
    /// </summary>
    private Slider _audio;
    /// <summary>
    /// Кнопка перезапуска сцены
    /// </summary>
    private Button _restartScene;
    /// <summary>
    /// Проверка и настройка полей.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private void Awake()
    {
        Button icon = GameObject.Find("MenuIcon")?.GetComponent<Button>();
        if (icon == null) throw new ArgumentNullException("GameMenu: icon is null");
        icon.onClick.AddListener(ControllMenu);
        
        GameObject menu = GameObject.Find("SettingsMenu");
        if (menu == null) throw new ArgumentNullException("GameMenu: menu is null");
        _save = menu?.transform?.GetChild(2)?.GetComponent<Button>();
        _inMainMenu = menu?.transform?.GetChild(3)?.GetComponent<Button>();
        _restartScene = menu?.transform?.GetChild(4)?.GetComponent<Button>();
        _audio = menu?.transform?.GetChild(1)?.GetComponent<Slider>();
        if (_save == null) throw new ArgumentNullException("GameMenu: _save is null");
        if (_inMainMenu == null) throw new ArgumentNullException("GameMenu: _inMainMenu is null");
        if (_restartScene == null) throw new ArgumentNullException("GameMenu: _restartScene is null");
        if (_audio == null) throw new ArgumentNullException("GameMenu: _audio is null");
        _save.onClick.AddListener(Save);
        _inMainMenu.onClick.AddListener(InMainMenu);
        _restartScene.onClick.AddListener(RestartScene);
        _audio.onValueChanged.AddListener(SetVolume);
        if (PlayerPrefs.HasKey("Volume")) _audio.value = PlayerPrefs.GetFloat("Volume");
    }
    /// <summary>
    /// Переключение на главное меню (без сохранения прогресса на текущей сцене)
    /// </summary>
    private void InMainMenu()
    {
        transform.GetChild(3).gameObject.SetActive(true);
    }
    /// <summary>
    /// Установка громокости звука.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetVolume(float volume)
    {
        if (volume < 0) throw new ArgumentOutOfRangeException("GameMenu: volume < 0");
        if (volume > 1) throw new ArgumentOutOfRangeException("GameMenu: volume > 1");
        AudioListener.volume = volume;
    }
    /// <summary>
    /// Закрытие меню с сохранением настроек
    /// </summary>
    private void Save()
    {
        Time.timeScale = 1;
        _animator.SetBool(name: "startOpen", false);
        PlayerPrefs.SetFloat("Volume", _audio.value); //Сохранение громкости.
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Запускает заново сцену
    /// </summary>
    private void RestartScene()
    {
        GameObject notice = transform.GetChild(3).gameObject;
        notice.SetActive(true);
        //Указание на какую сцену перейти.
        notice.GetComponent<PopUpNotice>().SceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    /// <summary>
    /// Открытие/закрытие меню по нажатию на иконку менюшки без сохранения настроек
    /// </summary>
    private void ControllMenu()
    {
        if (_animator.GetBool(name: "startOpen"))
        {
            Time.timeScale = 1;
            _animator.SetBool(name: "startOpen", false);
        }
        else
        {
            Time.timeScale = 0;
            _animator.SetBool(name: "startOpen", true);
        }
    }
}
