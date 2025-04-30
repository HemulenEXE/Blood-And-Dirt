using CameraLogic.CameraEffects;
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
    private bool trigger1 = true;
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
        if (trigger && (dialogue.GetCurentNodeIndex() == 2 || dialogue.GetCurentNodeIndex() == 3) && !DialogueWindow.transform.GetChild(4).gameObject.activeSelf)
        {
            Debug.Log("Start set anim");
            //Включить анимации смертей солдат
            Animator soldier = GameObject.Find("Soldier").GetComponent<Animator>();
            Animator soldier1 = GameObject.Find("Soldier (1)").GetComponent<Animator>();
            soldier.SetBool("suiside", true);
            soldier1.SetBool("suiside", true);
            Vector3 pos = soldier.transform.position;
            Vector3 pos1 = soldier1.transform.position;
            soldier.gameObject.transform.position = new Vector3(pos.x + 0.5f, pos.y + 2.3f, pos.z);
            soldier1.gameObject.transform.position = new Vector3(pos1.x + 0.5f, pos.y + 2.3f, pos.z);
            trigger = false;
        }
        else if (trigger1 && (dialogue.GetCurentNodeIndex() == 2) && (DialogueWindow.currentState == DialogueWndState.WindowState.EndPrint))
        {
            //Включить анимацию смерти ГГ
            player.gameObject.SetActive(false);
            GameObject.Find("суицид ГГ").GetComponent<Animator>().enabled = true;
            trigger1 = false;
            StartCoroutine(AnimationEnd());
        }
        else if (trigger1 && (dialogue.GetCurentNodeIndex() == 3) && (DialogueWindow.currentState == DialogueWndState.WindowState.EndPrint))
        {
            //Включить анимацию смерти ГГ
            player.gameObject.SetActive(false);
            GameObject.Find("суицид ГГ").GetComponent<Animator>().enabled = true;
            trigger1 = false;
            StartCoroutine(AnimationEnd());
        }
        else if (dialogue.GetCurentNodeIndex() == 4 && DialogueWindow.currentState == DialogueWndState.WindowState.EndPrint)
        {
            player.GetComponent<PlayerMotion>().enabled = true;
        }
    }
    //Ждёт конца анимации и включает титры
    private IEnumerator AnimationEnd()
    {     
        var anim = GameObject.Find("суицид ГГ").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (anim.IsName("суицид ГГ_Clip"))
            yield return new WaitForSeconds(anim.length + 0.5f);
        
        GameObject.Find("Title").GetComponent<TitleManager>().ShowCredits();
    }
}
