using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Переход на новую сцену при попадании в область
public class SwitchOnEnter : SwitchScene
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Switch();
    }
}
