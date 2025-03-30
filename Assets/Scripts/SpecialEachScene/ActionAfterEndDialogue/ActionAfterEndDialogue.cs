using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class ActionAfterEndDialogue : MonoBehaviour
{
    [SerializeField] private TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool isTriger = false;

    [SerializeField] IAction action;
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        action = GetComponent<IAction>();
    }
    void Update()
    {
        if (_director.FileName == FileName)
        {
            DialogueWindow = _director.DialogueWindow.gameObject;
            _dialogue = _director.GetDialogue();
        }

        if (!isTriger && !DialogueWindow.activeSelf && (_dialogue.GetCurentNode().exit == "True"))
        {
            Debug.Log("Окончание диалога зафиксировано!");
            isTriger = true;
            action.DoIt();
        }
    }
}
