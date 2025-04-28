using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;


public class DialogueController3_1 : MonoBehaviour
{
    private ShowDialogueDubl _director;
    [SerializeField] DialogueWndState DialogueWnd;
    [SerializeField] PlayableDirector CutScene2;
    [SerializeField] GameObject CutsceneCamera;

    

    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        InvokeRepeating("FixedUpdateEvery1Sec", 0f, 1f);
    }

    //метод запуска диалога для флажка TimeLine
    public void StartDialogue()
    {
        _director.StartDialogue();
    }

    void FixedUpdateEvery1Sec()
    {
        if (DialogueWnd.currentState == DialogueWndState.WindowState.EndPrint) 
        {
            StartCutscene2();
            CancelInvoke("FixedUpdateEvery1Sec");
        }
    }

    void StartCutscene2()
    {
        CutScene2.Play();  
        CutsceneCamera.SetActive(false); 
    }

}
