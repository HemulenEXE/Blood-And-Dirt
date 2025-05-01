using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneAfterRiver : MonoBehaviour
{
    private SwitchScene _switch;

    private void Start()
    {
        _switch = GetComponent<SwitchScene>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _switch.Switch();
        }
    }
}
