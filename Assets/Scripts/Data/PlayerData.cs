using SkillLogic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public static class PlayerData
{
    private static string _savedPath = "C:\\Users\\Amethyst\\Desktop\\Downloads\\PlayerData.xml";
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

    public static HashSet<Skill> Skills = new HashSet<Skill>();

    public static int MaxHealth { get; set; } = 100;
    public static int CurrentHealth { get; set; } = 100;
    public static bool IsGod { get; set; } = false; // Неузвимость
    public static int ResurrectionCount { get; set; } = 0; // Количество воскрешений
    public static int CurrentResurrectionCount { get; set; } = 0;
    public static int HitsToSurvive { get; set; } = 0; // Количество пропускаемых ударов
    public static int CurrentHitsToSurvive { get; set; } = 0;

    public static bool IsStealing { get; set; } = false;
    public static bool IsWalking { get; set; } = false;
    public static bool IsRunning { get; set; } = false;

    public static float StealSpeed { get; set; } = 2;
    public static float WalkSpeed { get; set; } = 4;
    public static float RunSpeed { get; set; } = 6;

    public static float StealNoise { get; set; } = 0.3f;
    public static float WalkNoise { get; set; } = 2f;
    public static float RunNoise { get; set; } = 5f;

    public static int BleedingDamage { get; set; } = 5;
    public static bool IsBleeding { get; set; } = false;

    public static int BandageCount { get; set; } = 0;
    public static int MaxBandageCount { get; set; } = 5;

    public static int FirstAidKitCount { get; set; } = 0;
    public static int MaxFirstAidKitCount { get; set; } = 5;

    public static int SimpleGrenadeCount { get; set; } = 0;
    public static int MaxSimpleGrenadeCount { get; set; } = 5;

    public static int SmokeGrenadeCount { get; set; } = 0;
    public static int MaxSmokeGrenadeCount { get; set; } = 5;

    public static int BandageHealth { get; set; } = 15; // Сколько бинт восстанавливает здоровья
    public static int FirstAidKitHealth { get; set; } = 30; // Сколько аптечка восстанавливает здоровья

    public static int Score { get; set; } = 10_000; // Количество очков для прокачки

    public static int CountArmor;

    public static int InventoryCapacity { get; set; } = 3; // Число слотов

    public static void LoadData()
    {
        string directoryPath = Path.GetDirectoryName(_savedPath);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        if (File.Exists(_savedPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);
            XmlNode root = xmlDoc.DocumentElement;

            //MaxHealth = int.Parse(root.SelectSingleNode("MaxHealth").InnerText);
            //CurrentHealth = int.Parse(root.SelectSingleNode("CurrentHealth").InnerText);
            //HitsToSurvive = int.Parse(root.SelectSingleNode("HitsToSurvive").InnerText);
            //IsGod = bool.Parse(root.SelectSingleNode("IsGod").InnerText);
            //ResurrectionCount = int.Parse(root.SelectSingleNode("ResurrectionCount").InnerText);

            //StealSpeed = float.Parse(root.SelectSingleNode("StealSpeed").InnerText);
            //WalkSpeed = float.Parse(root.SelectSingleNode("WalkSpeed").InnerText);
            //RunSpeed = float.Parse(root.SelectSingleNode("RunSpeed").InnerText);

            //StealNoise = float.Parse(root.SelectSingleNode("StealNoise").InnerText);
            //WalkNoise = float.Parse(root.SelectSingleNode("WalkNoise").InnerText);
            //RunNoise = float.Parse(root.SelectSingleNode("RunNoise").InnerText);

            //BleedingDamage = int.Parse(root.SelectSingleNode("BleedingDamage").InnerText);
            //IsBleeding = bool.Parse(root.SelectSingleNode("IsBleeding").InnerText);

            //CountArmor = int.Parse(root.SelectSingleNode("CountArmor").InnerText);

            //BandageHealth = int.Parse(root.SelectSingleNode("BandageHealth").InnerText);
            //FirstAidKitHealth = int.Parse(root.SelectSingleNode("FirstAidKitHealth").InnerText);
            //HitsToSurvive = int.Parse(root.SelectSingleNode("HitsToSurvive").InnerText);
            //IsGod = bool.Parse(root.SelectSingleNode("IsGod").InnerText);

            //BandageCount = int.Parse(root.SelectSingleNode("BandageCount").InnerText);
            //FirstAidKitCount = int.Parse(root.SelectSingleNode("FirstAidKitCount").InnerText);
            //SimpleGrenadeCount = int.Parse(root.SelectSingleNode("SimpleGrenadeCount").InnerText);
            //SmokeGrenadeCount = int.Parse(root.SelectSingleNode("SmokeGrenadeCount").InnerText);

            //MaxBandageCount = int.Parse(root.SelectSingleNode("MaxBandageCount").InnerText);
            //MaxFirstAidKitCount = int.Parse(root.SelectSingleNode("MaxFirstAidKitCount").InnerText);
            //MaxSmokeGrenadeCount = int.Parse(root.SelectSingleNode("MaxSmokeGrenadeCount").InnerText);
            //MaxSimpleGrenadeCount = int.Parse(root.SelectSingleNode("MaxSimpleGrenadeCount").InnerText);

            //CurrentHitsToSurvive = int.Parse(root.SelectSingleNode("CurrentHitsToSurvive").InnerText);
            //CurrentResurrectionCount = int.Parse(root.SelectSingleNode("CurrentResurrectionCount").InnerText);

            //Score = int.Parse(root.SelectSingleNode("Score").InnerText);

            //InventoryCapacity = int.Parse(root.SelectSingleNode("InventoryCapacity").InnerText);

            Skills.Clear();

            XmlNode skillsNode = root.SelectSingleNode("Skills");
            if (skillsNode != null)
            {
                foreach (XmlNode x in skillsNode.ChildNodes)
                {
                    string name = x.SelectSingleNode("Name").InnerText;
                    bool isUnlocked = bool.Parse(x.SelectSingleNode("IsUnlocked").InnerText);
                    if (SkillsStorage.TryGetValue(name, out Skill skill)) Skills.Add(skill);
                }
            }
        }
        else
        {
            // Значения по умолчанию

            Skills.Clear();

            //MaxHealth = 100;
            //CurrentHealth = MaxHealth;
            //HitsToSurvive = 0;
            //IsGod = false;
            //ResurrectionCount = 0;

            //StealSpeed = 2f;
            //WalkSpeed = 4f;
            //RunSpeed = 6f;

            //StealNoise = 0.3f;
            //WalkNoise = 2f;
            //RunNoise = 5f;

            //BleedingDamage = 5;
            //IsBleeding = false;

            //CountArmor = 0;

            //BandageHealth = 15;
            //FirstAidKitHealth = 30;

            //BandageCount = 0;
            //MaxBandageCount = 5;

            //FirstAidKitCount = 0;
            //MaxFirstAidKitCount = 5;

            //SmokeGrenadeCount = 0;
            //MaxSmokeGrenadeCount = 5;

            //SimpleGrenadeCount = 0;
            //MaxSimpleGrenadeCount = 5;

            //CurrentResurrectionCount = ResurrectionCount;
            //CurrentHitsToSurvive = HitsToSurvive;

            //Score = 0;

            //InventoryCapacity = 3;
        }
    }
    public static void SaveData()
    {
        string directoryPath = Path.GetDirectoryName(_savedPath);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("PlayerData");
        xmlDoc.AppendChild(root);

        //root.AppendChild(CreateElement(xmlDoc, "MaxHealth", MaxHealth));
        //root.AppendChild(CreateElement(xmlDoc, "CurrentHealth", CurrentHealth));
        //root.AppendChild(CreateElement(xmlDoc, "HitsToSurvive", HitsToSurvive));
        //root.AppendChild(CreateElement(xmlDoc, "IsGod", IsGod));
        //root.AppendChild(CreateElement(xmlDoc, "ResurrectionCount", ResurrectionCount));

        //root.AppendChild(CreateElement(xmlDoc, "StealSpeed", StealSpeed));
        //root.AppendChild(CreateElement(xmlDoc, "WalkSpeed", WalkSpeed));
        //root.AppendChild(CreateElement(xmlDoc, "RunSpeed", RunSpeed));

        //root.AppendChild(CreateElement(xmlDoc, "StealNoise", StealNoise));
        //root.AppendChild(CreateElement(xmlDoc, "WalkNoise", WalkNoise));
        //root.AppendChild(CreateElement(xmlDoc, "RunNoise", RunNoise));

        //root.AppendChild(CreateElement(xmlDoc, "BleedingDamage", BleedingDamage));
        //root.AppendChild(CreateElement(xmlDoc, "IsBleeding", IsBleeding));

        //root.AppendChild(CreateElement(xmlDoc, "CountArmor", CountArmor));

        //root.AppendChild(CreateElement(xmlDoc, "BandageCount", BandageCount));
        //root.AppendChild(CreateElement(xmlDoc, "FirstAidKitCount", FirstAidKitCount));
        //root.AppendChild(CreateElement(xmlDoc, "SimpleGrenadeCount", SimpleGrenadeCount));
        //root.AppendChild(CreateElement(xmlDoc, "SmokeGrenadeCount", SmokeGrenadeCount));

        //root.AppendChild(CreateElement(xmlDoc, "BandageHealth", BandageHealth));
        //root.AppendChild(CreateElement(xmlDoc, "FirstAidKitHealth", FirstAidKitHealth));

        //root.AppendChild(CreateElement(xmlDoc, "MaxBandageCount", MaxBandageCount));
        //root.AppendChild(CreateElement(xmlDoc, "MaxFirstAidKitCount", MaxFirstAidKitCount));
        //root.AppendChild(CreateElement(xmlDoc, "MaxSmokeGrenadeCount", MaxSmokeGrenadeCount));
        //root.AppendChild(CreateElement(xmlDoc, "MaxSimpleGrenadeCount", MaxSimpleGrenadeCount));

        //root.AppendChild(CreateElement(xmlDoc, "CurrentResurrectionCount", CurrentResurrectionCount));
        //root.AppendChild(CreateElement(xmlDoc, "CurrentHitsToSurvive", CurrentHitsToSurvive));

        //root.AppendChild(CreateElement(xmlDoc, "Score", Score));

        //root.AppendChild(CreateElement(xmlDoc, "InventoryCapacity", InventoryCapacity));

        XmlElement skillsElement = xmlDoc.CreateElement("Skills");
        foreach (var skill in Skills)
        {
            XmlElement skillElement = xmlDoc.CreateElement("Skill");
            skillElement.AppendChild(CreateElement(xmlDoc, "Name", skill.Name));
            skillsElement.AppendChild(skillElement);
        }
        root.AppendChild(skillsElement);

        xmlDoc.Save(_savedPath);
    }
    public static void Reboot()
    {
        MaxHealth = 100;
        CurrentHealth = 100;
        IsGod = false;
        ResurrectionCount = 0;
        HitsToSurvive = 0;
        CurrentHitsToSurvive = 0;
        CurrentResurrectionCount = 0;

        IsStealing = false;
        IsWalking = false;
        IsRunning = false;

        StealSpeed = 2;
        WalkSpeed = 4;
        RunSpeed = 6;

        StealNoise = 0.3f;
        WalkNoise = 2f;
        RunNoise = 5f;

        BleedingDamage = 5;
        IsBleeding = false;

        BandageCount = 0;
        MaxBandageCount = 5;

        FirstAidKitCount = 0;
        MaxFirstAidKitCount = 5;

        SimpleGrenadeCount = 0;
        MaxSimpleGrenadeCount = 5;

        SmokeGrenadeCount = 0;
        MaxSmokeGrenadeCount = 5;

        BandageHealth = 15;
        FirstAidKitHealth = 30;

        Score = 10_000;

        InventoryCapacity = 3;
        Skills.Clear();

        if (File.Exists(_savedPath))
        {
            File.Delete(_savedPath);
        }
    }
    public static bool HasSkill<T>() where T : Skill
    {
        return Skills.OfType<T>().Any();
    }
    public static bool HasSkill(Skill skill)
    {
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