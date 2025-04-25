using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3_2Point3 : MonoBehaviour
{
    public Scene3_2 scene3_2;
    
    void Start()
    {
        InvokeRepeating("FixedUpdateRepeat1Sec", 2f, 2f);
    }

    void FixedUpdateRepeat1Sec()
    {
        if(scene3_2.AllEnemiesDied(scene3_2.BotsRoom3))
        {
           //завершаем сцену
            Debug.Log("Контент сцены завершен. Вставьте переход к след. сцене, где комментарий ////завершаем сцену: Assets/scripts/SpecialEachScene/Scene3_2/Scene3_2Point3");
        }
    }


    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            scene3_2.StartWave3();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
   
}
