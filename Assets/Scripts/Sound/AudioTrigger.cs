using System;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public static event Action<string> AudioEvent;
    public string clip_name;
    public bool IsActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !IsActivated && collision.gameObject.CompareTag("Player"))
        {
            IsActivated = true;
            AudioEvent?.Invoke(clip_name);
        }
    }
}
