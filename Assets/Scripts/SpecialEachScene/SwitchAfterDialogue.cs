using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Скрипт перехода на новую сцену после окончания диалога (считается, что диалог закончен, если проиграна его ПОСЛЕДНЯЯ реплика, НЕ реплика со свойством EXIT)
//Навешивать на персонажа, после разговора с которым происхдит переход на другую сцену
public class SwitchAfterDialogue : SwitchScene
{
    [SerializeField] TextAsset FileName;
    //public bool AfterExit = false;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
    }
    void Update()
    {
        if (_director.FileName == FileName)
        {
            DialogueWindow = _director.DialogueWindow.gameObject;
            _dialogue = _director.GetDialogue();
        }

        //if (!AfterExit && !DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1))
        if (!DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1))
            Switch();
        //else if (!DialogueWindow.activeSelf && (_dialogue.GetCurentNode().exit == "True" || _dialogue.GetCurentNode().answers.Any(x => x.exit == "True")))
         //   Switch();
    }
}
