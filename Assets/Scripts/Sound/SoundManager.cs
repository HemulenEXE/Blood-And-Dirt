using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public HashSet<AudioClip> AudioClips;

    private void Start()
    {
        AudioClips = Resources.LoadAll<AudioClip>("Audios").ToHashSet();

        ShotGun.AudioEvent += PlayAudio;
        Pistol.AudioEvent += PlayAudio;
        MachineGun.AudioEvent += PlayAudio;
        GrenadeLauncher.AudioEvent += PlayAudio;
        Knife.AudioEvent += PlayAudio;
        ShrapnelMine.AudioEvent += PlayAudio;
        Body.AudioEvent += PlayAudio;
        ShrapnelGrenade.AudioEvent += PlayAudio;
        SmokeGrenade.AudioEvent += PlayAudio;
        InventoryAndConsumableCounterUI.AudioEvent += PlayAudio;
        PlayerHealth.AudioEvent += PlayAudio;
    }

    private void PlayAudio(Transform transform, string audio_name)
    {
        foreach (var x in AudioClips)
        {
            if (x.name.Equals(audio_name))
            {
                var temp = transform.gameObject.GetComponent<AudioSource>();
                if (temp == null) temp = transform.gameObject.AddComponent<AudioSource>();
                temp.enabled = true;
                temp.volume = SettingData.Volume;
                temp.PlayOneShot(x);
                Debug.Log(x.name);
            }
        }
    }
}
