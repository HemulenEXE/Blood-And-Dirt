using SkillLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace PlayerLogic
{
    public static class PlayerInfo
    {

        public static float _stealSpeed = 2f;
        public static float _walkSpeed = 4f;
        public static float _runSpeed = 6f;

        public static bool _isStealing;
        public static bool _isWalking;
        public static bool _isRunning;
        public static bool _isStaying;

        public static float _stealNoise = 0.3f;
        public static float _walkNoise = 2f;
        public static float _runNoise = 8f;

        public static int _fullHealth = 100;
        public static int _currentHealth = 100;

        public static int _bleedingDamage;

        public static bool _isBleeding;

        public static int _countArmor;
        public static int _bandageHealth;
        public static int _firstAidKitHealth;

        public static int _hitsToSurvive = 0;

        public static bool _isGod; // Неузвимость

        public static int _bodyCount; // Количество воскрешений

        private static string _savedPath = "";

        public static HashSet<Skill> _skillSet = new HashSet<Skill>();

        public static bool HasSkill<T>() where T : Skill
        {
            return _skillSet.OfType<T>().Any();
        }
        public static bool HasSkill(Skill skill)
        {
            foreach(var x in _skillSet)
            {
                if (x.GetType() == skill.GetType()) return true;
            }
            return false;
        }

        public static void AddSkill(Skill skill)
        {
            _skillSet.Add(skill);
        }
        public static bool RemoveSkill<T>() where T : Skill
        {
            foreach (Skill skill in _skillSet)
            {
                if (skill is T) return _skillSet.Remove(skill);
            }
            return false;
        }
        public static T GetSkill<T>() where T : Skill
        {
            foreach (Skill skill in _skillSet)
            {
                if (skill is T)
                {
                    return skill as T;
                }
            }
            return null;
        }
        public static void SaveData()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("PlayerData");
            xmlDoc.AppendChild(root);

            root.AppendChild(CreateElement(xmlDoc, "stealSpeed", _stealSpeed));
            root.AppendChild(CreateElement(xmlDoc, "walkSpeed", _walkSpeed));
            root.AppendChild(CreateElement(xmlDoc, "runSpeed", _runSpeed));
            root.AppendChild(CreateElement(xmlDoc, "stealNoise", _stealNoise));
            root.AppendChild(CreateElement(xmlDoc, "walkNoise", _walkNoise));
            root.AppendChild(CreateElement(xmlDoc, "runNoise", _runNoise));
            root.AppendChild(CreateElement(xmlDoc, "fullHealth", _fullHealth));
            root.AppendChild(CreateElement(xmlDoc, "currentHealth", _currentHealth));
            root.AppendChild(CreateElement(xmlDoc, "bleedingDamage", _bleedingDamage));
            root.AppendChild(CreateElement(xmlDoc, "isBleeding", _isBleeding));
            root.AppendChild(CreateElement(xmlDoc, "countArmor", _countArmor));
            root.AppendChild(CreateElement(xmlDoc, "bandageHealth", _bandageHealth));
            root.AppendChild(CreateElement(xmlDoc, "firstAidKitHealth", _firstAidKitHealth));
            root.AppendChild(CreateElement(xmlDoc, "hitsToSurvive", _hitsToSurvive));
            root.AppendChild(CreateElement(xmlDoc, "isGod", _isGod));
            root.AppendChild(CreateElement(xmlDoc, "bodyCount", _bodyCount));
            XmlElement skillsElement = xmlDoc.CreateElement("Skills");
            foreach (var skill in _skillSet)
            {
                XmlElement skillElement = xmlDoc.CreateElement("Skill");
                skillElement.AppendChild(CreateElement(xmlDoc, "name", skill._name));
                skillElement.AppendChild(CreateElement(xmlDoc, "isUnlocked", skill._isUnlocked));
                skillsElement.AppendChild(skillElement);
            }
            root.AppendChild(skillsElement);

            xmlDoc.Save(_savedPath);
        }
        private static XmlElement CreateElement(XmlDocument xmlDoc, string name, object value)
        {
            XmlElement element = xmlDoc.CreateElement(name);
            element.InnerText = value.ToString();
            return element;
        }
        public static void LoadData()
        {
            if (!File.Exists(_savedPath))
            {
                UnityEngine.Debug.Log("Файл PlayerData.xml не найден.");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_savedPath);
            XmlNode root = xmlDoc.DocumentElement;

            _stealSpeed = float.Parse(root.SelectSingleNode("stealSpeed").InnerText);
            _walkSpeed = float.Parse(root.SelectSingleNode("walkSpeed").InnerText);
            _runSpeed = float.Parse(root.SelectSingleNode("runSpeed").InnerText);
            _stealNoise = float.Parse(root.SelectSingleNode("stealNoise").InnerText);
            _walkNoise = float.Parse(root.SelectSingleNode("walkNoise").InnerText);
            _runNoise = float.Parse(root.SelectSingleNode("runNoise").InnerText);
            _fullHealth = int.Parse(root.SelectSingleNode("fullHealth").InnerText);
            _currentHealth = int.Parse(root.SelectSingleNode("currentHealth").InnerText);
            _bleedingDamage = int.Parse(root.SelectSingleNode("bleedingDamage").InnerText);
            _isBleeding = bool.Parse(root.SelectSingleNode("isBleeding").InnerText);
            _countArmor = int.Parse(root.SelectSingleNode("countArmor").InnerText);
            _bandageHealth = int.Parse(root.SelectSingleNode("bandageHealth").InnerText);
            _firstAidKitHealth = int.Parse(root.SelectSingleNode("firstAidKitHealth").InnerText);
            _hitsToSurvive = int.Parse(root.SelectSingleNode("hitsToSurvive").InnerText);
            _isGod = bool.Parse(root.SelectSingleNode("isGod").InnerText);
            _bodyCount = int.Parse(root.SelectSingleNode("bodyCount").InnerText);

            _skillSet.Clear();
            XmlNode skillsNode = root.SelectSingleNode("Skills");
            if (skillsNode != null)
            {
                foreach (XmlNode x in skillsNode.ChildNodes)
                {
                    string name = x.SelectSingleNode("name").InnerText;
                    bool isUnlocked = bool.Parse(x.SelectSingleNode("isUnlocked").InnerText);
                    switch (name)
                    {
                        case "AnyPrice":
                            AnyPrice ap = new AnyPrice();
                            _skillSet.Add(ap);
                            break;
                        case "BlindRange":
                            BlindRange br = new BlindRange();
                            _skillSet.Add(br);
                            break;
                        case "DropByDrop":
                            DropByDrop dbd = new DropByDrop();
                            _skillSet.Add(dbd);
                            break;
                        case "Hatred":
                            Hatred h = new Hatred();
                            _skillSet.Add(h);
                            break;
                        case "IncreasedMetabolism":
                            IncreasedMetabolism im = new IncreasedMetabolism();
                            _skillSet.Add(im);
                            break;
                        case "LiveInNotVain":
                            LiveInNotVain lnv = new LiveInNotVain();
                            _skillSet.Add(lnv);
                            break;
                        case "MusclesSecondSkeleton":
                            MusclesSecondSkeleton mss = new MusclesSecondSkeleton();
                            _skillSet.Add(mss);
                            break;
                        case "MusclesSecondSkeleton2":
                            MusclesSecondSkeleton2 mss2 = new MusclesSecondSkeleton2();
                            _skillSet.Add(mss2);
                            break;
                        case "Reincarnation":
                            Reincarnation r = new Reincarnation();
                            _skillSet.Add(r);
                            break;
                        case "SledGrenade":
                            SledGrenade sg = new SledGrenade();
                            _skillSet.Add(sg);
                            break;
                        case "Sound":
                            Sound s = new Sound();
                            _skillSet.Add(s);
                            break;
                        case "Spin":
                            Spin s2 = new Spin();
                            _skillSet.Add(s2);
                            break;
                        case "StartOfANewLife":
                            StartOfANewLife soanl = new StartOfANewLife();
                            _skillSet.Add(soanl);
                            break;
                    }
                }
            }
        }
    }

}
