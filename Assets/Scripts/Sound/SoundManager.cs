using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public HashSet<AudioClip> AudioClips;
    public float MaxDistance = 1f;
    public float MinDistance = 1f;

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
        HealthBot.AudioEvent += PlayAudio;
        PlayerMotion.AudioEvent += PlayAudio;
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

                temp.minDistance = MinDistance;
                temp.maxDistance = MaxDistance;

                temp.spatialBlend = 2.0f;

                temp.volume = SettingData.Volume / GetClipVolume(x);

                temp.PlayOneShot(x);
                break;
            }
        }
    }
    private float GetClipVolume(AudioClip clip) // Для нормализации громкости клипа
    {
        float[] arr = new float[clip.samples * clip.channels];
        clip.GetData(arr, 0);

        float sum = 0f;
        for (int i = 0; i < arr.Length; ++i) sum += Mathf.Abs(arr[i]);

        float result = sum / arr.Length;
        return result;
    }
}
