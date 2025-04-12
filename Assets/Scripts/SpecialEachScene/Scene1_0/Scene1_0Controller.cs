using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//Управляет развитием диалога и последующим переключением сцены 
public class Scene1_0Controller : SwitchScene
{
    [SerializeField] TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    [SerializeField] Animator _animator;
    private bool flag = true; 
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        _dialogue = _director.GetDialogue();
        DialogueWindow = _director.DialogueWindow.gameObject;
        GameObject.Find("Pistol").GetComponent<BoxCollider2D>().enabled = false;
        _director.WithAction = true;
        _director.SetAct();
    }
    void Update()
    {

        if (_dialogue.GetCurentNodeIndex() == 2 && _animator.GetBool("IsShooting") == false && flag)
        {
            //Debug.Log(_animator.GetBool("IsShooting"));
            DialogueWindow.transform.Find("Continue").gameObject.SetActive(false);
            _director.WithEnd = false;

            GameObject.Find("Pistol").GetComponent<BoxCollider2D>().enabled = true;
            flag = false;
        }
        if (_dialogue.GetCurentNodeIndex() == 2 && _animator.GetBool("IsShooting") == true)
        {
            DialogueWindow.transform.Find("Continue").gameObject.SetActive(true);
            _director.WithAction = false;
            _director.SetAct();
        }
        if (DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1))
            _director.WithEnd = true;
        if (!DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1) && !flag)
        {
            flag = true;
            Switch();
        }
    }
}
