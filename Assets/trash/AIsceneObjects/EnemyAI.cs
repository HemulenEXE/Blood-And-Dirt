using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Vector3 EnemyTransform;
    GameObject Player;
    RaycastHit2D RayOBJ;
   
    void Awake(){
    
    }
    // Start is called before the first frame update
    void Start()
    {
        Player=GameObject.FindWithTag("Player");
        InvokeRepeating("Seeker", 0, 0.8f);
    }


    void Seeker()
    {
        EnemyTransform=gameObject.transform.position;
        RayOBJ=Physics2D.Raycast(EnemyTransform, Player.transform.position-EnemyTransform);
        if (RayOBJ.collider!=null )
        {
        if ( RayOBJ.collider.CompareTag("Player"))
        {
            Debug.DrawRay(EnemyTransform, Player.transform.position-EnemyTransform, Color.green);
            CancelInvoke("Seeker");
            Debug.Log("Player is detected!!! Attack him!");
            //отослать данные о координатах игрока и статусе атаки(стрельба/запинывание)
        }
        else
        {
            Debug.DrawRay(EnemyTransform, Player.transform.position-EnemyTransform, Color.red);
        }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    void Idle (){
    }
    //private void OnTriggerEnter2D(Collider2D OBJ)
    //{
        //EnemyTransform=gameObject.transform.position;
        //RayOBJ=Physics2D.Raycast(EnemyTransform, OBJ.gameObject.transform.position-EnemyTransform);
        //if (RayOBJ.collider!=null )
        //{
        
        //if ( RayOBJ.collider.CompareTag("Player"))
        //{
          //  Debug.DrawRay(EnemyTransform, OBJ.gameObject.transform.position-EnemyTransform, Color.green);
          //  Debug.Log("Player is detected!!! Attack him!");
            //отослать данные о координатах игрока и статусе атаки(стрельба/запинывание)
        //}
        //else
        //{
         //   Debug.DrawRay(EnemyTransform, OBJ.gameObject.transform.position-EnemyTransform, Color.red);
        //}
        //}
    ///}
}
