using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//Запускает кат-сцену 3 или 4 в зависимости от выбранной реплики
public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool isTriger = false;
    private PlayableDirector catScene;
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
            isTriger = true;
            if (PlayerPrefs.GetInt("nextScene") == -1)
                GameObject.Find("CatScene3").GetComponent<PlayableDirector>().Play(); 
            else GameObject.Find("CatScene4").GetComponent<PlayableDirector>().Play();
        }
    }
}
