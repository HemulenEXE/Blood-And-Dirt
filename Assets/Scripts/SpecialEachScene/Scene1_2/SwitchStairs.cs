using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Отвечает за переход со сцены 1_2 на сцену 1_3
public class SwitchStairs : SwitchScene
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Transform player = collision.transform;   
            Vector3 rot = player.eulerAngles;
            rot.z = 0;
            Quaternion rotate = Quaternion.Euler(rot);
            Vector3 position = new Vector3(player.position.x + 0.6f, player.position.y, player.position.z);

            if (PlayerInitPosition.Instance != null)
            {
                PlayerInitPosition.Instance.SavePosition(SceneManager.GetActiveScene().buildIndex, position, rotate);
            }
            Switch();
        }
    }
}
