using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneSignal : MonoBehaviour
{
    public void EndScene()
    {
        ScenesManager.Instance.OnNextScene();
    }
}
