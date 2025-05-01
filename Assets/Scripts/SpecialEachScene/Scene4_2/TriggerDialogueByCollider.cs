using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueByCollider : MonoBehaviour
{
    [SerializeField] private TextAsset FileName;
    private ShowDialogueDubl _director;
    private bool isDialogueFinished = false;
    private SwitchScene _switch;
    private Dialogue _dialogue;
    private GameObject DialogueWindow;

    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        _switch = GetComponent<SwitchScene>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _director.StartDialogue();
        }
    }

    private void Update()
    {
        if (_director.FileName == FileName)
        {
            DialogueWindow = _director.DialogueWindow.gameObject;
            _dialogue = _director.GetDialogue();
        }
        if (!isDialogueFinished && !DialogueWindow.activeSelf && _director.GetDialogue().GetCurentNode().exit == "True")
        {
            isDialogueFinished = true;
            _switch.Switch();
        }
    }
}
