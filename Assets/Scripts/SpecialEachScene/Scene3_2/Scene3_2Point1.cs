using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3_2Point1 : MonoBehaviour
{
     public Scene3_2 scene3_2;
    
    void Start()
    {
        InvokeRepeating("FixedUpdateRepeat1Sec", 2f, 2f);
    }

    void FixedUpdateRepeat1Sec()
    {
        if(scene3_2.AllEnemiesDied(scene3_2.BotsRoom1))
        {
            scene3_2.AfterWave1();
            CancelInvoke("FixedUpdateRepeat1Sec");
        }
    }



    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            scene3_2.StartWave1();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
