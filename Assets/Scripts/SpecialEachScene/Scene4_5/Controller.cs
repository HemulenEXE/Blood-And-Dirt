using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Controller : MonoBehaviour
{
    [SerializeField] PlayableDirector CatScene1;
    [SerializeField] PlayableDirector CatScene2;
    [SerializeField] DialogueWndState DialogueWnd;
    [SerializeField] DialogueWndState DialogueWnd1;
    [SerializeField] GameObject Budda;
    private Transform player;
    private Transform muller;
    private Transform gector;
    private Transform tomas;
    private Transform soldier1;
    private Transform soldier2;
    private List<Transform> people = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        muller = GameObject.Find("ћюллер  анимаци€").transform;
        gector = GameObject.Find("гектор анимаци€").transform;
        tomas = GameObject.Find("томас анимаци€ 1").transform;
        soldier1 = GameObject.Find("GreenSoldier1").transform;
        soldier2 = GameObject.Find("GreenSoldier1 (1)").transform;

        people.Add(player);
        people.Add(muller);
        people.Add(gector);
        people.Add(tomas);
        people.Add(soldier1);
        people.Add(soldier2);
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
            positions[i] = people[i].position;
            rotations[i] = people[i].rotation;
        }
    }
    public void RestorePositions()
    {
        for (int i = 0; i < people.Count; i++)
        {
            people[i].position = positions[i];
            people[i].rotation = rotations[i];
            Debug.Log($"[i]: {people[i].position} {people[i].rotation}");
        }
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
        
        ShowDialogueDubl director = Budda.GetComponent<ShowDialogueDubl>();
        Dialogue dialogue = director.GetDialogue();

        director.WithEnd = false;
        director.StartDialogue();

        StartCoroutine(_EndDialogue());
    }
    private IEnumerator _EndDialogue()
    {
        while (DialogueWnd1.currentState != DialogueWndState.WindowState.EndPrint)
            yield return new WaitForFixedUpdate();
        ScenesManager.Instance.OnNextScene();
    }
}
