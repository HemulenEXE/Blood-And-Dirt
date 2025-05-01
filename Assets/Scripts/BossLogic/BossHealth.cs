using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class BossHealth : HealthBot
{
    [SerializeField] GameObject title;
    // Start is called before the first frame update
    public override void Death()
    {
        transform.parent.GetComponent<BossController>().enabled = false;
        transform.parent.GetComponentInChildren<Animator>().SetTrigger("Death1");

        foreach (var item in transform.parent.GetComponentsInChildren<BoxCollider2D>())
        {
            item.enabled = false;
        }

        transform.parent.GetComponent<BossController>().enabled = false;

        if (title != null)
        {
            title.GetComponent<TitleManager>().ShowCredits();
        }
    }
}
