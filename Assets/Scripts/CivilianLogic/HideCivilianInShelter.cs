using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideCivillian : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collider)
    {
        CivilianController controller = collider.gameObject.GetComponent<CivilianController>();
        Debug.Log(controller);
        if(controller != null && controller.IsFlee())
        {
            controller.gameObject.SetActive(false);
        }
    }
}
