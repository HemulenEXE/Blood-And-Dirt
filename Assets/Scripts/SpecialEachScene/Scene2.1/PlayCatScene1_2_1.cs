using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//Запускает стартовую кат-сцену после диалога с Гектором в сцене 2.1 средней линии. Навешивается на Гектора
public class PlayCatScene1_2_1 : MonoBehaviour
{
    [SerializeField] private TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private bool isTriger = false;
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        this.GetComponent<ShowDialogueDubl>().StartDialogue();
        GameObject.FindWithTag("Player").GetComponent<PlayerMotion>().enabled = false;
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
            GameObject.Find("CatScene1").GetComponent<PlayableDirector>().Play();
        }
    }
    //Активирует возможность двигаться после катсцены
    public void ActivMotion()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMotion>().enabled = true;
    }
}
