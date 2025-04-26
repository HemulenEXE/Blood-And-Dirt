using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TitleManager TitleManager;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TitleManager.ShowCredits();
        }
    }
}
