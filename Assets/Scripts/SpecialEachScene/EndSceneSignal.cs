using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneSignal : MonoBehaviour
{
    public Image Frontground;

    public void EndScene()
    {
        ScenesManager.Instance.OnNextScene();
    }

    public void FadeToBlackFrontground()
    {
        InvokeRepeating("FadeToBlackFrontgroundCycle", 0, 0.02f);
    }

    void FadeToBlackFrontgroundCycle()
    {
        Frontground.color += new Color(0f, 0f, 0f, 0.002f);
        if (Frontground.color.a >= 1f)
        {
            CancelInvoke("FadeToBlackFrontgroundCycle");
        }
    }
}
