using InteractiveObjects;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Скрипт навешивается на NPS. Для корректной работы на сцене должен быть объект EventSystem. На NPC навешан colloder и rigitbody, на игрока только collider 
/// </summary>
public class ShowDialogue : MonoBehaviour
{
    public TextAsset FileName;
    public string NPCName;
    public GameObject _dialogueWindow; //Canvas
    public Font font; //Шрифт текста
    public int FontSize; //Размер шрифта

    private Dialogue _dialogue;
    private Transform _panel;
    private Button _continue;
    private TextMeshProUGUI _npcName;

    bool IsTrigger; //Для контроля включения диалога
    bool IsPrinting;
    private int _nodeInd;
    private int _startInd; //Указывает на начало текущей части реплики (для корректного переключения между ними)
    private const int _maxReplicLength = 240; //Максимальное число символов реплики на экране
    private void Start()
    {
        //Меняем кнопку взаимодействия с NPS
        this.GetComponent<ClickedObject>().Description = "T"; 

        _dialogue = Dialogue.Load(FileName);

        _nodeInd = 0;
        _startInd = 0;
        IsPrinting = false;
        IsTrigger = false;
    }
    /// <summary>
    /// Переход к выбору ответов/пропуск анимации печати/окончание диалога по нажатию кнопки _continue
    /// </summary>
    private void Continue()
    {
        Text currentReplic = _panel.GetChild(0).GetComponent<Text>();

        //Если реплика печатаетя - пропускаем анимацию
        if (IsPrinting)
        {
            StopAllCoroutines();

            //Переставляет _startInd в правильную позицию (чтобы потом можно было дописать реплику, если была написана не вся)
            int i = _dialogue.Nodes[_nodeInd].npcText.Length - 1;
            char[] chars = new char[] { '!', '.', '?' };

            while (i - _startInd > _maxReplicLength)
            {
                i = _dialogue.Nodes[_nodeInd].npcText[..i].LastIndexOfAny(chars);
            }

            currentReplic.text = _dialogue.Nodes[_nodeInd].npcText[(_startInd+ 1)..(i + 1)];
            _startInd = i + 1;
            IsPrinting = false;
        }
        else if (_startInd < _dialogue.Nodes[_nodeInd].npcText.Length) //Если реплика напечатана не вся, пускаем на печать вторую часть
        {
            currentReplic.text = "";

            StartCoroutine(PrintReplic(_dialogue.Nodes[_nodeInd].npcText[(_startInd + 1)..]));
        }
        else //Если реплика полностью напечатана, то: в случае, если она конечная, закрываем диалог, иначе - пускааем на печать ответы к ней
        {
            _startInd = 0;
            
            currentReplic.transform.parent = null;
            Destroy(currentReplic.gameObject);

            if (_dialogue.Nodes[_nodeInd].exit == "True")
                EndDialogue();
            else
                PrintAnswers();
        }
    }
    private void Update()
    {
        if (IsTrigger && Input.GetKeyDown(KeyCode.T))
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
        _dialogueWindow.SetActive(true);
        _npcName = _dialogueWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _panel = _dialogueWindow.transform.GetChild(2).GetComponent<Transform>();
        _continue = _dialogueWindow.transform.GetChild(3).GetComponent<Button>();
        _npcName.text = NPCName;

        _continue.onClick.RemoveAllListeners();
        _continue.onClick.AddListener(Continue);

        if (_dialogue.Nodes[_nodeInd].npcText != null)
            PrintNode();
        else
            PrintAnswers();
    }
    /// <summary>
    /// Печатет конкретную реплику побуквенно 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private IEnumerator PrintReplic(string text)
    {
        IsPrinting = true;
        float speed = 0.01f;
        int i = 0;

        Text replic = _panel.GetChild(0).GetComponent<Text>();

        char[] chars = new char[] { '!', '.', '?' };
        while (i < text.Length && !(chars.Contains(text[i]) && (text.IndexOfAny(chars, i+1) > _maxReplicLength)))
        {
            Debug.Log($"{i} - nextchar:{text.IndexOfAny(chars, i + 1)} - char:{chars.Contains(text[i])}, {text[i]}");
            replic.text += text[i];
            i++;
            yield return new WaitForSeconds(speed);
        }
        _startInd += i + 1;
        Debug.Log(_startInd);
        IsPrinting = false;
    }
    /// <summary>
    /// Создаёт варианты ответов к реплике 
    /// </summary>
    private void PrintAnswers()
    {
        _continue.gameObject.SetActive(false);
        _npcName.gameObject.SetActive(false);

        Dialogue.Node currentNode = _dialogue.Nodes[_nodeInd];
        Dialogue.Answer[] currentAnswers = currentNode.answers;

        for (int i = 0; i < currentAnswers.Length; i++)
        {
            CreateButton(i.ToString(), currentAnswers[i].text, currentAnswers[i].toNode, currentAnswers[i].exit);
        }
    }
    /// <summary>
    /// Очищает панель от кнопок и запускает печать реплики 
    /// </summary>
    private void PrintNode()
    {
        Debug.Log("Реплика печатается!");
        _continue.gameObject.SetActive(true);
        _npcName.gameObject.SetActive(true);

        int count = _panel.childCount;
        //Если на панеле есть кнопки удаляем их  
        while (count > 0)
        {
            GameObject btn = _panel.GetChild(0).gameObject;
            btn.transform.parent = null;
            Destroy(btn);
            count--;
        }
        CreateText(_panel, "NpcReplic", "");
        StartCoroutine(PrintReplic(_dialogue.Nodes[_nodeInd].npcText));
    }
    private void EndDialogue()
    {
        if (_dialogueWindow.activeSelf)
            _dialogueWindow.SetActive(false);

        _nodeInd = 0;
        _startInd = 0;
    }
    /// <summary>
    /// Создание кнопки внутри панели 
    /// </summary>
    /// <param name="elemName"></param>
    /// <param name="text"></param>
    /// <param name="toNode"></param>
    /// <param name="exit"></param>

    private void CreateButton(string elemName, string text, int toNode, string exit)
    {
        //Создание кнопки
        GameObject btn = new GameObject("btn" + elemName, typeof(Image), typeof(Button));
        Color color = btn.GetComponent<Image>().color;
        color.a = 0;
        btn.GetComponent<Image>().color = color;
        btn.transform.SetParent(_panel.GetComponent<VerticalLayoutGroup>().transform);
        //Настройка положения кнопки на панеле
        RectTransform btnRect = btn.GetComponent<RectTransform>();
        btnRect.SetParent(_panel);
        btnRect.localScale = Vector2.one;

        //Создание текста в кнопке
        CreateText(btn.transform, elemName, text);

        //Добавление интерактивности
        AddInteractive(btn.GetComponent<Button>());

        //Привязка действия к кнопке
        btn.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Действие привязано!");
            if (exit == "True")  
                EndDialogue();
            else
            {
                _nodeInd = toNode;
                PrintNode();
            }
        });
    }
    /// <summary>
    /// Создание текста внутри поданного объекта 
    /// </summary>
    /// <param name="elemName"></param>
    /// <param name="text"></param>
    private void CreateText(Transform parent, string elemName, string text)
    {
        //Создание текста 
        GameObject txt = new GameObject("txt" + elemName, typeof(Text));
        txt.transform.SetParent(parent);
        txt.GetComponent<Text>().font = font;
        txt.GetComponent<Text>().fontSize = FontSize;
        txt.GetComponent<Text>().text = text;
        txt.GetComponent<Text>().color = Color.white;
        txt.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        
        //Настройка позиции текста
        RectTransform rt = txt.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.localScale = Vector2.one;
        rt.localPosition = Vector2.zero;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = Vector2.one;
    }
    /// <summary>
    /// Добавляет интерактивности кнопкам (меняет размер текста при наведении курсора)
    /// </summary>
    /// <param name="btn"></param>
    private void AddInteractive(Button btn)
    {
        Text text = btn.transform.GetChild(0).GetComponent<Text>();

        EventTrigger eventTrigger = btn.gameObject.AddComponent<EventTrigger>();

        // Создаем событие для наведения курсора на кнопку
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        // Добавляем обработчик события
        pointerEnterEntry.callback.AddListener((data) => { text.fontSize = text.fontSize + 4; });
        eventTrigger.triggers.Add(pointerEnterEntry);

        // Создаем событие для ухода курсора с кнопки
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        // Добавляем обработчик события
        pointerExitEntry.callback.AddListener((data) => { text.fontSize = text.fontSize - 4; });
        eventTrigger.triggers.Add(pointerExitEntry);
    }
}
