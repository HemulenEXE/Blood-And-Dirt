using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class From1_1To1_2 : SwitchScene
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt("LoadingNumber", 2);
        PlayerPrefs.Save();
        if (collision.tag == "Player")
            Switch();
    }
}
