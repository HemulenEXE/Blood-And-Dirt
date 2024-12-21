using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Printer printer;

    private void Awake()
    {   
        _dialogue = Dialogue.Load(FileName);
        
        _npcName = DialogueWindow.GetChild(1).GetComponent<TextMeshProUGUI>();
        _panel = DialogueWindow.GetChild(2).GetComponent<Transform>();
        _continue = DialogueWindow.GetChild(3).GetComponent<Button>();
        _npcName.text = NPCName;
        _replicText = _panel.GetChild(0).GetComponent<TextMeshProUGUI>();
        _continue.onClick.RemoveAllListeners();
        _continue.onClick.AddListener(Continue);
        
        IsTrigger = false;
        _prefab = Resources.Load<Button>("Prefabs/Interface/DialogueButton");
        printer = GetComponent<Printer>();
        printer.Init(_replicText, TimeBetweenLetters);
        _replicParts = new Queue<string>();
    }
    /// <summary>
    /// Переход к выбору ответов/пропуск анимации печати/окончание диалога по нажатию кнопки _continue
    /// </summary>
    private void Continue()
    {
        _replicInd = printer._rInd;
        Debug.Log($"_replicInd = {_replicInd}, Length = {_replicParts.Peek().Length}");
        printer.StopAllCoroutines();
        if (_replicInd != _replicParts.Peek().Length - 1)
            printer.PrintReplicEntirely(ref _replicInd, _replicParts.Peek());
        else
        {
            _replicParts.Dequeue();
            _replicInd = 0;
            if (_replicParts.Count == 0)
                GoToAnswers();
            else
            {
                _replicText.text = "";
                printer.PrintReplicGradually(_replicInd, _replicParts.Peek());
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
                if (locAnsw.exit == "True")
                    EndDialogue();
                else 
                {
                    _dialogue.ToNodeWithInd(locAnsw.toNode);
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
        printer.PrintReplicGradually(_replicInd, _replicParts.Peek());
    }
    private void EndDialogue()
    {
        if (DialogueWindow.gameObject.activeSelf)
            DialogueWindow.gameObject.SetActive(false);
    }
}
