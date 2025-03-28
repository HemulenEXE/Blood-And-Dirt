using PlayerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;

//Отвечает за активацию/деактивацию нужных скриптов при каждом новом попадании на эту сцену
public class FanaticsBaseController : MonoBehaviour
{
    private int LoadingNumber; //Фиксирует число заходов на эту сцену, т.к. каждый раз на ней происходят разные действия

    private void Start()
    {
        Debug.Log("Scene is loaded!");
        Debug.Log($"{PlayerPrefs.HasKey("LoadingNumber")} <-> {PlayerPrefs.GetInt("LoadingNumber")}");
        LoadingNumber = PlayerPrefs.GetInt("LoadingNumber", 1);
        Debug.Log(LoadingNumber);
        switch (LoadingNumber)
        {
            case 1: 
                {
                    break;
                }
            case 2: 
                {
                    Debug.Log(LoadingNumber);
                    GameObject.Find("Player1").SetActive(false);
                    GameObject.Find("DoorOpenRight").transform.GetChild(1).gameObject.SetActive(false); //Отключили триггер на двери тюрьмы
                    PlayableDirector catScene = GameObject.Find("CatScene2").GetComponent<PlayableDirector>();
                    catScene.Play();
                    catScene.stopped += DialoguePreparing;
                    break;
                }
            default: Debug.Log($"OTHER LODING NUMBER = {LoadingNumber}"); break;
        
        }
    }
    //Отключает возможность двигаться во время проигрывания диалога в кат-сцене 
    private void DialoguePreparing(PlayableDirector anim)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMotion>().enabled = false;
    }
    //Включает эту возможность обратно
    public void DialogueEnd() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMotion>().enabled = true;
    }

}
