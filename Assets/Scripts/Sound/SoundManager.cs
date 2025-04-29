using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public HashSet<AudioClip> AudioClips;

    public static float MaxDistance = 1f;
    public static float MinDistance = 1f;

    [SerializeField]
    private AudioClip _startBackGroundAudio;

    public static AudioClip _currentBackGroundAudio;

    private static AudioSource _backgroundAudioSource;

    private void Start()
    {
        AudioClips = Resources.LoadAll<AudioClip>("Audios").ToHashSet();

        _backgroundAudioSource = this.GetComponent<AudioSource>();

        SettingAudioSource(_backgroundAudioSource);
        _backgroundAudioSource.spatialBlend = 0f;

        PlayBackgroundAudio(_startBackGroundAudio);
    }

    private void OnEnable()
    {
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
        StationaryShrapnelGun.AudioEvent += PlayAudio;
        SummonExplosive.Explosive += PlayAudio;
        PlaceToHide.AudioEvent += PlayAudio;

        AudioTrigger.AudioEvent += PlayBackgroundAudio;
        TitleManager.AudioEvent += PlayBackgroundAudio;
        GameMenu.AudioEvent += PlayBackgroundAudio;
        GameOverMenu.AudioEvent += PlayBackgroundAudio;
    }
    private void OnDisable()
    {
        ShotGun.AudioEvent -= PlayAudio;
        Pistol.AudioEvent -= PlayAudio;
        MachineGun.AudioEvent -= PlayAudio;
        GrenadeLauncher.AudioEvent -= PlayAudio;
        Knife.AudioEvent -= PlayAudio;
        ShrapnelMine.AudioEvent -= PlayAudio;
        Body.AudioEvent -= PlayAudio;
        ShrapnelGrenade.AudioEvent -= PlayAudio;
        SmokeGrenade.AudioEvent -= PlayAudio;
        InventoryAndConsumableCounterUI.AudioEvent -= PlayAudio;
        PlayerHealth.AudioEvent -= PlayAudio;
        HealthBot.AudioEvent -= PlayAudio;
        PlayerMotion.AudioEvent -= PlayAudio;
        StationaryShrapnelGun.AudioEvent -= PlayAudio;
        SummonExplosive.Explosive -= PlayAudio;
        PlaceToHide.AudioEvent -= PlayAudio;

        AudioTrigger.AudioEvent -= PlayBackgroundAudio;
        TitleManager.AudioEvent -= PlayBackgroundAudio;
        GameMenu.AudioEvent -= PlayBackgroundAudio;
        GameOverMenu.AudioEvent -= PlayBackgroundAudio;
    }
    private void SettingAudioSource(AudioSource audioSource)
    {
        audioSource.enabled = true;

        audioSource.minDistance = MinDistance;
        audioSource.maxDistance = MaxDistance;

        audioSource.spatialBlend = 2.0f;

        audioSource.volume = SettingData.Volume;
    }

    public void PlayAudio(Transform transform, string audio_name)
    {
        foreach (var x in AudioClips)
        {
            if (x.name.Equals(audio_name))
            {
                var temp = transform.gameObject.GetComponent<AudioSource>();
                if (temp == null) temp = transform.gameObject.AddComponent<AudioSource>();

                SettingAudioSource(temp);

                temp.PlayOneShot(x);
                break;
            }
        }
    }
    public void PlayBackgroundAudio(AudioClip clip)
    {
        _currentBackGroundAudio = clip;
        if (clip == null) return;
        StopAllCoroutines();
        StartCoroutine(FadeOutAndPlayNewSound(clip));
    }
    public void PlayBackgroundAudio(string audio_name)
    {
        _currentBackGroundAudio = AudioClips.First(x => x.name.Equals(audio_name));
        StopAllCoroutines();
        StartCoroutine(FadeOutAndPlayNewSound(_currentBackGroundAudio));
    }
    private IEnumerator FadeOutAndPlayNewSound(AudioClip newClip) // Для плавной смены сопровождающей музыки
    {
        float fadeDuration = 1f;

        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            _backgroundAudioSource.volume = Mathf.Lerp(SettingData.Volume, 0, t / fadeDuration);
            yield return null;
        }

        _backgroundAudioSource.volume = SettingData.Volume;
        _backgroundAudioSource.Stop();

        yield return new WaitForSecondsRealtime(0.5f);

        _backgroundAudioSource.PlayOneShot(newClip);

        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            _backgroundAudioSource.volume = Mathf.Lerp(0, SettingData.Volume, t / fadeDuration);
            yield return null;
        }
        _backgroundAudioSource.volume = SettingData.Volume;
    }
    private void OnDestroy()
    {
        Debug.Log("SoundManager was destroyed");
    }
}
