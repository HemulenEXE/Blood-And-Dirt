using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3_2Point3 : MonoBehaviour
{
    public Scene3_2 scene3_2;
    
    void Start()
    {
        InvokeRepeating("FixedUpdateRepeat1Sec", 0f, 1f);
    }

    void FixedUpdateRepeat1Sec()
    {
        if(scene3_2.BotsRoom3.Length==0)
        {
           //завершаем сцену
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
