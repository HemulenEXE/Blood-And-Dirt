using System;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class SettingData
{
    private static string _savedPath; // Значение присваивается в методе Initialize()
    public static Resolution Resolution {  get; private set; }
    public static float Volume { get; private set; }
    public static float Sensitivity { get; private set; }
    public static bool FullScreen { get; private set; }

    public static Resolution[] Resolutions { get; private set; }

    public static KeyCode Up { get; private set; } = KeyCode.W;
    public static KeyCode Down { get; private set; } = KeyCode.S;
    public static KeyCode Left { get; private set; } = KeyCode.A;
    public static KeyCode Right { get; private set; } = KeyCode.D;

    public static KeyCode Run { get; private set; } = KeyCode.LeftShift;
    public static KeyCode Steal { get; private set; } = KeyCode.LeftControl;
    public static KeyCode Interact { get; private set; } = KeyCode.E;
    public static KeyCode Dialogue { get; private set; } = KeyCode.T;

    public static KeyCode FirstAidKit { get; private set; } = KeyCode.Alpha3;
    public static KeyCode Bandage { get; private set; } = KeyCode.Alpha4;
    public static KeyCode SimpleGrenade { get; private set; } = KeyCode.Alpha2;
    public static KeyCode SmokeGrenade { get; private set; } = KeyCode.Alpha1;

    public static void Initialize()
    {
        _savedPath = Path.Combine(Application.persistentDataPath, "Settings.xml");
        LoadData();
    }
    public static void SaveData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("Settings");
        xmlDoc.AppendChild(root);

        XmlElement resolutionElement = xmlDoc.CreateElement("Resolution");
        resolutionElement.InnerText = $"{Resolution.width}x{Resolution.height}@{Resolution.refreshRate}";
        root.AppendChild(resolutionElement);

        XmlElement fullscreenElement = xmlDoc.CreateElement("Fullscreen");
        fullscreenElement.InnerText = FullScreen ? "1" : "0";
        root.AppendChild(fullscreenElement);

        XmlElement volumeElement = xmlDoc.CreateElement("Volume");
        volumeElement.InnerText = Volume.ToString();
        root.AppendChild(volumeElement);

        XmlElement sensitivityElement = xmlDoc.CreateElement("Sensitivity");
        sensitivityElement.InnerText = Sensitivity.ToString();
        root.AppendChild(sensitivityElement);

        xmlDoc.Save(_savedPath);
        LoadData();
    }
    public static void LoadData()
    {
        if (File.Exists(_savedPath))
        {
            Debug.Log("Exists");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);

            XmlNode root = xmlDoc.SelectSingleNode("Settings");
            if (root != null)
            {
                Debug.Log("Not null");
                Volume = float.TryParse(root["Volume"]?.InnerText, out float volume) ? volume : 0.5f;
                string temp = root["Resolution"]?.InnerText;
                string[] parts = temp.Split(new[] { 'x', '@' });
                if (parts.Length == 3 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height) && int.TryParse(parts[2], out int refreshRate))
                    Resolution = new Resolution { width = width, height = height, refreshRate = refreshRate };
                FullScreen = int.TryParse(root["Fullscreen"]?.InnerText, out int fullscreen) && fullscreen == 1;
                Sensitivity = float.TryParse(root["Sensivity"]?.InnerText, out float sensivity) ? sensivity : 1.0f;
            }
        }
        else
        {
            Resolutions = Screen.resolutions;
            // Значения по умолчанию
            Volume = 0.5f;
            Resolution = Resolutions.Last();
            FullScreen = true;
            Sensitivity = 1.0f;
        }
        ApplySettings();
    }
    public static void ApplySettings()
    {
        Screen.SetResolution(Resolution.width, Resolution.height, Screen.fullScreen);
        AudioListener.volume = Volume;
        Screen.fullScreen = FullScreen;
        // Нет отдельного компонента Unity, который работает с чувствительностью мыши
    }
    public static void RebootSetting()
    {
        if (File.Exists(SettingData._savedPath)) File.Delete(_savedPath);
    }

    public static void SetVolume(float value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException("SettingData: value < 0");
        if (value > 1) throw new ArgumentOutOfRangeException("SettingData: value > 1");
        Volume = value;
    }
    public static void SetSensitivity(float value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException("SettingData: value < 0");
        if (value > 1) throw new ArgumentOutOfRangeException("SettingData: value > 1");
        Sensitivity = value;
    }
    public static void SetResolution(Resolution value)
    {
        Resolution = value;
    }
    public static void SetResolution(int index)
    {
        Resolution = Resolutions[index];
    }
    public static void SetFullScreen(bool value)
    {
        FullScreen = value;
    }
    public static void SetUpButton(KeyCode value)
    {
        Up = value;
    }
    public static void SetDownButton(KeyCode value)
    {
        Down = value;
    }
    public static void SetLeftButton(KeyCode value)
    {
        Left = value;
    }
    public static void SetRightButton(KeyCode value)
    {
        Right = value;
    }
}
