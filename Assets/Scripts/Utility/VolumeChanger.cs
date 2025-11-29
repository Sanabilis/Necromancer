using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class VolumeChanger : MonoBehaviour
{
    private bool _volumeUp = false;
    private float _volume = 0f;

    void Update()
    {
        if (_volumeUp)
        {
            if (GameManager.Instance.IsGamePaused())
                _volume = Mathf.Min(0.5f / 4f, _volume + Time.deltaTime / 40f);
            else
                _volume = Mathf.Min(0.5f, _volume + Time.deltaTime / 10f);
        }
        else
        {
            _volume = Mathf.Max(0f, _volume - Time.deltaTime / 10f);
        }

        AudioManager.instance.ambientMusic.volume = _volume;
        AudioManager.instance.ambientMusic.source.volume = _volume;
    }

    void Start()
    {
        AudioManager.instance.AmbientMusicPlay();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            _volumeUp = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            _volumeUp = false;
        }
    }
}
