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
    [SerializeField] GameObject Player;


    

    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        InvokeRepeating("FixedUpdateEvery1Sec", 0f, 1f);
    }

    //метод запуска диалога для флажка TimeLine
    public void StartDialogue()
    {
        Player.GetComponent<PlayerMotion>().enabled = false;
        _director.StartDialogue();
    }

    void FixedUpdateEvery1Sec()
    {
        if (DialogueWnd.currentState == DialogueWndState.WindowState.EndPrint) 
        {
            StartCutscene2();
            Player.GetComponent<PlayerMotion>().enabled = true;
            CancelInvoke("FixedUpdateEvery1Sec");
        }
    }

    void StartCutscene2()
    {
        CutScene2.Play();  
        CutsceneCamera.SetActive(false); 
    }

}
