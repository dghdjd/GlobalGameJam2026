using UnityEngine;
using Yarn.Unity;
using System;

public class MusicController : MonoBehaviour
{
    [Serializable]
    public struct MusicTrack
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private MusicTrack[] tracks;

    [Header("Optional crossfade (seconds, 0 = instant)")]
    [SerializeField] private float crossfadeDuration = 0.5f;

    private float baseVolume;

    private void Awake()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();
        if (musicSource != null)
            baseVolume = musicSource.volume;
    }

    [YarnCommand("change_music")]
    public static void ChangeMusic(string trackName)
    {
        var controller = FindFirstObjectByType<MusicController>();
        if (controller == null)
        {
            Debug.LogError("MusicController: No MusicController in scene.");
            return;
        }
        controller.PlayTrack(trackName);
    }

    public void PlayTrack(string trackName)
    {
        if (musicSource == null)
        {
            Debug.LogWarning("MusicController: No AudioSource assigned.");
            return;
        }

        AudioClip clip = null;
        if (tracks != null)
        {
            foreach (var t in tracks)
            {
                if (t.name.Equals(trackName, StringComparison.OrdinalIgnoreCase))
                {
                    clip = t.clip;
                    break;
                }
            }
        }

        if (clip == null)
        {
            Debug.LogWarning($"MusicController: No track named '{trackName}'.");
            return;
        }

        if (crossfadeDuration > 0f && musicSource.isPlaying)
        {
            StartCoroutine(CrossfadeTo(clip));
        }
        else
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    private System.Collections.IEnumerator CrossfadeTo(AudioClip nextClip)
    {
        float elapsed = 0f;
        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(baseVolume, 0f, elapsed / crossfadeDuration);
            yield return null;
        }
        musicSource.volume = 0f;
        musicSource.clip = nextClip;
        musicSource.Play();
        elapsed = 0f;
        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, baseVolume, elapsed / crossfadeDuration);
            yield return null;
        }
        musicSource.volume = baseVolume;
    }
}
