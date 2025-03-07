using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public Button save;
    public Button exit;
    public Button reboot;

    public TMP_Dropdown resolution;
    public Slider volume;
    public Slider sensitivity;
    public Toggle fullScreen;

    public void ExitSetting()
    {
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (exit == null) throw new ArgumentNullException("SettingMenu: _exit is null");
        if (save == null) throw new ArgumentNullException("SettingMenu: _save is null");
        if (resolution == null) throw new ArgumentNullException("SettingMenu: _resolution is null");
        if (volume == null) throw new ArgumentNullException("SettingMenu: _audio is null");
        if (fullScreen == null) throw new ArgumentNullException("SettingMenu: _fullScreen is null");
        if (sensitivity == null) throw new ArgumentNullException("SettingMenu: _sensivity is null");

        exit.onClick.AddListener(ExitSetting);
        save.onClick.AddListener(() =>
        {
            SettingData.SaveSetting();
            ExitSetting();});
        reboot.onClick.AddListener(() => {
            SettingData.RebootSetting();
            SettingData.LoadSettings();
        });

        SettingData.LoadSettings();

        resolution.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < SettingData.Resolutions.Length; i++)
        {
            options.Add($"{SettingData.Resolutions[i].width} x {SettingData.Resolutions[i].height} @ {SettingData.Resolutions[i].refreshRate} Hz");
            if (SettingData.Resolutions[i].width.Equals(Screen.currentResolution.width) && SettingData.Resolutions[i].height.Equals(Screen.currentResolution.height))
                resolution.value = i;
        }

        resolution.AddOptions(options);
        resolution.RefreshShownValue();
        resolution.onValueChanged.AddListener(SettingData.SetResolution);

        volume.value = SettingData.Volume;
        volume.onValueChanged.AddListener(value =>
        {
            SettingData.SetVolume(value);
            SettingData.ApplySettings();
        });

        sensitivity.value = SettingData.Sensitivity;
        sensitivity.onValueChanged.AddListener(value =>
        {
            SettingData.SetSensitivity(value);
            SettingData.ApplySettings();
        });

        fullScreen.isOn = SettingData.FullScreen;
        fullScreen.onValueChanged.AddListener(value =>
        {
            SettingData.SetFullScreen(value);
            SettingData.ApplySettings();
        });
    }
    private void FixedUpdate()
    {
        volume.value = SettingData.Volume;
        sensitivity.value = SettingData.Sensitivity;
        fullScreen.isOn = SettingData.FullScreen;
    }
}
