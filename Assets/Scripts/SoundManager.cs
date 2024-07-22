using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [field: SerializeField] private List<AudioClip> backgroundMusic;

    [field: SerializeField] private AudioSource backgroundMusicAudioSource;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        backgroundMusicAudioSource.volume = 0.5f;
        backgroundMusicAudioSource.clip = backgroundMusic[Random.Range(0, backgroundMusic.Count)];
        backgroundMusicAudioSource.loop = true;
        backgroundMusicAudioSource.Play();
    }
}
