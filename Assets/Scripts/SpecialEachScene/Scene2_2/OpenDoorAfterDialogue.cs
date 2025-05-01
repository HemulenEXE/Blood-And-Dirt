using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//Запускает диалог и открывает дверь после его окончания. Сцена 2_1. Навешивается на говорящего
public class OpenDoorAfterDialogue : MonoBehaviour
{
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool isTriger = false;
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        _director.StartDialogue();
        DialogueWindow = _director.DialogueWindow.gameObject;
        _dialogue = _director.GetDialogue();
    }
    void Update()
    {
        if (!isTriger && !DialogueWindow.activeInHierarchy && (_dialogue.GetCurentNode().answers[0].exit == "True"))
        {
            isTriger = true;
            GameObject.Find("door").GetComponent<CircleCollider2D>().enabled = true;
        }
    }

}
