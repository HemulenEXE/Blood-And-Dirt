using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scene3_2 : MonoBehaviour
{
    //Набор массивов ботов
    public GameObject[] BotsArena, BotsRoom1, BotsRoom2, BotsRoom3;
    public GameObject Siren;
    

    //набор массивов открытых дверей
    [SerializeField] GameObject[] DoorsBeforeWave1, DoorsBetweenWave1and2, DoorsBetweenWave2and3;

    //Набор слоев с закрытыми дверями
    [SerializeField] GameObject DoorsLayerBattle, DoorsLayerBeforeWave1, DoorsLayerAfterWave1, DoorsLayerAfterWave2;


    
    void Start()
    {
        InvokeRepeating("FixedUpdateRepeat1Sec", 0f, 1f);

    }

   
    void FixedUpdateRepeat1Sec()
    {
        if (BotsArena.Any(x => x.GetComponent<BotController>().GetIsPlayerDetected()))
        {
            Siren.SetActive(true);
            CancelInvoke("FixedUpdateRepeat1Sec");
        }
    }


    internal void StartWave1()
    {
        
        BotArenaSwitch(BotsArena, false);

        //активация "закрытия" всех дверей
        DoorsLayerBattle.SetActive(true);
        DoorsLayerBeforeWave1.SetActive(false);

        //скрываем открытые двери
        foreach(GameObject door in DoorsBeforeWave1)
        {
            door.SetActive(false);
        }
    
        //включаем наших ботов
        BotArenaSwitch(BotsRoom1, true);
    }


    internal void StartWave2()
    {
        //активация "закрытия" всех дверей
        DoorsLayerBattle.SetActive(true);
        DoorsLayerAfterWave1.SetActive(false);

        //скрываем открытые двери
        foreach(GameObject door in DoorsBetweenWave1and2)
        {
            door.SetActive(false);
        }
                
        //включаем наших ботов
        BotArenaSwitch(BotsRoom2, true);
    }


    internal void StartWave3()
    {
        //активация "закрытия" всех дверей
        DoorsLayerBattle.SetActive(true);
        DoorsLayerAfterWave2.SetActive(false);

        //скрываем открытые двери
        foreach(GameObject door in DoorsBetweenWave2and3)
        {
            door.SetActive(false);
        }

        //включаем наших ботов
        BotArenaSwitch(BotsRoom3, true);
    }


    internal void AfterWave1()
    {
        //деактивация "закрытия" всех дверей
        DoorsLayerBattle.SetActive(false);

        //включаем открытые двери
        foreach(GameObject door in DoorsBetweenWave1and2)
        {
            door.SetActive(true);
        }

        //включаем новый слой с дверями
        DoorsLayerAfterWave1.SetActive(true);
    }


    internal void AfterWave2()
    {
        //деактивация "закрытия" всех дверей
        DoorsLayerBattle.SetActive(false);
        DoorsLayerAfterWave1.SetActive(false);

        //включаем открытые двери
        foreach(GameObject door in DoorsBetweenWave2and3)
        {
            door.SetActive(true);
        }

        //включаем новый слой с дверями
        DoorsLayerAfterWave2.SetActive(true);

    }

/// <summary>
/// Принимает массив ботов и переключает активацию согласно переменной SwitchTo
/// </summary>
/// <param name="BotArray"></param>
/// <param name="SwitchTo"></param>
    void BotArenaSwitch(GameObject[] BotArray, bool SwitchTo)
    {
        foreach(GameObject Bot in BotArray)
        {
            if (Bot != null)
            {
                 Bot.SetActive(SwitchTo);
            }
        }
    }

    public bool AllEnemiesDied(GameObject[] BotArray)
    {
        return BotArray.All(x => x == null);
    }
}
