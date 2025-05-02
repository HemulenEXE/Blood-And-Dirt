using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// Отвечает за воспроизведение диалога
/// </summary>
public class ShowDialogueDubl : MonoBehaviour
{
    [SerializeField] public TextAsset FileName;
    [SerializeField] public Transform DialogueWindow; //Canvas
    [SerializeField] public const int MaxReplicLength = 350; //Максимальное число символов реплики на экране
    [SerializeField] public float TimeBetweenLetters = 0.01f;
    public bool WithEnd = true; //Должен ли диалог заканчиваться, при выходе игрока из тигерной зоны 
    public bool WithAction = false; //Должны kи быть доступны другие действия во время диалога (стрельба, расходники и т.д.)

    private Dialogue _dialogue;
    private Transform _panelForText;
    private Transform _panelForButtons;
    private Button _continue;
    private TextMeshProUGUI _npcName;
    private AudioSource _audio;

    private bool IsTrigger; //Для контроля включения диалога
    private Queue<string> _replicParts; //Части текущей реплики. На экране отображается всегда часть сверху очереди
    private int _replicInd = 0; //Текущий индекс в ктеущей части реплики
    private Button _prefab;
    private Printer printer;
    private bool isPrint = false; //идёт ли печать в данный момент

    public Dialogue GetDialogue() { return _dialogue; }
    private void Awake()
    {
        _dialogue = Dialogue.Load(FileName);

        _npcName = DialogueWindow.GetChild(1).GetComponent<TextMeshProUGUI>();
        _panelForText = DialogueWindow.GetChild(2).GetComponent<Transform>();
        _panelForButtons = DialogueWindow.GetChild(3).GetComponent<Transform>();
        _continue = DialogueWindow.GetChild(4).GetComponent<Button>();
        _audio = DialogueWindow.GetComponent<AudioSource>();
        _continue.onClick.RemoveAllListeners();
        _continue.onClick.AddListener(Continue);

        IsTrigger = false;
        _prefab = Resources.Load<Button>("Prefabs/Interfaces/DialogueButton");
        printer = this.GetComponent<Printer>();
        _replicParts = new Queue<string>();
    }
    /// <summary>
    /// Переход к выбору ответов/пропуск анимации печати/окончание диалога по нажатию кнопки _continue
    /// </summary>
    public void Continue()
    {
        _replicInd = printer._rInd;
        printer.StopAllCoroutines();
        if (_replicInd < _replicParts.Peek().Length - 1)
            printer.PrintReplicEntirely(_replicInd, _replicParts.Peek());
        else
        {
            DOTween.KillAll();
            _replicParts.Dequeue();
            _replicInd = 0;
            if (_replicParts.Count == 0)
            {
                if (_dialogue.GetCurentNode().exit == "True") EndDialogue();
                else if (_dialogue.GetCurentNode().answers == null)
                {
                    _dialogue.ToNextNode();

                    if (_dialogue.GetCurentNode().npcText != null)
                        GoToReplic();
                    else GoToAnswers();
                }
                else GoToAnswers();
            }
            else
            {
                foreach (Transform child in _panelForText.GetComponentInChildren<Transform>())
                    Destroy(child.gameObject);
                printer.PrintReplicGradually(_replicInd, _replicParts.Peek());
            }
        }
    }
    private void Update()
    {
        if (IsTrigger && Input.GetKeyDown(KeyCode.T) && !isPrint) //Поменялась кнопка!!! 
        {
            Debug.Log(isPrint);
            StartDialogue();
            isPrint = true;
            IsTrigger = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            IsTrigger = true;
    }
    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player" && !DialogueWindow.gameObject.active && Input.GetKeyDown(KeyCode.T) && !isPrint)
        { 
            StartDialogue();
            isPrint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsTrigger = false;
        EndDialogue(false); 
    }
    /// <summary>
    /// Запускает диалог
    /// </summary>
    public void StartDialogue()
    {
        if (!WithAction)
            SetAct();
        DialogueWindow.gameObject.SetActive(true);
        
        printer.Init(_panelForText, TimeBetweenLetters, _audio);
        _dialogue.ToNodeWithInd(0);
        _replicParts.Clear();
        _replicInd = 0;

        if (_dialogue.GetCurentNode().npcText != null)
            GoToReplic();
        else GoToAnswers();
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
        _panelForText.gameObject.SetActive(false);

        //Очистка старых кнопок
        var buttons = _panelForButtons.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
            DestroyImmediate(btn.gameObject);
        }
        _panelForButtons.gameObject.SetActive(true);
        //Добавление новых кнопок
        foreach (Dialogue.Answer answ in _dialogue.GetCurentNode().answers)
        {
            Dialogue.Answer locAnsw = answ;
            Button btn = Instantiate(_prefab, _panelForButtons);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = locAnsw.text;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                _audio.Play();
                if (locAnsw.toScene != 0)
                {
                    PlayerPrefs.SetInt("nextScene", locAnsw.toScene); //Сохраняет ключевые ответы 
                    PlayerPrefs.Save();
                }
                if (locAnsw.exit == "True")
                    EndDialogue();
                else
                {
                    _dialogue.ToNodeWithInd(locAnsw.toNode);
                    if (_dialogue.GetCurentNode().npcText != null)
                        GoToReplic();
                    else GoToAnswers();
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
        _panelForButtons.gameObject.SetActive(false);

        _npcName.text = _dialogue.GetCurentNode().npcName;

        foreach (Transform child in _panelForText.GetComponentInChildren<Transform>())
            Destroy(child.gameObject);
        _panelForText.gameObject.SetActive(true);

        //Заполнение очереди частями реплики
        string currentReplic = _dialogue.GetCurentNode().npcText;

        int start = 0, end = 0;
        char[] delimiters = new char[] { '!', '.', '?', '>'};

        string color = null;
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

            //Добавляем в часть недостающие теги, если они есть
            string part = currentReplic.Substring(start, end - start + 1);

            string pattern1 = @"<(\w+)(?:=\w+)?>";
            string pattern2 = @"<\/(\w+)>";

            MatchCollection startTags = Regex.Matches(part, pattern1);
            MatchCollection endTags = Regex.Matches(part, pattern2);
            
            if (startTags.Count > endTags.Count)
            {
                if (endTags.Count == 0)
                {
                    for (int i = startTags.Count - 1; i >=0 ; i--)
                        part = part + @"</" + startTags[i].Groups[1].Value + ">";

                }
                else
                {
                    //Отслеживает <b>text</b>..<i>text  - когда не закрыт тег после какого-то тега                  
                    int i = startTags.Count -1;
                    while (startTags[i].Index > endTags[endTags.Count - 1].Index)
                    {
                        if (startTags[i].Groups[1].Value == "color")
                            color = startTags[i].Groups[2].Value;
                        part = part + @"</" + startTags[i].Groups[1].Value + ">";
                        i--;
                    }
                    //Отслеживает когда не закрыт начальный <b> text ... <i>text</i> ...
                    int j = 0;
                    while (j < i && startTags[j].Groups[1].Value != endTags[0].Groups[1].Value)
                    {
                        if (startTags[j].Groups[1].Value == "color")
                            color = startTags[i].Groups[2].Value;
                        part = part + @"</" + startTags[j].Groups[1].Value + ">";
                        j++;
                    }

                }
            }
            else if (endTags.Count > startTags.Count)
            {
                if (startTags.Count == 0)
                {
                    for (int i = 0; i < endTags.Count; i++)
                        part = "<" + endTags[i].Groups[1].Value + ">" + part;
                }
                else
                {
                    //Отслеживает text</b> ... <i>text</i>
                    int i = 0;
                    while (endTags[i].Index < startTags[0].Index)
                    {
                        if (endTags[i].Groups[1].Value == "color" && color != null)
                            part = "<" + endTags[i].Groups[1].Value + "="+ color + ">" + part;
                        else part = "<" + endTags[i].Groups[1].Value + ">" + part;
                        i++;
                    }
                    //Отслеживает ... <i>text</i> ... text</b>
                    int j = endTags.Count - 1;
                    while (j >= i && endTags[j].Groups[1].Value != startTags[startTags.Count - 1].Groups[1].Value)
                    {
                        if (endTags[j].Groups[1].Value == "color" && color != null)
                            part = "<" + endTags[j].Groups[1].Value + "=" + color + ">" + part;
                        else part = "<" + endTags[j].Groups[1].Value + ">" + part;
                        j--;
                    }
                }
            }

            // Добавляем часть строки в очередь
            if (!string.IsNullOrWhiteSpace(part))
            {
                _replicParts.Enqueue(part);
                Debug.Log($"Добавлена часть реплики: {part}\nДлинной (c тегами): {part.Length}");
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
        printer.PrintReplicGradually(_replicInd, _replicParts.Peek());
    }
    //Булевый параметр отвечает за то, закончился ли диалог или игрок вышел из триггера
    public void EndDialogue(bool end = true)
    {
        if (DialogueWindow != null)
        {
            if ((DialogueWindow.gameObject.activeSelf && WithEnd) || (end && DialogueWindow.gameObject.activeSelf))
            {
                DialogueWindow.gameObject.SetActive(false);
                IsTrigger = false;
                isPrint = false;
                DialogueWindow.GetComponent<DialogueWndState>().currentState = DialogueWndState.WindowState.EndPrint;
                if (!WithAction) //Включаем обратно возможность действовать, если она отключена
                    SetAct();
            }
        }
    }
    //Включает/отключает возможность что-то делать во время диалога
    public void SetAct()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if(player.GetComponent<PlayerGrenade>() != null) player.GetComponent<PlayerGrenade>().enabled = !player.GetComponent<PlayerGrenade>().enabled;
        if (player.GetComponent<PlayerKnife>() != null) player.GetComponent<PlayerKnife>().enabled = !player.GetComponent<PlayerKnife>().enabled;
        if (player.GetComponent<PlayerShooting>() != null)  player.GetComponent<PlayerShooting>().enabled = !player.GetComponent<PlayerShooting>().enabled;
    }

}
