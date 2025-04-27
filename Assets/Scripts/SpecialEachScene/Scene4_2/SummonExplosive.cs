using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SummonExplosive : MonoBehaviour
{
    public static event Action<Transform, string> Explosive;

    public  void SummonSound()
    {
        Debug.Log("SUMMON EXPLOSION");
        Explosive.Invoke(gameObject.transform, "SoundExplosive");
    }
}
