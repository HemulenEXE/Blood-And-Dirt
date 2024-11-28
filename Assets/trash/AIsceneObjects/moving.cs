using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)){
            rb.velocity=new Vector2(0, 3);
        }
        
        if (Input.GetKey(KeyCode.A)){
            rb.velocity=new Vector2(-3, 0);
        }
        
        if (Input.GetKey(KeyCode.S)){
            rb.velocity=new Vector2(0, -3);
        }
        
        if (Input.GetKey(KeyCode.D)){
            rb.velocity=new Vector2(3, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.Q)){
            rb.gameObject.transform.Rotate(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.E)){
            rb.gameObject.transform.Rotate(0, 0, -90);
        }
        
    }
}
