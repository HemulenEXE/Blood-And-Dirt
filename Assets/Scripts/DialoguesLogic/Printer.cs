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
    private bool IsAnim = false; //Происходит ли анимация тектса
    public int _rInd { get; private set; } //Аналог _replicInd из ShowDialogue. Отслеживает позицию в тексте

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

        _rInd = 0;
        _lineHight = _prefab.fontSize + 4f;
        _panel.gameObject.SetActive(true);
        _lineWidth = _panel.GetComponent<RectTransform>().rect.width;
        _hightPanel = _panel.GetComponent<RectTransform>().rect.height;
        _panel.gameObject.SetActive(false);
    }
    /// <summary>
    /// Если побукыенная печать была остановлена на моменте печати текста с анимацией/форматированием, то этот метод вначале допечатывает этот текст, а потом переходит к быстрой печати
    /// </summary>
    /// <param name="_replicInd"></param>
    /// <param name="text"></param>
    public void PrintReplicEntirely(int _replicInd, string text)
    {
        if (IsAnim == true)
        {
            Match m = Regex.Match(text, @"<([^>]+)>(.*?)<\/[^>]+>");
            while (!(m.Index < _replicInd && _replicInd < m.Index + m.Length))
                m = m.NextMatch();
            switch (m.Groups[1].Value)
            {
                case "wave":
                    {
                        List<string> chars = SplitForSimbols(m.Groups[2].Value);
                        for (int i = _replicInd - m.Groups[2].Index; i < chars.Count; i++)
                        {
                            GameObject letter = AddLetter(chars[i], false);
                            float hight = currentY + _lineHight * 0.5f + Mathf.Sin(Time.time * (TimeBetweenLetters / i) + 0.4f * i) * (_lineHight * 0.5f);
                            letter.transform.DOLocalMoveY(hight, 0.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                        }
                        break;
                    }
                case "shake":
                    {
                        List<string> chars = SplitForSimbols(m.Groups[2].Value);
                        for (int j = _replicInd - m.Groups[2].Index; j < chars.Count; j++)
                        {
                            GameObject letter = AddLetter(chars[j], false);
                            letter.transform.DOShakePosition(1f, 2.5f).SetLoops(-1);
                        }
                        break;
                    }
                default:
                    {
                        List<string> chars = SplitForSimbols(m.Value);
                        for (int i = _replicInd - m.Index - m.Groups[1].Length - 2; i < chars.Count; i++)
                            AddLetter(chars[i], false);
                        break;
                    }
            }
            _audio.Play();
            IsAnim = false;
            _replicInd += m.Length - (_replicInd - m.Index);
            _PrintReplicEntirely(_replicInd, text);
        }
        else _PrintReplicEntirely(_replicInd, text);
    }
    /// <summary>
    /// Печатает реплику быстро
    /// </summary>
    /// <param name="from"></param>
    /// <param name="last"></param>
    public void _PrintReplicEntirely(int _replicInd, string text)
    {
        _rInd = _replicInd;
        Debug.Log("PrintReplicEntirely запущен!");
        
        while (_rInd < text.Length - 1)
        {
            Debug.Log(_rInd);
            int i = _rInd;
            float length = currentX;
            while ((i != -1) && (length < _lineWidth / 2))
            { 
                i = text.IndexOf(" ", i + 1);
                length += (text.IndexOf(" ", i + 1) - i + 1) * 20f;
            }
            if (i == -1)
                i = text.Length - 1;
            if (text.IndexOf("<", _rInd) != -1)
                i = Math.Min(i, text.IndexOf("<", _rInd) - 1);

            Debug.Log(text.Substring(_rInd, i - _rInd + 1));
            AddLetter(text.Substring(_rInd, i - _rInd + 1), false);
            _rInd = i; 
            
            
            Match m = Regex.Match(text[_rInd..], @"<([^>]+)>(.*?)<\/([^>]+)>");
            int ind = text.Length - 1 - (text.Length - _rInd) + m.Index; 
            if (ind == _rInd) 
            {
                _rInd += m.Groups[1].Length + 2;
                Debug.Log(m.Value);
                switch (m.Groups[1].Value)
                {
                    case "wave":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            for (int j = 0; i < chars.Count; j++)
                            {
                                GameObject letter = AddLetter(chars[i], false);
                                float hight = currentY + _lineHight * 0.5f + Mathf.Sin(Time.time * (TimeBetweenLetters / i) + 0.4f * i) * (_lineHight * 0.5f);
                                letter.transform.DOLocalMoveY(hight, 0.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                            }
                            break;
                        }
                    case "shake":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            for (int j = 0; j < chars.Count; j++)
                            {
                                GameObject letter = AddLetter(chars[j], false);
                                letter.transform.DOShakePosition(1f, 2.5f).SetLoops(-1);
                            }
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
                _rInd += m.Groups[3].Length + 4;
            }
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
        _rInd = _replicInd;
        Debug.Log("PrintReplicGradually запущен!");

        currentX = -_lineWidth / 2;
        currentY = _hightPanel / 2 - 20f;

        while (_rInd < text.Length)
        {
            if (text[_rInd] != '<') //текст без анимаций, с обычны форматированием
            {
                AddLetter(text[_rInd].ToString(), true);
            }
            else//текст с необычным форматированием и/или анимацией 
            {
                IsAnim = true;
                Match m = Regex.Match(text[_rInd..], @"<([^>]+)>(.*?)<\/([^>]+)>");
                Debug.Log(m.Value);
                _rInd += m.Groups[1].Length + 2;
                switch (m.Groups[1].Value)
                {
                    case "wave":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            for (int i = 0; i < chars.Count; i++)
                            {
                                GameObject letter = AddLetter(chars[i], true);
                                letter.transform.DOLocalMoveY(currentY + _lineHight * 0.5f, 0.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                                yield return new WaitForSeconds(TimeBetweenLetters);
                            }
                            break;
                        }
                    case "shake":
                        {
                            List<string> chars = SplitForSimbols(m.Groups[2].Value);
                            for (int i = 0; i < chars.Count; i++)
                            {
                                GameObject letter = AddLetter(chars[i], true);
                                letter.transform.DOShakePosition(1f, 2.5f).SetLoops(-1);
                                yield return new WaitForSeconds(TimeBetweenLetters);
                            }
                            break;
                        }
                    default:
                        {
                            List<string> chars = SplitForSimbols(m.Value);
                            foreach (var c in chars)
                            {
                                AddLetter(c, true);
                                yield return new WaitForSeconds(TimeBetweenLetters);
                            }
                            break;
                        }
                }
                IsAnim = false;
                _rInd += m.Groups[3].Length + 4;
            }
            yield return new WaitForSeconds(TimeBetweenLetters);
        }
        _rInd--;
        Debug.Log($"_replicInd = {_replicInd}");
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
        Debug.Log($"currentX = {currentX}");
        if (playAudio)
            _audio.Play();

        _rInd++;
        return letter.gameObject;
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
