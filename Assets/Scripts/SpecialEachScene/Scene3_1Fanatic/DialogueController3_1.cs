using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class DialogueController3_1 : MonoBehaviour
{
    [SerializeField] TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool flag = false;

    [SerializeField] GameObject CutScene2, MainCamera, CutsceneCamera;

    

    private void Start()
    {
         flag = true;
        _director = GetComponent<ShowDialogueDubl>();
        InvokeRepeating("FixedUpdateEvery1Sec", 0f, 1f);
    }


    public void StartDialogue()
    {
        _director.StartDialogue();
    }

    void FixedUpdateEvery1Sec()
    {
        if (flag && !DialogueWindow.activeSelf && _dialogue.GetCurentNode().exit == "True") 
        {
            StartCutscene2();
            CancelInvoke("FixedUpdateEvery1Sec");
        }
    }

    void StartCutscene2()
    {
        CutScene2.SetActive(true);  
        MainCamera.SetActive(true);
        CutsceneCamera.SetActive(false); 
    }

}
