using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene3_1Fanatic : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //завершаем сцену
            ScenesManager.Instance.OnNextScene();
        }
    }
    
}
