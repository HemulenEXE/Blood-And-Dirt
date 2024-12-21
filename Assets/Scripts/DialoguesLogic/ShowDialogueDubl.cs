using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions;

/// <summary>
/// Отвечает за воспроизведение диалога
/// </summary>
public class ShowDialogueDubl : MonoBehaviour
{
    public TextAsset FileName;
    public string NPCName;
    public Transform DialogueWindow; //Canvas
    public const int MaxReplicLength = 350; //Максимальное число символов реплики на экране
    public float TimeBetweenLetters = 0.01f;
    
    private Dialogue _dialogue;
    private Transform _panel;
    private TextMeshProUGUI _replicText;
    private Button _continue;
    private TextMeshProUGUI _npcName;

    private bool IsTrigger; //Для контроля включения диалога
    private Queue<string> _replicParts; //Части текущей реплики. На экране отображается всегда часть сверху очереди
    private int _replicInd = 0; //Текущий индекс в ктеущей части реплики
    private Button _prefab;

    private void Awake()
    {   
        IsTrigger = false;
        _dialogue = Dialogue.Load(FileName);
        _npcName = DialogueWindow.GetChild(1).GetComponent<TextMeshProUGUI>();
        _panel = DialogueWindow.GetChild(2).GetComponent<Transform>();
        _continue = DialogueWindow.GetChild(3).GetComponent<Button>();
        _npcName.text = NPCName;
        _replicText = _panel.GetChild(0).GetComponent<TextMeshProUGUI>();
        _continue.onClick.RemoveAllListeners();
        _continue.onClick.AddListener(Continue);
        _prefab = Resources.Load<Button>("Prefabs/Interface/DialogueButton");

        _replicParts = new Queue<string>();
    }
    /// <summary>
    /// Переход к выбору ответов/пропуск анимации печати/окончание диалога по нажатию кнопки _continue
    /// </summary>
    private void Continue()
    {
        StopAllCoroutines();
        if (_replicInd != _replicParts.Peek().Length - 1)
            PrintReplicEntirely(_replicInd);
        else
        {
            _replicParts.Dequeue();
            _replicInd = 0;
            if (_replicParts.Count == 0)
                GoToAnswers();
            else
            {
                _replicText.text = "";
                StartCoroutine(PrintReplicGradually());
            }
        }
    }
    private void Update()
    {
        if (IsTrigger && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
            IsTrigger = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            IsTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EndDialogue();
    }
    /// <summary>
    /// Запускает диалог
    /// </summary>
    private void StartDialogue()
    {
        DialogueWindow.gameObject.SetActive(true);
        
        if (_dialogue.GetCurentNode().npcText != null)
            GoToReplic();
        else GoToAnswers();
    }
    /// <summary>
    /// Печать части реплики с from по </fast> целиком. Или если </fast> нет, то до конца реплики
    /// </summary>
    /// <param name="from"></param>
    /// <param name="last"></param>
    private void PrintReplicEntirely(int from) 
    {
        Debug.Log("PrintReplicEntirely запущен!");
        string text = _replicParts.Peek();
        Match m = Regex.Match(text[_replicInd..], @"<fast>(\w+?)</fast>");
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
    }
    /// <summary>
    /// Печатет реплику побуквенно с помощью DOTween
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private IEnumerator PrintReplicGradually()
    {
        Debug.Log("PrintReplicGradually запущен!");
        string text = _replicParts.Peek();

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
                    case "fast": PrintReplicEntirely(_replicInd); break;
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
            yield return new WaitForSeconds(TimeBetweenLetters);
        }
        _replicInd--;
        Debug.Log($"_replicInd = {_replicInd}");
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
    /// <summary>
    /// Выводит варианты ответов к реплике
    /// </summary>
    private void GoToAnswers()
    {
        Debug.Log("GoToAnswers запущен!");
        _replicParts.Clear();
        _continue.gameObject.SetActive(false);
        _npcName.gameObject.SetActive(false);
        _replicText.gameObject.SetActive(false);

        foreach (Dialogue.Answer answ in _dialogue.GetCurentNode().answers)
        {
            Dialogue.Answer locAnsw = answ;
            Debug.Log(_panel.childCount);
            Button btn = Instantiate(_prefab, _panel);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = locAnsw.text;
            Debug.Log(locAnsw.toNode);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                Debug.Log("***");
                if (locAnsw.exit == "True")
                    EndDialogue();
                else 
                {
                    Debug.Log("!!!");
                    _dialogue.ToNodeWithInd(locAnsw.toNode);
                    Debug.Log(_dialogue.GetCurentNode());
                    GoToReplic(); 
                }
            });
        }
    }
    /// <summary>
    /// Подготавливает панель к печати реплики после ответов или в самом начале
    /// </summary>
    private void GoToReplic()
    {
        Debug.Log("GoToReplic запущен!");
        _continue.gameObject.SetActive(true);
        _npcName.gameObject.SetActive(true);

        var buttons = _panel.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
            DestroyImmediate(btn.gameObject);
        }
        _replicText.text = "";
        _replicText.gameObject.SetActive(true);

        //Заполнение очереди частями реплики
        string currentReplic = _dialogue.GetCurentNode().npcText;

        if (string.IsNullOrEmpty(currentReplic))
        {
            Debug.LogWarning("Текущая реплика пуста!");
            return;
        }

        int start = 0, end = 0;
        char[] delimiters = new char[] { '!', '.', '?' };

        // Формирование частей реплики
        while (start < currentReplic.Length)
        {
            // Ищем конец предложения или обрезаем, если превышает MaxReplicLength
            while (end - start + 1 < MaxReplicLength)
            {
                end = currentReplic.IndexOfAny(delimiters, end + 1);
                if (end == -1)
                {
                    end = currentReplic.Length - 1;
                    break;
                }
            }

            // Добавляем часть строки в очередь
            string part = currentReplic.Substring(start, end - start + 1);
            if (!string.IsNullOrWhiteSpace(part))
            {
                _replicParts.Enqueue(part);
                Debug.Log($"Добавлена часть реплики: {part}");
            }

            start = end + 1;
        }

        if (_replicParts.Count == 0)
        {
            Debug.LogError("Не удалось создать части реплики. Диалог завершён.");
            EndDialogue();
            return;
        }

        //Запуск побуквенной печати
        StartCoroutine(PrintReplicGradually());
    }
    private void EndDialogue()
    {
        if (DialogueWindow.gameObject.activeSelf)
            DialogueWindow.gameObject.SetActive(false);
    }
}
