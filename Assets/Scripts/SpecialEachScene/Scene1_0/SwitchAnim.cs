using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnim : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            this.GetComponent<Animator>().SetBool("IsShooting", true);
        }
    }
}
