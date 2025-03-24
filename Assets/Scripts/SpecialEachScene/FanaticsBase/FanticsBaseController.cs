using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;

//Отвечает за активацию/деактивацию нужных скриптов при каждом новом попадании на эту сцену
public class FanaticsBaseController : MonoBehaviour
{
    public int LoadingNumber = 0; //Фиксирует число заходов на эту сцену, т.к. каждый раз на ней происходят разные действия
    private static FanaticsBaseController instance;
    public static FanaticsBaseController Instance() { 
        if (instance == null)
            instance = new FanaticsBaseController();
        return instance;
    }
    private void Start()
    {
        switch (LoadingNumber)
        {
            case 1: 
                {
                    break;
                }
            case 2: 
                {
                    GameObject.Find("Player1").SetActive(false);
                    GameObject.Find("DoorOpenRight").transform.GetChild(1).gameObject.SetActive(false); //Отключили триггер на двери тюрьмы
                    PlayableDirector catScene = GameObject.Find("CatScene2").GetComponent<PlayableDirector>();
                    catScene.Play();
                    break;
                }
        
        }
    }
}
