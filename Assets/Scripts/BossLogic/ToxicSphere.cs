using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSphere : MonoBehaviour
{
    //[SerializeField] float radiusSphere;
    [SerializeField] int damage;
    [SerializeField] int timeLife;

    private PlayerHealth health = null;
    void Start()
    {
        //GetComponent<CircleCollider2D>().radius = radiusSphere;
        Destroy(gameObject,timeLife);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(health == null) 
            {
                collision.gameObject.GetComponent<PlayerHealth>().GetDamage(damage);
            }
            else
            {
                health.GetDamage(damage);
            }
            
        }
    }
}
