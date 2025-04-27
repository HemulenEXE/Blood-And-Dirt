using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Навешивается на говорящего 
public class DialogueController : MonoBehaviour
{
    private ShowDialogueDubl director;
    private Dialogue dialogue;

    private void Start()
    {
        director = GetComponent<ShowDialogueDubl>();
        dialogue = director.GetDialogue();

        director.StartDialogue();
    }
    private void FixedUpdate()
    {
        
    }
}
