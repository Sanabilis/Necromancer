using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;

        [HideInInspector]
        public AudioSource source;
    }

    public static AudioManager instance;

    public Sound music;
    public Sound ambientMusic;
    public Sound ambience;
    public Sound[] sounds;

    void Awake()
    {
        GameManager.Instance.OnPause += Pause;

        instance = this;

        // DontDestroyOnLoad(gameObject);

        music.source = gameObject.AddComponent<AudioSource>();
        music.source.name = music.name;
        music.source.clip = music.clip;
        music.source.loop = music.loop;
        music.source.volume = music.volume;
        music.source.pitch = music.pitch;

        ambientMusic.source = gameObject.AddComponent<AudioSource>();
        ambientMusic.source.name = ambientMusic.name;
        ambientMusic.source.clip = ambientMusic.clip;
        ambientMusic.source.loop = ambientMusic.loop;
        ambientMusic.source.volume = ambientMusic.volume;
        ambientMusic.source.pitch = ambientMusic.pitch;

        ambience.source = gameObject.AddComponent<AudioSource>();
        ambience.source.name = ambience.name;
        ambience.source.clip = ambience.clip;
        ambience.source.loop = ambience.loop;
        ambience.source.volume = ambience.volume;
        ambience.source.pitch = ambience.pitch;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.name = s.name;
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void Start()
    {
        //MusicPlay();
        AmbiencePlay();
    }

    void Update()
    {
        if (ambience.source != null && !ambience.source.isPlaying)
        {
            ambience.source.time = 3f;
            ambience.source.Play();
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }

    public void Pause(bool paused)
    {
        if (paused)
        {
            music.source.volume /= 4f;
            ambientMusic.source.volume /= 4f;
            ambience.source.volume /= 3f;
        }
        else
        {
            music.source.volume = music.volume;
            ambientMusic.source.volume = ambientMusic.volume;
            ambience.source.volume = ambience.volume;
        }

        foreach (Sound sound in sounds)
        {
            if (paused) sound.source.Pause();
            else sound.source.UnPause();
        }
    }

    public void MusicPlay()
    {
        music.source.PlayDelayed(3f);
    }

    public void AmbientMusicPlay()
    {
        ambientMusic.source.Play();
    }

    public void AmbiencePlay()
    {
        ambience.source.Play();
    }
}
