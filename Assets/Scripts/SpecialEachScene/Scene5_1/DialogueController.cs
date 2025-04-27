using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Навешивается на говорящего 
public class DialogueController : MonoBehaviour
{
    private ShowDialogueDubl director;
    private Dialogue dialogue;
    [SerializeField] DialogueWndState DialogueWindow;
    private bool trigger = true;
    private GameObject player;
    private void Start()
    {
        director = GetComponent<ShowDialogueDubl>();
        dialogue = director.GetDialogue();
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMotion>().enabled = false; //Возможность двигаться отключена на время диалога
        director.StartDialogue();
        
    }
    private void FixedUpdate()
    {
        if (!trigger && dialogue.GetCurentNodeIndex() == 2 && DialogueWindow.currentState == DialogueWndState.WindowState.EndPrint)
        {
            //Включить анимацию смерти ГГ
            trigger = true;
        }
        else if (!trigger && dialogue.GetCurentNodeIndex() == 3 && DialogueWindow.currentState == DialogueWndState.WindowState.EndPrint)
        {
            //Включить анимацию смерти ГГ
            trigger = true;
        }
        else if (trigger && dialogue.GetCurentNodeIndex() == 2 && DialogueWindow.transform.GetChild(4).gameObject.activeSelf)
        {
            //Включить анимации смертей солдат
            trigger = false;
        }
        else if (dialogue.GetCurentNodeIndex() == 4 && DialogueWindow.currentState == DialogueWndState.WindowState.EndPrint)
        {
            player.GetComponent<PlayerMotion>().enabled = true;
        }
    }
    //Ждёт конца анимации и включает титры
    private IEnumerator AnimationEnd()
    {
        yield return new WaitForFixedUpdate();
    }
}
