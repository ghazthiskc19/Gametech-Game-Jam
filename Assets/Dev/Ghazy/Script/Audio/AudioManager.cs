using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] sfxClips;
    public AudioClip[] bgmClips;
    [SerializeField] private bool sfxLocked = false;
    private Dictionary<int, AudioClip> sfxById = new();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < sfxClips.Length; i++)
            {
                if (!sfxById.ContainsKey(i))
                    sfxById.Add(i, sfxClips[i]);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        bgmSource.volume = musicVol;
        sfxSource.volume = sfxVol;
    }
    public void PlaySFXById(int id)
    {
        if (sfxById.ContainsKey(id))
        {
            sfxSource.PlayOneShot(sfxById[id]);
        }
        else
        {
            Debug.LogWarning($"SFX with id {id} not found!");
        }
    }
    public void PlayBGM(string name)
    {
        AudioClip clip = FindClip(bgmClips, name);
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM not found: " + name);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void PlaySFX(string name, bool preventOverlap = false)
    {
        AudioClip clip = FindClip(sfxClips, name);

        if (clip != null)
        {
            if (preventOverlap && sfxLocked) return;

            sfxSource.PlayOneShot(clip);

            if (preventOverlap)
                StartCoroutine(SFXCooldown(clip.length));
        }
        else
        {
            Debug.LogWarning("SFX not found: " + name);
        }
    }

    private IEnumerator SFXCooldown(float duration)
    {
        sfxLocked = true;
        yield return new WaitForSeconds(duration);
        sfxLocked = false;
    }
    private AudioClip FindClip(AudioClip[] clips, string name)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == name)
                return clip;
        }
        return null;
    }
    public void SetMusicVolume(float value)
    {
        bgmSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }
    public float GetBGMVolume() => bgmSource.volume;
    public float GetSFXVolume() => sfxSource.volume;
}
