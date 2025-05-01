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
        //Запускает действия, которые должны происходить при каждом заходе на сцену по его номеру
        switch (LoadingNumber)
        {
            case 1:
                {
                    break;
                }
            case 2: case 3: //Убрать case 3, когда будет готов 3ий сценарий появления игрока на этой сцене
                {
                    InActive(2);
                    Camera.main.transform.position = new Vector3(-48.23f, -10.1f, -10f);
                    GameObject scene = GameObject.Find("Scene1.2");
                    scene.transform.Find("Player2").gameObject.SetActive(true);
                    PlayableDirector catScene = GameObject.Find("CatScene2").GetComponent<PlayableDirector>();
                    catScene.Play();
                    break;
                }
                /* Раскоментировать и дополнить, когда будет готов 3ий сценарий появления игрока
            case 3:
                {
                    InActive(2);
                    InActive(3);
                    break;
                }*/ 

        }
    }
    //Отключает ненужные объекты из сцены по номеру захода на сцену
    private void InActive(int n)
    {
        switch (n)
        {
            case 2:
                {
                    GameObject.Find("Player1").SetActive(false);
                    GameObject.Find("GameMenu (2)").SetActive(false);
                    GameObject.Find("DoorOpenRight").transform.GetChild(1).gameObject.SetActive(false); //Отключили триггер на двери тюрьмы
                    break;
                }
            case 3:
                {
                    GameObject scene = GameObject.Find("Scene1.2");
                    scene.GetComponentInChildren<SwitchScene>().enabled = false;
                    scene.transform.Find("Soldier").gameObject.SetActive(false);
                    scene.transform.Find("Soldier (1)").gameObject.SetActive(false);
                    scene.transform.Find("Patient1").gameObject.SetActive(false);
                    break;
                }

        }
    }
}
