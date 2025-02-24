using System;
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
    private int _curentNode = 0;

    [System.Serializable]
    public class Node
    {
        [XmlAttribute("npcText")]
        public string npcText;
        [XmlAttribute("npcName")]
        public string npcName;
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
        [XmlAttribute("toScene")]
        public int toScene;
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
    /// <summary>
    /// Возвращает текущую рерлику
    /// </summary>
    /// <returns></returns>
    public Node GetCurentNode() { return Nodes[_curentNode]; }
    /// <summary>
    /// Переход к следующей реплике
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public void ToNextNode() {
        if (_curentNode + 1 >= Nodes.Length)
            throw new IndexOutOfRangeException();
        _curentNode++; 
    }
    /// <summary>
    /// Переход к реплике по индексу
    /// </summary>
    /// <param name="i"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ToNodeWithInd(int i) {
        if (i >= Nodes.Length)
            throw new ArgumentOutOfRangeException();
        _curentNode = i;
    }
}
