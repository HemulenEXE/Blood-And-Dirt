using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    /// <summary>
    /// Кнопка сохранения настроек.
    /// </summary>
    private Button _save;
    /// <summary>
    /// Кнопка выхода из настроек.
    /// </summary>
    private Button _exit;
    /// <summary>
    /// Список разрешений.
    /// </summary>
    private TMP_Dropdown _resolution;
    /// <summary>
    /// Регулятор громкости.
    /// </summary>
    private Slider _audio;
    /// <summary>
    /// Кнопка полного экрана.
    /// </summary>
    private Toggle _fullScreen;
    /// <summary>
    /// Типы разрешений.
    /// </summary>
    Resolution[] resolutions;
    private void Awake()
    {

        _exit = this.transform.Find("Exit").GetComponent<Button>();
        _save = this.transform.Find("Save").GetComponent<Button>();
        _resolution = this.transform.Find("Resolution").GetComponent<TMP_Dropdown>();
        _audio = this.transform.Find("Audio").GetComponent<Slider>();
        _fullScreen = this.transform.Find("FullScreen").GetComponent<Toggle>();

        _exit.onClick.AddListener(ExitSetting);
        _save.onClick.AddListener(() =>
        {
            SaveSetting();
            ExitSetting();
        });

        _resolution.ClearOptions();

        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add($"{resolutions[i].width} x {resolutions[i].height} @ {resolutions[i].refreshRate} Hz");
            if (resolutions[i].width.Equals(Screen.currentResolution.width) && resolutions[i].height.Equals(Screen.currentResolution.height))
            {
                _resolution.value = i;
            }
        }

        _resolution.AddOptions(options);
        _resolution.RefreshShownValue();
        _resolution.onValueChanged.AddListener(SetResolution);

        _audio.onValueChanged.AddListener(SetVolume);

        LoadSettings();
    }
    /// <summary>
    /// Загрузка настроек.
    /// </summary>
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            _audio.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            _audio.value = 0.5f;
        }
        if (PlayerPrefs.HasKey("Resolution"))
        {
            _resolution.value = PlayerPrefs.GetInt("Resolution");
        }
        else
        {
            _resolution.value = resolutions.Length - 1;
        }
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            _fullScreen.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
        }
        else
        {
            _fullScreen.isOn = true;
        }

        ApplySettings();
    }
    /// <summary>
    /// Применение настроек.
    /// </summary>
    public void ApplySettings()
    {
        SetVolume(_audio.value);
        SetResolution(_resolution.value);
        SetFullScreen(_fullScreen.isOn);
    }
    /// <summary>
    /// Сброс настроек.
    /// </summary>
    public void ResetSetting()
    {
        PlayerPrefs.DeleteAll();
        LoadSettings();
    }
    /// <summary>
    /// Сохранение настроек.
    /// </summary>
    public void SaveSetting()
    {
        PlayerPrefs.SetInt("Resolution", _resolution.value); //Сохранение разрешения.
        PlayerPrefs.SetInt("Fullscreen", _fullScreen.isOn ? 1 : 0); //Сохранение полноэкранного режима.
        PlayerPrefs.SetFloat("Volume", _audio.value); //Сохранение громкости.
        PlayerPrefs.Save();
        ApplySettings();
    }
    /// <summary>
    /// Выход из меню.
    /// </summary>
    public void ExitSetting()
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// Установка полноэкранного режима.
    /// </summary>
    /// <param name="value"></param>
    public void SetFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
    /// <summary>
    /// Установка разрешения.
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetResolution(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException("SettingMenu: index < 0");
        if (index >= resolutions.Length) throw new ArgumentOutOfRangeException("SettingMenu: index >= resolutions.Length");
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }
    /// <summary>
    /// Установка громокости звука.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetVolume(float volume)
    {
        if (volume < 0) throw new ArgumentOutOfRangeException("SettingMenu: volume < 0");
        if (volume > 1) throw new ArgumentOutOfRangeException("SettingMenu: volume > 1");
        AudioListener.volume = volume;
    }
}
