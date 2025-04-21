using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public HashSet<AudioClip> AudioClips;

    public static float MaxDistance = 1f;
    public static float MinDistance = 1f;

    private AudioSource _playerAudioSource;

    private void Start()
    {
        AudioClips = Resources.LoadAll<AudioClip>("Audios").ToHashSet();

        _playerAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AudioSource>();

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

        AudioTrigger.AudioEvent += PlayBackgroundAudio;
    }
    private void SettingAudioSource(AudioSource audioSource, AudioClip audioclip)
    {
        audioSource.enabled = true;

        audioSource.minDistance = MinDistance;
        audioSource.maxDistance = MaxDistance;

        audioSource.spatialBlend = 2.0f;

        audioSource.volume = SettingData.Volume / GetClipVolume(audioclip);
    }

    public void PlayAudio(Transform transform, string audio_name)
    {
        foreach (var x in AudioClips)
        {
            if (x.name.Equals(audio_name))
            {
                var temp = transform.gameObject.GetComponent<AudioSource>();
                if (temp == null) temp = transform.gameObject.AddComponent<AudioSource>();

                SettingAudioSource(temp, x);

                temp.PlayOneShot(x);
                break;
            }
        }
    }
    public void PlayBackgroundAudio(string audio_name)
    {
        var sound = AudioClips.First(x => x.name.Equals(audio_name));
        if (sound != null)
        {
            StartCoroutine(FadeOutAndPlayNewSound(sound));
        }
    }
    private IEnumerator FadeOutAndPlayNewSound(AudioClip newClip) // Для плавной смены сопровождающей музыки
    {
        float fadeDuration = 1f;
        float startVolume = _playerAudioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _playerAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        _playerAudioSource.Stop();
        _playerAudioSource.volume = startVolume;

        SettingAudioSource(_playerAudioSource, newClip);

        yield return new WaitForSeconds(0.5f);

        _playerAudioSource.PlayOneShot(newClip);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _playerAudioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        _playerAudioSource.volume = startVolume;
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
