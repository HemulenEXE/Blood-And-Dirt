using SkillLogic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

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
        { "StartOfANewLife", new StartOfANewLife() }};

    public static HashSet<Skill> Skills = new HashSet<Skill>();

    public static int MaxHealth { get; set; }
    public static int CurrentHealth { get; set; }
    public static bool IsGod { get; set; } // Неузвимость
    public static int ResurrectionCount { get; set; } // Количество воскрешений
    public static int HitsToSurvive { get; set; } // Количество пропускаемых ударов

    public static bool IsStealing { get; set; }
    public static bool IsWalking { get; set; }
    public static bool IsRunning { get; set; }

    public static float StealSpeed { get; set; }
    public static float WalkSpeed { get; set; }
    public static float RunSpeed { get; set; }

    public static float StealNoise { get; set; }
    public static float WalkNoise { get; set; }
    public static float RunNoise { get; set; }

    public static int BleedingDamage { get; set; }
    public static bool IsBleeding { get; set; }

    public static int BandageCount { get; set; }
    public static int FirstAidKitCount { get; set; }
    public static int SimpleGrenadeCount { get; set; }
    public static int SmokeGrenadeCount { get; set; }

    public static int BandageHealth { get; set; } // Сколько бинт восстанавливает здоровья
    public static int FirstAidKitHealth { get; set; } // Сколько аптечка восстанавливает здоровья

    public static int Score { get; set; } // Количество очков для прокачки

    public static int CountArmor;

    public static void LoadData()
    {
        if (File.Exists(_savedPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);
            XmlNode root = xmlDoc.DocumentElement;

            MaxHealth = int.Parse(root.SelectSingleNode("MaxHealth").InnerText);
            CurrentHealth = int.Parse(root.SelectSingleNode("CurrentHealth").InnerText);
            HitsToSurvive = int.Parse(root.SelectSingleNode("HitsToSurvive").InnerText);
            IsGod = bool.Parse(root.SelectSingleNode("IsGod").InnerText);
            ResurrectionCount = int.Parse(root.SelectSingleNode("ResurrectionCount").InnerText);

            StealSpeed = float.Parse(root.SelectSingleNode("StealSpeed").InnerText);
            WalkSpeed = float.Parse(root.SelectSingleNode("WalkSpeed").InnerText);
            RunSpeed = float.Parse(root.SelectSingleNode("RunSpeed").InnerText);

            StealNoise = float.Parse(root.SelectSingleNode("StealNoise").InnerText);
            WalkNoise = float.Parse(root.SelectSingleNode("WalkNoise").InnerText);
            RunNoise = float.Parse(root.SelectSingleNode("RunNoise").InnerText);

            BleedingDamage = int.Parse(root.SelectSingleNode("BleedingDamage").InnerText);
            IsBleeding = bool.Parse(root.SelectSingleNode("IsBleeding").InnerText);

            CountArmor = int.Parse(root.SelectSingleNode("CountArmor").InnerText);

            BandageHealth = int.Parse(root.SelectSingleNode("BandageHealth").InnerText);
            FirstAidKitHealth = int.Parse(root.SelectSingleNode("FirstAidKitHealth").InnerText);
            HitsToSurvive = int.Parse(root.SelectSingleNode("HitsToSurvive").InnerText);
            IsGod = bool.Parse(root.SelectSingleNode("IsGod").InnerText);

            BandageCount = int.Parse(root.SelectSingleNode("BandageCount").InnerText);
            FirstAidKitCount = int.Parse(root.SelectSingleNode("FirstAidKitCount").InnerText);
            SimpleGrenadeCount = int.Parse(root.SelectSingleNode("SimpleGrenadeCount").InnerText);
            SmokeGrenadeCount = int.Parse(root.SelectSingleNode("SmokeGrenadeCount").InnerText);

            Score = int.Parse(root.SelectSingleNode("Score").InnerText);

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
            MaxHealth = 100;
            CurrentHealth = 100;
            HitsToSurvive = 0;
            IsGod = false;
            ResurrectionCount = 0;

            StealSpeed = 2f;
            WalkSpeed = 4f;
            RunSpeed = 6f;

            StealNoise = 0.3f;
            WalkNoise = 2f;
            RunNoise = 5f;

            BleedingDamage = 5;
            IsBleeding = false;
            CountArmor = 0;
            BandageHealth = 15;
            FirstAidKitHealth = 30;
        }
    }
    public static void SaveData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("PlayerData");
        xmlDoc.AppendChild(root);

        root.AppendChild(CreateElement(xmlDoc, "MaxHealth", MaxHealth));
        root.AppendChild(CreateElement(xmlDoc, "CurrentHealth", CurrentHealth));
        root.AppendChild(CreateElement(xmlDoc, "HitsToSurvive", HitsToSurvive));
        root.AppendChild(CreateElement(xmlDoc, "IsGod", IsGod));
        root.AppendChild(CreateElement(xmlDoc, "ResurrectionCount", ResurrectionCount));

        root.AppendChild(CreateElement(xmlDoc, "StealSpeed", StealSpeed));
        root.AppendChild(CreateElement(xmlDoc, "WalkSpeed", WalkSpeed));
        root.AppendChild(CreateElement(xmlDoc, "RunSpeed", RunSpeed));

        root.AppendChild(CreateElement(xmlDoc, "StealNoise", StealNoise));
        root.AppendChild(CreateElement(xmlDoc, "WalkNoise", WalkNoise));
        root.AppendChild(CreateElement(xmlDoc, "RunNoise", RunNoise));

        root.AppendChild(CreateElement(xmlDoc, "BleedingDamage", BleedingDamage));
        root.AppendChild(CreateElement(xmlDoc, "IsBleeding", IsBleeding));

        root.AppendChild(CreateElement(xmlDoc, "CountArmor", CountArmor));

        root.AppendChild(CreateElement(xmlDoc, "BandageCount", BandageCount));
        root.AppendChild(CreateElement(xmlDoc, "FirstAidKitCount", FirstAidKitCount));
        root.AppendChild(CreateElement(xmlDoc, "SimpleGrenadeCount", SimpleGrenadeCount));
        root.AppendChild(CreateElement(xmlDoc, "SmokeGrenadeCount", SmokeGrenadeCount));

        root.AppendChild(CreateElement(xmlDoc, "BandageHealth", BandageHealth));
        root.AppendChild(CreateElement(xmlDoc, "FirstAidKitHealth", FirstAidKitHealth));

        root.AppendChild(CreateElement(xmlDoc, "Score", Score));

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
    private static XmlElement CreateElement(XmlDocument xmlDoc, string name, object value) // Для сохранения в .xml
    {
        XmlElement element = xmlDoc.CreateElement(name);
        element.InnerText = value.ToString();
        return element;
    }
}