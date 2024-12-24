using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Linq;

/// <summary>
/// Класс, отвечающий за вывод текста на экран. Скрипт навешивается на NPS
/// </summary>
public class Printer : MonoBehaviour
{
    private Transform _panel;
    private float TimeBetweenLetters;
    private TextMeshProUGUI _prefab; //Префаб буквы
    private float _lineHight; //Высота строки
    private float _lineWidth; //Ширина строки
    private float _hightPanel; //Высота панели
    private float currentX = 0f; //Текущая позиция буквы по Х
    private float currentY = 0f; //Текущая позиция буквы по Y
    private AudioSource _audio;
    public int _rInd = 0; //Аналог _replicInd из ShowDialogue. Отслеживает позицию в тексте

    /// <summary>
    /// Инициализация всех (кроме текста) полей
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="time"></param>
    public void Init(Transform panel, float time, AudioSource audio) 
    {
        Debug.Log("PRINTER создан!");
        _prefab = Resources.Load<TextMeshProUGUI>("Prefabs/Interface/Letter");
        _panel = panel;
        TimeBetweenLetters = time;
        _audio = audio;

        _lineHight = _prefab.fontSize + 2f;
        _panel.gameObject.SetActive(true);
        _lineWidth = _panel.GetComponent<RectTransform>().rect.width;
        _hightPanel = _panel.GetComponent<RectTransform>().rect.height;
        _panel.gameObject.SetActive(false);
    }
    /// <summary>
    /// Печатает реплику быстро
    /// </summary>
    /// <param name="from"></param>
    /// <param name="last"></param>
    public void PrintReplicEntirely(int _replicInd, string text)
    {
        Debug.Log("PrintReplicEntirely запущен!");
        
        while (_replicInd < text.Length - 1)
        {
            Debug.Log(_replicInd);
            int i = _replicInd;
            float length = currentX;
            while ((i != -1) && (length < _lineWidth / 2))
            { 
                i = text.IndexOf(" ", i + 1);
                length += (text.IndexOf(" ", i + 1) - i + 1) * 20f;
            }
            if (i == -1)
                i = text.Length - 1;
            if (text.IndexOf("<", _replicInd) != -1)
                i = Math.Min(i, text.IndexOf("<", _replicInd) - 1);

            //Ошибка - если i = text.IndexOf("<", _replicInd), ТО печатается начало тега и потом тег находиться неправильно
            Debug.Log(text.Substring(_replicInd, i - _replicInd + 1));
            AddLetter(text.Substring(_replicInd, i - _replicInd + 1), false);
            _replicInd = i; 
            
            
            Match m = Regex.Match(text[_replicInd..], @"<([^>]+)>(.*?)<\/[^>]+>");
            int ind = text.Length - 1 - (text.Length - _replicInd) + m.Index; 
            if (ind == _replicInd) 
            { 
                Debug.Log(m.Value);
                switch (m.Groups[1].Value)
                {
                    case "wave":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            StartCoroutine(PrintWave(chars, 0f, null));
                            break;
                        }
                    case "shake":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            StartCoroutine(PrintShake(chars, 0f, null));
                            break;
                        }
                    default:
                        {
                            List<string> chars = SplitForSimbols(m.Value);
                            foreach (var c in chars)
                                AddLetter(c, false);
                            break;
                        }
                }
                _replicInd += m.Length + 1;
            }
            _rInd = _replicInd;
        }
        _audio.Play();
        currentX = -_lineWidth / 2;
        currentY = _hightPanel / 2 - 20f;
    }
    public void PrintReplicGradually(int _replcInd, string text)
    {
        Debug.Log($"ind = {_replcInd}, text = {text}");
      StartCoroutine(PrintReplicGraduallyCorutain(_replcInd, text));    
    }
    /// <summary>
    /// Печатет реплику побуквенно с помощью DOTween
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public IEnumerator PrintReplicGraduallyCorutain(int _replicInd, string text)
    {
        Debug.Log("PrintReplicGradually запущен!");

        currentX = -_lineWidth / 2;
        currentY = _hightPanel / 2 - 20f;

        while (_replicInd < text.Length)
        {
            if (text[_replicInd] != '<') //текст без анимаций, с обычны форматированием
            {
                AddLetter(text[_replicInd].ToString(), true);
                _replicInd++;
            }
            else//текст с необычным форматированием и/или анимацией 
            {
                Match m = Regex.Match(text[_replicInd..], @"<([^>]+)>(.*?)<\/[^>]+>");
                Debug.Log(m.Value);
                switch (m.Groups[1].Value)
                {
                    case "wave":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            bool wait = true;
                            StartCoroutine(PrintWave(chars, TimeBetweenLetters, ()=> wait = false));
                            while (wait)
                                yield return null;
                            break;
                        }
                    case "shake":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            bool wait = true;
                            StartCoroutine(PrintShake(chars, TimeBetweenLetters, () => wait = false));
                            while (wait)
                                yield return null;
                            break;
                        }
                    default:
                        {
                            List<string> chars = SplitForSimbols(m.Value);
                            foreach (var c in chars)
                                AddLetter(c, true);
                            break;
                        }
                }
                _replicInd += m.Length;
            }
            _rInd = _replicInd;
            yield return new WaitForSeconds(TimeBetweenLetters);
        }
        _replicInd--;
        Debug.Log($"_replicInd = {_replicInd}");
        _rInd = _replicInd;
    }
    /// <summary>
    /// Печатает поданный текст на панеле
    /// </summary>
    /// <param name="text"></param>
    /// <param name="playAudio"></param>
    /// <returns></returns>
    private GameObject AddLetter(string text, bool playAudio)
    {
        Debug.Log($"current sibol = {text}");
        TextMeshProUGUI letter = Instantiate(_prefab, _panel);
        letter.text = text;
        letter.ForceMeshUpdate(); //Обновление для получения актуальных размеров

        float letterWidth = Math.Max(letter.GetPreferredValues().x, 7f); //ширина символа не меньше 7f
        if (text.Length > 1 && text[^1] == ' ') letterWidth += 7f; //Для учёта пробелов в конце фраз

        Debug.Log($"letterWidth = {letterWidth}, currentX = {currentX}") ;
        if (currentX + letterWidth > _lineWidth / 2)
        {
            Debug.Log($"{currentX} + {letterWidth} = {currentX + letterWidth} > {_lineWidth}");
            currentX = -_lineWidth / 2;
            currentY -= _lineHight;
        }
        letter.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);
        currentX += letterWidth + 0.5f;

        if (playAudio)
            _audio.Play();

        return letter.gameObject;
    }
    /// <summary>
    /// Печатает трясущиеся символы
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator PrintShake(List<string> text, float time, Action callback)
    {
        float strength = 0.5f; //Сила тряски букв
        
        for (int i = 0; i < text.Count; i++)
        {
            GameObject letter;
            if (time == 0f)
                letter = AddLetter(text[i], false);
            else letter = AddLetter(text[i], true);

            letter.transform.DOShakePosition(1f, strength).SetLoops(-1);
            yield return new WaitForSeconds(time);
        }
        callback?.Invoke();
    }
    /// <summary>
    /// Печатает символы волной
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator PrintWave(List<string> text, float time, Action callback)
    {
        float startOffset = -1f; 
        for (int i = 0; i < text.Count; i++)
        {
            GameObject letter;
            if (time == 0f)
                letter = AddLetter(text[i], false);
            else letter = AddLetter(text[i], true);

            letter.transform.DOMoveY(letter.transform.position.y - startOffset, 0.5f).From(letter.transform.position.y + startOffset).SetLoops(-1);
            startOffset *= -1f;
            yield return new WaitForSeconds(time);
        }
        callback?.Invoke();
    }
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
