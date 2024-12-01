using System.IO;
using System.Xml.Serialization;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Читает xml-файл и создаёт диалог (массив node'ов)
/// </summary>
[XmlRoot("dialogue")]
public class Dialogue
{
    [XmlElement("node")]
    public Node[] Nodes;

    [System.Serializable]
    public class Node
    {
        [XmlAttribute("npcText")]
        public string npcText;
        [XmlAttribute("exit")]
        public string exit;
        [XmlElement("answer")]
        public Answer[] answers;
    }
    [System.Serializable]
    public class Answer
    {
        [XmlAttribute("text")]
        public string text;
        [XmlAttribute("toNode")]
        public int toNode;
        [XmlAttribute("exit")]
        public string exit;
    }
    public static Dialogue Load(TextAsset _xml)
    { 
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue));
        StringReader sr = new StringReader(_xml.text);
        Dialogue dialogue = serializer.Deserialize(sr) as Dialogue;
        return dialogue;
    }
}
