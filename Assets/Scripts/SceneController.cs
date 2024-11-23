using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Временный скрипт для проверки работы ScenesManager
/// </summary>
public class SceneController : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        ScenesManager.Instance.OnNextScene();
    }

}
