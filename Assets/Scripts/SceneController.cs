using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        ScenesManager.Instance.OnNextScene();
    }

}
