using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//Запускает диалог, а после него переход ня след. сцену. Средняя ветка сцена 2.1
public class EndScene : SwitchScene
{
    [SerializeField] TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool flag = false;
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log(this.GetComponent<ShowDialogueDubl>());
            flag = true;
            this.GetComponent<ShowDialogueDubl>().StartDialogue();
        }
    }
    private void Update()
    {
        if (_director.FileName == FileName)
        {
            DialogueWindow = _director.DialogueWindow.gameObject;
            _dialogue = _director.GetDialogue();
        }

        if (flag && !DialogueWindow.activeSelf && _dialogue.GetCurentNode().exit == "True")
            Switch();
    }
}
