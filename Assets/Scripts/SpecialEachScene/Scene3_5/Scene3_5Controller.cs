using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;
using CameraLogic.CameraMotion;

//Управляет диалогами и катсценами на сцене 3_5. Навешивается на триггерный коллайдер около храма
public class Scene3_5Controller : MonoBehaviour
{
    //Диалог с солдатом
    [SerializeField]
    public ShowDialogueDubl Director;
    [SerializeField]
    public DialogueWndState DialogueWnd;

    //Диалог со священником
    [SerializeField]
    public ShowDialogueDubl Director1;
    [SerializeField]
    public DialogueWndState DialogueWnd1;

    //Горящие люди
    [SerializeField] public Animator[] BurningPeoples;

    private PlayableDirector catScene1;
    private PlayableDirector catScene2;
    private GameObject player;
    private bool triger = true;
    private void Awake()
    {
        catScene1 = GameObject.Find("CatScene1").GetComponent<PlayableDirector>();
        catScene2 = GameObject.Find("CatScene2").GetComponent<PlayableDirector>();
        player = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        player.GetComponent<PlayerMotion>().enabled = false;
        Director.StartDialogue();
        StartCoroutine(StartCatScene1());
    }
    private IEnumerator StartCatScene1()
    {
        while (DialogueWnd.currentState != DialogueWndState.WindowState.EndPrint)
            yield return new WaitForFixedUpdate();
        catScene1.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && triger)
        {
            Vector3 churchman = GameObject.Find("Churchman").transform.position;
            player.GetComponent<PlayerMotion>().enabled = false;
            player.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            player.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Prefabs/Sprites/Persons/MainCharacter");
            Camera.main.GetComponent<CameraMove>().enabled = false;
            Vector3 newPos = new Vector3(churchman.x, churchman.y, -10);
            Camera.main.transform.DOMove(newPos, 1f).SetId(this);
            Camera.main.DOOrthoSize(6f, 1f).SetId(this);

            Director1.StartDialogue();
            StartCoroutine(ActionDuringDialogue());
            triger = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private IEnumerator ActionDuringDialogue()
    {
        Dialogue dialogue = Director1.GetDialogue();
        while (dialogue.GetCurentNodeIndex() < 5)
            yield return new WaitForFixedUpdate();

        Debug.Log("Start set Animations");
        GameObject dust = GameObject.Find("Dust");
        foreach (Animator anim in BurningPeoples)
        {
            Vector3 pos = anim.gameObject.transform.position;
            anim.SetBool("burn", true);
            anim.gameObject.transform.position = new Vector3(pos.x + 0.2f, pos.y + 0.4f, pos.z);
        }
       
        for (int i = 0; i < dust.transform.childCount; i++)
            dust.transform.GetChild(i).GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 4f);

        while (DialogueWnd1.gameObject.transform.GetChild(4).gameObject.activeSelf)
            yield return new WaitForFixedUpdate();
        
        catScene2.Play();

        while (dialogue.GetCurentNodeIndex() != 6)
            yield return new WaitForFixedUpdate();

        DialogueWnd1.gameObject.transform.GetChild(4).gameObject.SetActive(false);
        player.GetComponent<PlayerMotion>().enabled = true;
        player.transform.GetChild(0).GetComponent<Animator>().enabled = true;
    }
    public void CatScene1End()
    {
        player.GetComponent<PlayerMotion>().enabled = true;
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }

}
