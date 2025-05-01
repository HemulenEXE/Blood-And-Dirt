using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2_3StartDialogue : MonoBehaviour
{
     [SerializeField]  private ShowDialogueDubl _director;


    //метод запуска диалога для флажка TimeLine
    public void StartDialogue()
    {
        _director.StartDialogue();
    }
}
