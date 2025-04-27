using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueByCollider : MonoBehaviour
{
    private ShowDialogueDubl ShowDialogueDubl;

    private void Start()
    {
        ShowDialogueDubl = GetComponent<ShowDialogueDubl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ShowDialogueDubl.StartDialogue();
        }
    }
}
