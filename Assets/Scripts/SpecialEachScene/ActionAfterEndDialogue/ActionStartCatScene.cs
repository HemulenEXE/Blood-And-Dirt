using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActionStartCatScene : MonoBehaviour, IAction 
{
    [SerializeField] public PlayableDirector timeline;
    public void DoIt()
    {
        Debug.Log("Start");
        timeline.Play();
    }
}
