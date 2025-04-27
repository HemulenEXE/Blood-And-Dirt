using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//Контроллер для сцены 4_5. Всё взаимодействие с ним через tmeline
public class Controller : MonoBehaviour
{
    [SerializeField] PlayableDirector CatScene1;
    [SerializeField] DialogueWndState DialogueWnd;
    [SerializeField] DialogueWndState DialogueWnd1;
    [SerializeField] GameObject Budda;
    private List<Transform> people = new List<Transform>(6);
    private List<Vector3> positions = new List<Vector3> (6);
    private List<Quaternion> rotations = new List<Quaternion>(6);
    private ShowDialogueDubl director;
    private Dialogue dialogue; 
    private void Awake()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        Transform muller = GameObject.Find("Мюллер  анимация").transform;
        Transform gector = GameObject.Find("гектор анимация").transform;
        Transform tomas = GameObject.Find("томас анимация 1").transform;
        Transform soldier1 = GameObject.Find("GreenSoldier1").transform;
        Transform soldier2 = GameObject.Find("GreenSoldier1 (1)").transform;

        people.Add(player);
        people.Add(muller);
        people.Add(gector);
        people.Add(tomas);
        people.Add(soldier1);
        people.Add(soldier2);
        foreach (Transform p in people)
        {
            Debug.Log(p);
        }
    }
    public void SavePositions()
    {
        for (int i = 0; i < people.Count; i++)
        {
            var animator = people[i].GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
            positions.Add(people[i].position);
            rotations.Add(people[i].rotation);
            //Debug.Log($"[{i}]: {people[i].position} {people[i].rotation}");
        }
    }
    public void RestorePositions()
    {
        for (int i = 0; i < people.Count; i++)
        {
            people[i].position = positions[i];
            people[i].rotation = rotations[i];
            //Debug.Log($"[{i}]: {people[i].position} {people[i].rotation}");
        }
        positions.Clear();
        rotations.Clear();
    }
    private void EnableAnimators()
    {
        foreach (Transform p in people)
        {
            var animator = p.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
            }
        }
    }
    public void CheckDialogue()
    {
        StartCoroutine(_CheckDialogue());
    }
    private IEnumerator _CheckDialogue()
    {
        SavePositions();
        SetAct();
        CatScene1.Pause();
        RestorePositions();

        Debug.Log("Pause");
        while (DialogueWnd.currentState != DialogueWndState.WindowState.EndPrint)
            yield return new WaitForFixedUpdate();

        EnableAnimators();
        CatScene1.Play();
    }

    public void StartNextScene()
    {
        SavePositions();
        RestorePositions();

        people[0].GetComponent<PlayerMotion>().enabled = false;
        
        director = Budda.GetComponent<ShowDialogueDubl>();
        dialogue = director.GetDialogue();

        director.WithEnd = false;
        director.StartDialogue();

        StartCoroutine(_EndDialogue());
    }
    private IEnumerator _EndDialogue()
    {
        bool flag = true;
        bool flag1 = true;

        while (DialogueWnd1.currentState != DialogueWndState.WindowState.EndPrint)
        {
            if (flag && dialogue.GetCurentNodeIndex() == 13)
            {
                //director.WithAction = true;
                //director.SetAct();
                SetAct();

                DialogueWnd1.gameObject.transform.GetChild(4).gameObject.SetActive(false);
                flag = false;
            }
            if (flag1 && false) //Заменить на отслеживание выполнения анимации
            {
                DialogueWnd1.gameObject.transform.GetChild(4).gameObject.SetActive(true);
                flag1 = false;
            }

            yield return new WaitForFixedUpdate();
        }
        ScenesManager.Instance.OnNextScene();
    }
    public void SetAct()
    {
        GameObject player = GameObject.FindWithTag("Player");

        player.GetComponent<PlayerGrenade>().enabled = !player.GetComponent<PlayerGrenade>().enabled;
        player.GetComponent<PlayerKnife>().enabled = !player.GetComponent<PlayerKnife>().enabled;
        player.GetComponent<PlayerShooting>().enabled = !player.GetComponent<PlayerShooting>().enabled;
        player.GetComponent<PlayerMotion>().enabled = !player.GetComponent<PlayerMotion>().enabled;
    }
}
