using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Enemy : MonoBehaviour
{
    public float minVisionDistance, maxVisionDistance;
    public float VisionAngle; //чем больше значение, тем больше угол обзора
    public float EnemySpeed;

    bool IsDetect=false;
    public string WeaponType;
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
                IsDetect=true;
                Debug.Log("Игрок обнаружен зрением врага");
                InvokeRepeating("Hunt", 0, 0.015f);
                //
                CancelInvoke("Seeker");
            }
            else if ((Vector3.Distance(EnemyT.position, PlayerT.position)<minVisionDistance && Random.Range(0, 10)==4)  ||  (Vector3.Distance(EnemyT.position, PlayerT.position)<1.5f))
            {
                IsDetect=true;
                Debug.Log("Враг неожиданно обернулся...");
                InvokeRepeating("Hunt", 0, 0.015f);
                //
                CancelInvoke("Seeker");
            }
        }
    }




void Hunt()
    {
         if (Vector3.Distance(EnemyT.position, PlayerT.position)<maxVisionDistance  && IsDetect)
        {
            //EnemyT.position=Vector2.MoveTowards(EnemyT.position, PlayerT.position, EnemySpeed);
            Invoke("Damaging", 0.9f);
        }
        else
        {
            IsDetect=false;
            InvokeRepeating("Seeker", 0, 0.8f);
            Debug.Log("преследование прекращено");
            
            CancelInvoke("Hunt");
        }
    }




    void Damaging()
    {
        if (WeaponType=="Knife"  && Vector3.Distance(VisionT.position, PlayerT.position)<0.8f && Vector3.Distance(EnemyT.position, PlayerT.position)<1.3f )
        {
            //InvokeRepeating("", 0.3f, 0.7f);   //вызываем метод атаки ножом
            Debug.Log("Нанесен ближний урон");
        }
        else if (WeaponType=="AK")
        {
            //InvokeRepeating("", 0.3f, 0.2f);  //вызываем стрельбу из автомата
            Debug.Log("Нанесен урон огнестрелом");
        }
    }

    
    


    void Modes()
    {
        
    }

    void FixedUpdate()
    {
       //Debug.Log($"Vision->Player{Vector3.Distance(VisionT.position, PlayerT.position)}");
       //Debug.Log($"Enemy->Player{Vector3.Distance(EnemyT.position, PlayerT.position)}");
    }
}
