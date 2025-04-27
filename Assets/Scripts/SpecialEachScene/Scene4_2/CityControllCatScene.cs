using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CityControllCatScene : MonoBehaviour
{
    private ShowDialogueDubl dialogue;
    [SerializeField] bool check;
    [SerializeField] private PlayableDirector catScene;
    [SerializeField] private PlayableDirector catScene_2;
    private bool checkFirst = false;
    private bool checkSecond = false;

    private void Start()
    {
        dialogue = GetComponent<ShowDialogueDubl>();
        catScene_2.stopped += StopCatScene2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !checkSecond && !checkFirst)
        {
            dialogue.StartDialogue();
            GetComponent<Talker>().enabled = false;
            GetComponent<Printer>().enabled = false;
            dialogue.SetAct();
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerMotion>().enabled = false;
            player.GetComponentInChildren<Animator>().SetBool("IsMoving", false);
       }
    }

    private void Update()
    {
        if (!checkFirst && dialogue.GetDialogue().GetCurentNodeIndex() == 1) 
        {
            catScene.Play();
            checkFirst = true;
        }
        if(!checkSecond && !dialogue.DialogueWindow.gameObject.activeSelf && (dialogue.GetDialogue().GetCurentNode().exit == "True"))
        {
            GetComponent<ShowDialogueDubl>().enabled = false;
            dialogue.SetAct();
            checkSecond = true;
            catScene_2.Play();
        }
    }

    private void StopCatScene2(PlayableDirector obj)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotion>().enabled = true;
        
    }
}
