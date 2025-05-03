using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCatScene : MonoBehaviour
{
    [SerializeField] private TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool isTriger = false;
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
    }
    void Update()
    {
        if (_director.FileName == FileName)
        {
            DialogueWindow = _director.DialogueWindow.gameObject;
            _dialogue = _director.GetDialogue();
        }

        if (!isTriger && !DialogueWindow.activeSelf && (_dialogue.GetCurentNode().exit == "True"))
        {
            Debug.Log("��������� ������� �������������!");
            isTriger = true;
            PlayerPrefs.SetInt("LoadingNumber", 3);
            PlayerPrefs.Save();
            GameObject.Find("CatScene4").GetComponent<PlayableDirector>().Play();
        }
    }
}
