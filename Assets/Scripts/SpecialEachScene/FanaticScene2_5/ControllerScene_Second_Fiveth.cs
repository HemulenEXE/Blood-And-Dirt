using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScene_Second_Fiveth : MonoBehaviour
{
    [SerializeField] TextAsset FileName;
    private ShowDialogueDubl ShowDialogueDubl;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private Animator animator;
    private bool trigger = false;
    private bool triggerSecond = false;
    private SwitchScene switchScene;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ShowDialogueDubl = GetComponent<ShowDialogueDubl>();
        switchScene = GetComponent<SwitchScene>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == "Player")
        //{
        //    ShowDialogueDubl.StartDialogue();
        //}
    }

    private void Update()
    {
        if (ShowDialogueDubl.FileName == FileName)
        {
            DialogueWindow = ShowDialogueDubl.DialogueWindow.gameObject;
            _dialogue = ShowDialogueDubl.GetDialogue();
        }
        if (!trigger
            && DialogueWindow.activeSelf
            && _dialogue.GetCurentNodeIndex() == 3)
        {
            Debug.Log("Test!");
            trigger = true;
            animator.SetTrigger("Injection");
        }
        if (!triggerSecond
            && !DialogueWindow.activeSelf
            && _dialogue.GetCurentNode().exit == "True")
        {
            Debug.Log("END DIALOGUE");
            triggerSecond = true;
            switchScene.Switch();
        }
    }

}
