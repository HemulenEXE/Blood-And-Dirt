using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//После окончания диалога и конца timelinе в сцене 1.3б происходит переключение на след. сцену
public class End1_2 : MonoBehaviour
{
    [SerializeField] TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool catSceneOver = false;
    public void SetCatSceneOver() { catSceneOver = !catSceneOver; }
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
        //Если завершились и анимация и диалог
        if (!DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1) && catSceneOver) 
        {
            PlayerPrefs.SetInt("LoadingNumber", 3);
            PlayerPrefs.Save();
            GameObject.Find("CatScene5").GetComponent<PlayableDirector>().Play();
        }
    }
}
