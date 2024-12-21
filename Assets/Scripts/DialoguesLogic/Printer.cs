using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Printer : MonoBehaviour
{
    private TextMeshProUGUI _replicText;
    private float TimeBetweenLetters;
    public int _rInd = 0; //Аналог _replicInd из ShowDialogue. Отслеживает позицию в тексте

    public void Init(TextMeshProUGUI text, float time) 
    {
        Debug.Log("PRINTER создан!");
        _replicText = text;
        TimeBetweenLetters = time;
        Debug.Log(_replicText);
    }
    /// <summary>
    /// Печать части реплики с from по </fast> целиком. Или если </fast> нет, то до конца реплики
    /// </summary>
    /// <param name="from"></param>
    /// <param name="last"></param>
    public void PrintReplicEntirely(ref int _replicInd, string text)
    {
        Debug.Log("PrintReplicEntirely запущен!");
        string txt = text;
        Match m = Regex.Match(txt[_replicInd..], @"<fast>(\w+?)</fast>");
        if (m.Success)
        {
            _replicText.text += m.Groups[1].Value;
            _replicInd += m.Length - 1; //После отпечатки части реплики быстро, нужно допечатать оставшееся побуквенно
        }
        else
        {
            _replicText.text = text;
            _replicInd = text.Length - 1;
        }
        _rInd = _replicInd;
    }
    public void PrintReplicGradually(int _replcInd, string text)
    {
        Debug.Log($"ind = {_replcInd}, text = {text}");
        Coroutine c = StartCoroutine(PrintReplicGraduallyCorutain(_replcInd, text));    
        Debug.Log($"_replicind in Corutine = {_replcInd}");
    }
    /// <summary>
    /// Печатет реплику побуквенно с помощью DOTween
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public IEnumerator PrintReplicGraduallyCorutain(int _replicInd, string text)
    {
        Debug.Log("PrintReplicGradually запущен!");

        while (_replicInd < text.Length)
        {
            if (text[_replicInd] != '<') //текст без анимаций, с обычны форматированием
            {
                _replicText.text += text[_replicInd];
                _replicInd++;
            }
            else//текст с необычным форматированием и/или анимацией 
            {

                Match m = Regex.Match(text[_replicInd..], @"<([^>]+)>(.*?)<\/[^>]+>");
                Debug.Log(m.Value);
                switch (m.Groups[1].Value)
                {
                    case "fast": PrintReplicEntirely(ref _replicInd, text); break;
                    case "wave":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            StartCoroutine(PrintWave(chars, TimeBetweenLetters));
                            break;
                        }
                    case "shake":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            StartCoroutine(PrintShake(chars, TimeBetweenLetters));
                            break;
                        }
                    default:
                        {
                            List<string> chars = SplitForSimbols(m.Value);
                            foreach (var c in chars)
                                _replicText.text += c;
                            _replicInd += m.Length;
                            break;
                        }
                }
            }
            _rInd = _replicInd;
            yield return new WaitForSeconds(TimeBetweenLetters);
        }
        _replicInd--;
        Debug.Log($"_replicInd = {_replicInd}");
        _rInd = _replicInd;
    }
    /// <summary>
    /// Печатает трясущиеся символы
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator PrintShake(List<string> text, float time)
    { yield return new WaitForSeconds(time); }
    /// <summary>
    /// Печатает символы волной
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator PrintWave(List<string> text, float time)
    { yield return new WaitForSeconds(time); }
    /// <summary>
    /// К каждому символу текста между тегами применяет теги и записывает их в массив
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private List<string> SplitForSimbols(string text)
    {
        List<string> res = new List<string>();
        Match m = Regex.Match(text, @"(<[^>]+>)(.*?)(<\/[^>]+>)");
        Debug.Log(m.Groups[2].Value);
        if (m.Success)
            foreach (char c in m.Groups[2].Value)
                res.Add(m.Groups[1].Value + c + m.Groups[3].Value);
        else
            foreach (char c in text)
                res.Add(c.ToString());
        return res;
    }
}
