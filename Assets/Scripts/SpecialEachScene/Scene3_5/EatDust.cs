using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatDust : MonoBehaviour
{
    private bool triger = true;
    [SerializeField] public GameObject playerEating;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.E) && triger)
        {
            GameObject.FindWithTag("Player").SetActive(false);
            playerEating.SetActive(true);
            StartCoroutine(WaitForEnd());
        }
    }
    private IEnumerator WaitForEnd() 
    {
        yield return new WaitForSeconds(5f);
        ScenesManager.Instance.OnNextScene();
    }
}
