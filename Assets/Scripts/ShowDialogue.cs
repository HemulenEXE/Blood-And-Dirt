using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Скрипт навешивается на NPS
/// </summary>
public class ShowDialogue : MonoBehaviour
{
    public TextAsset FileName;
    public string NPCName;
    public GameObject _dialogueWindow; //Canvas
    public Font font; //Шрифт текста

    private Dialogue _dialogue;
    private Transform _panel;
    private Button _continue;
    private TextMeshProUGUI _npcName;

    bool IsTrigger = false; //Для контроля включения диалога
    bool IsPrinting;
    private int _nodeInd;
    private int _startInd; //Указывает на начало текущей части реплики (для корректного переключения между ними)
    private const int _maxReplicLength = 350; //Максимальное число символов реплики на экране
    private void Start()
    {
        _dialogue = Dialogue.Load(FileName);
    }
    /// <summary>
    /// Переход к выбору ответов/пропуск анимации печати/окончание диалога по нажатию кнопки _continue
    /// </summary>
    private void Continue()
    {
        Text currentReplic = _panel.GetChild(0).GetComponent<Text>();

        Debug.Log("0: " + _startInd + "-" + _dialogue.Nodes[_nodeInd].npcText.Length);
        //Если реплика печатаетя - пропускаем анимацию
        if (IsPrinting)
        {
            StopAllCoroutines();

            //Переставляет _startInd в правильную позицию (чтобы потом можно было дописать реплику, если была написана не вся)
            int i = _dialogue.Nodes[_nodeInd].npcText.Length - 1;
            char[] chars = new char[] { '!', '.', '?' };
            Debug.Log(_dialogue.Nodes[_nodeInd].npcText[..i].LastIndexOfAny(chars));

            while (i - _startInd > _maxReplicLength)
            {
                i = _dialogue.Nodes[_nodeInd].npcText[..i].LastIndexOfAny(chars);
            }

            currentReplic.text = _dialogue.Nodes[_nodeInd].npcText[_startInd..(i + 1)];
            _startInd = i + 1;
            IsPrinting = false;
            Debug.Log("1: " + _startInd + "-" + _dialogue.Nodes[_nodeInd].npcText.Length);
        }
        else if (_startInd < _dialogue.Nodes[_nodeInd].npcText.Length) //Если реплика напечатана не вся, пускаем на печать вторую часть
        {
            currentReplic.transform.parent = null;
            Destroy(currentReplic.gameObject);

            Debug.Log("2: " + _startInd);
            StartCoroutine(PrintReplic(_dialogue.Nodes[_nodeInd].npcText[(_startInd)..]));
        }
        else //Если реплика полностью напечатана, то: в случае, если она конечная, закрываем диалог, иначе - пускааем на печать ответы к ней
        {
            _startInd = 0;

            if (_dialogue.Nodes[_nodeInd].exit == "True")
                EndDialogue();
            else
            {
                currentReplic.transform.parent = null;
                Destroy(currentReplic.gameObject);

                PrintAnswers();
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
        _nodeInd = 0;
        _startInd = 0;
        IsPrinting = false;

        _continue.onClick.RemoveAllListeners();
        _continue.onClick.AddListener(Continue);

        if (_dialogue.Nodes[_nodeInd].npcText != null)
            StartCoroutine(PrintReplic(_dialogue.Nodes[_nodeInd].npcText));
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
        Debug.Log("Метод PrintReplic запущен");
        IsPrinting = true;
        float speed = 0.01f;
        int i = 0;

        CreateText(_panel,"NpcReplic", "");
        Text replic = _panel.GetChild(0).GetComponent<Text>();

        char[] chars = new char[] { '!', '.', '?' };
        while (i < text.Length && !(text[i].Equals(chars) && (text.IndexOfAny(chars, i+1) > _maxReplicLength)))
        {
            replic.text += text[i];
            i++;
            yield return new WaitForSeconds(speed);
        }
        _startInd += i;
        IsPrinting = false;
        Debug.Log("Метод PrintReplic завершён");
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
        Debug.Log("метод PrintNode запущен!");
        _continue.gameObject.SetActive(true);
        _npcName.gameObject.SetActive(true);

        int count = _panel.childCount;
        //Если на панеле есть кнопки удаляем их  
        while (count > 0)
        {
            GameObject btn = _panel.GetChild(0).gameObject;
            btn.transform.parent = null;
            Destroy(btn);
            Debug.Log("Кнопка " + count + " удалена");
            count--;
        }
        Debug.Log("метод PrintNode закончен!");
        StartCoroutine(PrintReplic(_dialogue.Nodes[_nodeInd].npcText));
    }
    private void EndDialogue()
    {
        _dialogueWindow.SetActive(false);
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
        Debug.Log("Создана кнопка: " + text + "\n" + toNode + " " + exit);

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
            Debug.Log(toNode);
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
        Debug.Log("Метод CreateText запущен");
        //Создание текста 
        GameObject txt = new GameObject("txt" + elemName, typeof(Text));
        txt.transform.SetParent(parent);
        txt.GetComponent<Text>().font = font;
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
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter(); });
        eventTrigger.triggers.Add(pointerEnterEntry);

        // Создаем событие для ухода курсора с кнопки
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        // Добавляем обработчик события
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit(); });
        eventTrigger.triggers.Add(pointerExitEntry);

        void OnPointerEnter()
        {
            text.fontSize = text.fontSize + 4;
        }

        void OnPointerExit()
        {
            text.fontSize = text.fontSize - 4;
        }
    }
}
