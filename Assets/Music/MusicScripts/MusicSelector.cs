using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class MusicSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioClip> BattleMusic = new List<AudioClip>();
    public List<AudioClip> StressMusic = new List<AudioClip>();
    public List<AudioClip> ComfortMusic = new List<AudioClip>();


    public enum MusicMode
    {
        Comfort,
        Stress,
        Battle
    }
    AudioSource audiosource;

    void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void MusicModeSelector(MusicMode musicMode)
    {
        switch (musicMode)
        {
            case MusicMode.Comfort:
                CancelInvoke();
                InvokeRepeating("FadeOut", 1f, 0.02f);
                Invoke("PlayComfortMusic", 10f);
                InvokeRepeating("FadeIn", 10f, 0.02f);
                break;
            case MusicMode.Stress:
                CancelInvoke();
                InvokeRepeating("FadeOut", 0f, 0.02f);
                Invoke("PlayStressMusic", 5f);
                break;
            case MusicMode.Battle:
                CancelInvoke();
                InvokeRepeating("FadeOutFast", 0f, 0.02f);
                Invoke("PlayBattleMusic", 1f);
                break;
            default:
                Console.WriteLine("Unknown Music Mode");
                break;
        }
    }

    void PlayComfortMusic()
    {
        audiosource.clip = ComfortMusic[UnityEngine.Random.Range(0, ComfortMusic.Count)];
        audiosource.PlayDelayed(2);
        Invoke("PlayComfortMusic", audiosource.clip.length+5);
    }

    void PlayStressMusic()
    {
        audiosource.volume=1f;
        audiosource.clip = StressMusic[UnityEngine.Random.Range(0, StressMusic.Count)];
        audiosource.PlayDelayed(1);
        Invoke("PlayStressMusic", audiosource.clip.length+5);
    }

    void PlayBattleMusic()
    {
        audiosource.volume=1f;
        audiosource.clip = BattleMusic[UnityEngine.Random.Range(0, BattleMusic.Count)];
        audiosource.PlayDelayed(1);
        Invoke("PlayBattleMusic", audiosource.clip.length+5);
    }


    void FadeIn()
    {
        if (audiosource.volume < 0.999f)
        {
            audiosource.volume+=0.005f;
        }
        else
        {
            CancelInvoke("FadeIn");
        }
    }

    void FadeOut()
    {
        if (audiosource.volume > 0.005f)
        {
            audiosource.volume-=0.005f;
        }
        else
        {
            CancelInvoke("FadeOut");
        }
    }

    void FadeOutFast()
    {
        if (audiosource.volume > 0.03f)
        {
            audiosource.volume-=0.05f;
        }
        else
        {
            CancelInvoke("FadeOutFast");
        }
    }
}
