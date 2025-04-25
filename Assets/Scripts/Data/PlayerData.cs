using SkillLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class PlayerData
{
    private static string _savedPath; // Значение присваивается в методе Initialize()

    public static Dictionary<string, Skill> SkillsStorage { get; } = new Dictionary<string, Skill>{ // Загатовки навыков
        { "AnyPrice", new AnyPrice() },
        { "BlindRange", new BlindRange() },
        { "DropByDrop", new DropByDrop() },
        { "Hatred", new Hatred() },
        { "IncreasedMetabolism", new IncreasedMetabolism() },
        { "LiveInNotVain", new LiveInNotVain() },
        { "MusclesSecondSkeleton", new MusclesSecondSkeleton() },
        { "MusclesSecondSkeleton2", new MusclesSecondSkeleton2() },
        { "Reincarnation", new Reincarnation() },
        { "SledGrenade", new SledGrenade() },
        { "Sound", new Sound() },
        { "Spin", new Spin() },
        { "StartOfANewLife", new StartOfANewLife() },
        {"InevitableDeath", new InevitableDeath() } };

    // Сохраняются в .xml

    public static HashSet<Skill> Skills = new HashSet<Skill>();
    public static bool IsGod { get; set; } // Неузвимость
    public static int Score { get; set; } // Количество очков для прокачки
    public static int InventoryCapacity { get; set; } = 3; // Число слотов
    public static int BandageCount { get; set; } = 0;
    public static int FirstAidKitCount { get; set; } = 0;
    public static int SimpleGrenadeCount { get; set; } = 0;
    public static int SmokeGrenadeCount { get; set; } = 0;

    // Не сохраняются в .xml
    public static int MaxHealth { get; set; } = 100;
    public static int CurrentHealth { get; set; } = MaxHealth;
    public static int CurrentResurrectionCount { get; set; } = 0;
    public static int CurrentHitsToSurvive { get; set; } = 0;
    public static int HitsToSurvive { get; set; } = 0; // Количество пропускаемых ударов
    public static int ResurrectionCount { get; set; } = 0; // Количество воскрешений
    

    public static bool IsStealing { get; set; } = false;
    public static bool IsWalking { get; set; } = false;
    public static bool IsRunning { get; set; } = false;

    public static float StealSpeed { get; set; } = 1f;
    public static float WalkSpeed { get; set; } = 3f;
    public static float RunSpeed { get; set; } = 4.5f;

    public static float StealNoise { get; set; } = 1f;
    public static float WalkNoise { get; set; } = 4f;
    public static float RunNoise { get; set; } = 5f;

    public static int BleedingDamage { get; set; } = 5;
    public static bool IsBleeding { get; set; } = false;

    public static int MaxBandageCount { get; set; } = 5;
    public static int MaxFirstAidKitCount { get; set; } = 5;
    public static int MaxSmokeGrenadeCount { get; set; } = 5;
    public static int MaxSimpleGrenadeCount { get; set; } = 5;

    public static int BandageHealth { get; set; } = 15; // Сколько бинт восстанавливает здоровья
    public static int FirstAidKitHealth { get; set; } = 30; // Сколько аптечка восстанавливает здоровья

    public static void Initialize()
    {
        _savedPath = Path.Combine(Application.persistentDataPath, "PlayerData.xml");
        if (!CheckFile())
        {
            Debug.Log("The PlayerData.xml is uncorrect or deleted. This file will be (re-)created");
            File.Delete(_savedPath);
            LoadData();
        }
        LoadData();
    }
    public static void LoadData()
    {
        if (File.Exists(_savedPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);
            XmlNode root = xmlDoc.DocumentElement;

            Skills.Clear();

            XmlNode skillsNode = root.SelectSingleNode("Skills");
            if (skillsNode != null)
            {
                foreach (XmlNode x in skillsNode.ChildNodes)
                {
                    string name = x.SelectSingleNode("Name").InnerText;
                    if (SkillsStorage.TryGetValue(name, out Skill skill)) Skills.Add(skill);
                }
            }

            IsGod = bool.Parse(root.SelectSingleNode("IsGod").InnerText);

            Score = int.Parse(root.SelectSingleNode("Score").InnerText);

            InventoryCapacity = int.Parse(root.SelectSingleNode("InventoryCapacity").InnerText);

            BandageCount = int.Parse(root.SelectSingleNode("BandageCount").InnerText);
            FirstAidKitCount = int.Parse(root.SelectSingleNode("FirstAidKitCount").InnerText);
            SmokeGrenadeCount = int.Parse(root.SelectSingleNode("SmokeGrenadeCount").InnerText);
            SimpleGrenadeCount = int.Parse(root.SelectSingleNode("SimpleGrenadeCount").InnerText);
        }
        else
        {
            DefaultParameters();
            SaveData();
        }
        // Автоматически применяются настройки
    }
    public static void SaveData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("PlayerData");
        xmlDoc.AppendChild(root);

        XmlElement skillsElement = xmlDoc.CreateElement("Skills");
        foreach (var x in Skills)
        {
            XmlElement skillElement = xmlDoc.CreateElement("Skill");
            skillElement.AppendChild(CreateElement(xmlDoc, "Name", x.Name));
            skillsElement.AppendChild(skillElement);
        }
        root.AppendChild(skillsElement);

        root.AppendChild(CreateElement(xmlDoc, "IsGod", IsGod));

        root.AppendChild(CreateElement(xmlDoc, "Score", Score));

        root.AppendChild(CreateElement(xmlDoc, "InventoryCapacity", InventoryCapacity));

        root.AppendChild(CreateElement(xmlDoc, "BandageCount", BandageCount));
        root.AppendChild(CreateElement(xmlDoc, "FirstAidKitCount", FirstAidKitCount));
        root.AppendChild(CreateElement(xmlDoc, "SimpleGrenadeCount", SimpleGrenadeCount));
        root.AppendChild(CreateElement(xmlDoc, "SmokeGrenadeCount", SmokeGrenadeCount));

        xmlDoc.Save(_savedPath);
        LoadData();
    }
    public static void DefaultParameters() // Определяет данные по умолчанию
    {
        Skills.Clear();
        IsGod = false;
        Score = 0;
        InventoryCapacity = 3;

        CurrentHealth = MaxHealth;
        CurrentResurrectionCount = 0;
        CurrentHitsToSurvive = 0;
    }
    public static void Reboot()
    {
        if (File.Exists(_savedPath)) File.Delete(_savedPath);
        DefaultParameters();
    }
    public static bool CheckFile()
    {
        if (!File.Exists(_savedPath)) return false;

        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);
            XmlNode root = xmlDoc.DocumentElement;

            if (root["Skills"] == null ||
                root["IsGod"] == null ||
                root["Score"] == null ||
                root["InventoryCapacity"] == null ||
                root["BandageCount"] == null ||
                root["FirstAidKitCount"] == null ||
                root["SimpleGrenadeCount"] == null ||
                root["SmokeGrenadeCount"] == null) return false;

            if (!bool.TryParse(root["IsGod"].InnerText, out _) ||
                !int.TryParse(root["Score"].InnerText, out _) ||
                !int.TryParse(root["InventoryCapacity"].InnerText, out _) ||
                !int.TryParse(root["BandageCount"].InnerText, out _) ||
                !int.TryParse(root["FirstAidKitCount"].InnerText, out _) ||
                !int.TryParse(root["SimpleGrenadeCount"].InnerText, out _) ||
                !int.TryParse(root["SmokeGrenadeCount"].InnerText, out _)) return false;

            XmlNode skillsNode = root["Skills"];
            if (skillsNode != null)
            {
                foreach (XmlNode skillNode in skillsNode.ChildNodes)
                {
                    string skillName = skillNode.SelectSingleNode("Name")?.InnerText;
                    if (string.IsNullOrEmpty(skillName) || !SkillsStorage.ContainsKey(skillName)) return false;
                }
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool HasSkill<T>() where T : Skill
    {
        return Skills.OfType<T>().Any();
    }
    public static bool HasSkill(Skill skill)
    {
        if (skill == null) return false;

        foreach (var x in Skills)
        {
            if (x.GetType() == skill.GetType()) return true;
        }
        return false;
    }
    public static void AddSkill(Skill skill)
    {
        Skills.Add(skill);
    }
    public static bool RemoveSkill<T>() where T : Skill
    {
        foreach (Skill skill in Skills)
        {
            if (skill is T) return Skills.Remove(skill);
        }
        return false;
    }
    public static T GetSkill<T>() where T : Skill
    {
        foreach (Skill skill in Skills)
        {
            if (skill is T result) return result;
        }
        return null;
    }
    public static void ClearInventoryConsumables()
    {
        BandageCount = 0;
        FirstAidKitCount = 0;
        SmokeGrenadeCount = 0;
        SimpleGrenadeCount = 0;
    }
    private static XmlElement CreateElement(XmlDocument xmlDoc, string name, object value) // Для сохранения в .xml
    {
        XmlElement element = xmlDoc.CreateElement(name);
        element.InnerText = value.ToString();
        return element;
    }
}