using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAfterDialogue : MonoBehaviour
{
    [SerializeField] public DialogueWndState window;
    [SerializeField] public GameObject Door1;
    [SerializeField] public GameObject Door2;
    private bool flag = true;
    private void Start()
    {
        Door1.GetComponent<CircleCollider2D>().enabled = false;
        Door2.GetComponent<CircleCollider2D>().enabled = false;
        Door1.GetComponent<ClickedObject>().enabled = false;
        Door2.GetComponent<ClickedObject>().enabled = false;
    }
    private void FixedUpdate()
    {
        if (flag && window.currentState == DialogueWndState.WindowState.EndPrint)
        {
            flag = false;
            Door1.GetComponent<CircleCollider2D>().enabled = true;
            Door2.GetComponent<CircleCollider2D>().enabled = true;
            Door1.GetComponent<ClickedObject>().enabled = true;
            Door2.GetComponent<ClickedObject>().enabled = true;
        }
    }
}
