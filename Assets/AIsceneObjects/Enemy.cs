using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int minVisionDistance, maxVisionDistance;
    public int VisionAngle; //чем больше значение, тем больше угол обзора
    public float EnemySpeed;
    Transform PlayerT, EnemyT, VisionT;
    RaycastHit2D RayOBJ;
    
    
    void Start()
    {
        VisionT=transform.Find("vision");
        PlayerT=GameObject.FindWithTag("Player").transform;
        EnemyT=gameObject.transform;
        InvokeRepeating("Seeker", 0, 0.8f);
    }


    void Seeker()
    {
        RayOBJ=Physics2D.Raycast(EnemyT.position, PlayerT.transform.position-EnemyT.position);
        if (RayOBJ.collider!=null  &&  RayOBJ.collider.CompareTag("Player"))
        {
            if (Vector3.Distance(EnemyT.position, PlayerT.position)<maxVisionDistance && Vector3.Distance(EnemyT.position, PlayerT.position)>Vector3.Distance(VisionT.position, PlayerT.position)+VisionAngle)
            {
                Debug.Log("player big DETECTED!!!");
                //SetBool "IsDetected" to True;
            }
            else if (Vector3.Distance(EnemyT.position, PlayerT.position)<minVisionDistance)
            {
                Debug.Log("player small DETECTED!!!");
                //SetBool "IsDetected" to True;
                
            }
        }
    }


    void Attack()
    {
        
    }


    void Modes()
    {
        
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(EnemyT.position, PlayerT.position)<maxVisionDistance && Vector3.Distance(EnemyT.position, PlayerT.position)>minVisionDistance)//Need add bool "IsDetected"
        {
            EnemyT.position=Vector2.MoveTowards(EnemyT.position, PlayerT.position, EnemySpeed);
        }
    }
}
