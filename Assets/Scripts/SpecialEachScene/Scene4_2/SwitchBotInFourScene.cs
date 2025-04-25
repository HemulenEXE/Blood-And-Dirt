using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBotInFourScene : MonoBehaviour
{
    [SerializeField] GameObject needSetActive;
    [SerializeField] GameObject needSetDisable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            needSetActive.SetActive(true);
            needSetDisable.GetComponent<BotSceneManager>().enabled = false;
            Destroy(this);
        }
    }
}
