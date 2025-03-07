using System;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class SettingData
{
    private static string _savedPath = "C:\\Users\\Amethyst\\Desktop\\Downloads\\Settings.xml";

    public static Resolution Resolution {  get; private set; }
    public static float Volume { get; private set; }
    public static float Sensitivity { get; private set; }
    public static bool FullScreen { get; private set; }

    public static Resolution[] Resolutions { get; private set; }  = Screen.resolutions;

    public static KeyCode Up { get; private set; }
    public static KeyCode Down { get; private set; }
    public static KeyCode Left { get; private set; }
    public static KeyCode Right { get; private set; }

    public static void SaveSetting()
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
        LoadSettings();
    }
    public static void LoadSettings()
    {
        if (File.Exists(_savedPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);

            XmlNode root = xmlDoc.SelectSingleNode("Settings");
            if (root != null)
            {
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
