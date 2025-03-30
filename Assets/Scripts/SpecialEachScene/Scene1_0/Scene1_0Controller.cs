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
        Destroy(GameObject.Find("Pistol").GetComponent<GunPickUp>());
    }
    void Update()
    {
        if (PlayerPrefs.GetInt("nextScene", 0) == 1)
            ScenesManager.Instance.OnSelectedScene(SceneManager.GetActiveScene().buildIndex);
        if (_dialogue.GetCurentNodeIndex() == 2 && _animator.GetBool("IsShooting") == false && flag)
        {
            Debug.Log(2); Debug.Log(_animator.GetBool("IsShooting"));
            DialogueWindow.transform.Find("Continue").gameObject.SetActive(false);
            _director.WithEnd = false;

            GameObject.Find("Pistol").AddComponent<GunPickUp>();
            GameObject.Find("Pistol").GetComponent<GunPickUp>().Icon = Resources.Load<Sprite>("Sprites/Icons/Pistol");
            flag = false;
        }
        if (_dialogue.GetCurentNodeIndex() == 2 && _animator.GetBool("IsShooting") == true)
        {
            DialogueWindow.transform.Find("Continue").gameObject.SetActive(true);
            Debug.Log(3);
        }
        if (DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1))
            _director.WithEnd = true;
        if (!DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1))
            Switch();
    }
}
