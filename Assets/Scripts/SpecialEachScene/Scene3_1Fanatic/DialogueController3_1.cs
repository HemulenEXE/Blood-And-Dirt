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
    

    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _director.StartDialogue();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    
}
