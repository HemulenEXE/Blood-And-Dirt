using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Переход от сцены 1.1 к сцене 1.2 религиозной линии
public class From1_1To1_2 : SwitchScene
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with tag: " + collision.tag);
        if (collision.tag.Equals("Player"))
        {
            Debug.Log("!!!");
            FanaticsBaseController.Instance().LoadingNumber++;
            Switch();
        }
    }
}
