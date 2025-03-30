using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за воспроизведение диалога
/// </summary>
public class ShowDialogueDubl : MonoBehaviour
{
    [SerializeField] public TextAsset FileName;
    [SerializeField] public Transform DialogueWindow; //Canvas
    [SerializeField] public const int MaxReplicLength = 350; //Максимальное число символов реплики на экране
    [SerializeField] public float TimeBetweenLetters = 0.01f;
    public bool WithEnd = true;

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
        if (IsTrigger && Input.GetKeyDown(KeyCode.T)) //Поменялась кнопка!!! 
        {
            StartDialogue();
            IsTrigger = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Continue();
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
    public void StartDialogue()
    {
        DialogueWindow.gameObject.SetActive(true);
        
        Debug.Log(_panelForText);
        Debug.Log(TimeBetweenLetters);
        Debug.Log(_audio);
        printer.Init(_panelForText, TimeBetweenLetters, _audio);
        Debug.Log(printer);
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
        printer.PrintReplicGradually(_replicInd, _replicParts.Peek());
    }
    public void EndDialogue()
    {
        if (DialogueWindow.gameObject.activeSelf && WithEnd)
            DialogueWindow.gameObject.SetActive(false);

    }
}
