using System;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class SettingData
{
    private static string _savedPath; // Значение присваивается в методе Initialize()
    public static Resolution[] Resolutions { get; private set; }

    // Настройки игры

    public static Resolution Resolution { get; private set; }
    public static float Volume { get; private set; }
    public static float Sensitivity { get; private set; }
    public static bool FullScreen { get; private set; }

    // Управление

    public static KeyCode Up { get; private set; }
    public static KeyCode Down { get; private set; }
    public static KeyCode Left { get; private set; }
    public static KeyCode Right { get; private set; }

    public static KeyCode Run { get; private set; }
    public static KeyCode Steal { get; private set; }
    public static KeyCode Interact { get; private set; }
    public static KeyCode Dialogue { get; private set; }

    public static KeyCode FirstAidKit { get; private set; }
    public static KeyCode Bandage { get; private set; }
    public static KeyCode SimpleGrenade { get; private set; }
    public static KeyCode SmokeGrenade { get; private set; }

    public static void Initialize()
    {
        _savedPath = Path.Combine(Application.persistentDataPath, "Settings.xml");
        Resolutions = Screen.resolutions;
        LoadData();
    }
    public static void LoadData()
    {
        if (File.Exists(_savedPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);

            XmlNode root = xmlDoc.DocumentElement;
            Volume = float.TryParse(root["Volume"]?.InnerText, out float volume) ? volume : 0.5f;
            string temp = root["Resolution"]?.InnerText;
            string[] parts = temp.Split(new[] { 'x', '@' });
            if (parts.Length == 3 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height) && int.TryParse(parts[2], out int refreshRate))
                Resolution = new Resolution { width = width, height = height, refreshRate = refreshRate };
            FullScreen = int.TryParse(root["Fullscreen"]?.InnerText, out int fullscreen) && fullscreen == 1;
            Sensitivity = float.TryParse(root["Sensivity"]?.InnerText, out float sensivity) ? sensivity : 1.0f;
            Up = LoadKeyElement(root["Up"]);
            Down = LoadKeyElement(root["Down"]);
            Left = LoadKeyElement(root["Left"]);
            Right = LoadKeyElement(root["Right"]);
            Run = LoadKeyElement(root["Run"]);
            Steal = LoadKeyElement(root["Steal"]);
            Interact = LoadKeyElement(root["Interact"]);
            Dialogue = LoadKeyElement(root["Dialogue"]);
            FirstAidKit = LoadKeyElement(root["FirstAidKit"]);
            Bandage = LoadKeyElement(root["Bandage"]);
            SimpleGrenade = LoadKeyElement(root["SimpleGrenade"]);
            SmokeGrenade = LoadKeyElement(root["SmokeGrenade"]);
        }
        else
        {
            DefaultParameters();
            SaveData();
        }
        ApplySettings();
    }
    public static void SaveData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("Settings");
        xmlDoc.AppendChild(root);

        XmlElement temp = xmlDoc.CreateElement("Resolution");
        temp.InnerText = $"{Resolution.width}x{Resolution.height}@{Resolution.refreshRate}";
        root.AppendChild(temp);

        XmlElement fullscreenElement = xmlDoc.CreateElement("Fullscreen");
        fullscreenElement.InnerText = FullScreen ? "1" : "0";
        root.AppendChild(fullscreenElement);

        XmlElement volumeElement = xmlDoc.CreateElement("Volume");
        volumeElement.InnerText = Volume.ToString();
        root.AppendChild(volumeElement);

        XmlElement sensitivityElement = xmlDoc.CreateElement("Sensitivity");
        sensitivityElement.InnerText = Sensitivity.ToString();
        root.AppendChild(sensitivityElement);

        root.AppendChild(CreateKeyElement(xmlDoc, "Up", Up));
        root.AppendChild(CreateKeyElement(xmlDoc, "Down", Down));
        root.AppendChild(CreateKeyElement(xmlDoc, "Left", Left));
        root.AppendChild(CreateKeyElement(xmlDoc, "Right", Right));
        root.AppendChild(CreateKeyElement(xmlDoc, "Run", Run));
        root.AppendChild(CreateKeyElement(xmlDoc, "Steal", Steal));
        root.AppendChild(CreateKeyElement(xmlDoc, "Interact", Interact));
        root.AppendChild(CreateKeyElement(xmlDoc, "Dialogue", Dialogue));
        root.AppendChild(CreateKeyElement(xmlDoc, "FirstAidKit", FirstAidKit));
        root.AppendChild(CreateKeyElement(xmlDoc, "Bandage", Bandage));
        root.AppendChild(CreateKeyElement(xmlDoc, "SimpleGrenade", SimpleGrenade));
        root.AppendChild(CreateKeyElement(xmlDoc, "SmokeGrenade", SmokeGrenade));

        xmlDoc.Save(_savedPath);
        LoadData();
    }
    public static void DefaultParameters() // Определяет данные по умолчанию
    {
        Volume = 0.5f;
        Resolution = Resolutions.Last();
        FullScreen = true;
        Sensitivity = 1.0f;

        Up = KeyCode.W;
        Down = KeyCode.S;
        Left = KeyCode.A;
        Right = KeyCode.D;

        Run = KeyCode.LeftShift;
        Steal = KeyCode.LeftControl;
        Interact = KeyCode.E;
        Dialogue = KeyCode.T;

        FirstAidKit = KeyCode.Alpha3;
        Bandage = KeyCode.Alpha4;
        SimpleGrenade = KeyCode.Alpha2;
        SmokeGrenade = KeyCode.Alpha1;
    }
    public static void Reboot()
    {
        if (File.Exists(SettingData._savedPath)) File.Delete(_savedPath);
        DefaultParameters();
    }
    public static void ApplySettings()
    {
        Screen.SetResolution(Resolution.width, Resolution.height, Screen.fullScreen);
        AudioListener.volume = Volume;
        Screen.fullScreen = FullScreen;
        // Нет отдельного компонента Unity, который работает с чувствительностью мыши
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

    private static XmlElement CreateKeyElement(XmlDocument xmlDoc, string keyName, KeyCode key)
    {
        XmlElement keyElement = xmlDoc.CreateElement(keyName);
        keyElement.InnerText = key.ToString();
        return keyElement;
    }
    private static KeyCode LoadKeyElement(XmlNode keyNode)
    {
        return keyNode != null ? Enum.TryParse(keyNode.InnerText, out KeyCode result) ? result : KeyCode.None : KeyCode.None;
    }
}
