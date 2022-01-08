using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
    public float startTime = 0f;
    [Range(0.0f, 1.0f)]
    public float volume;
    [Range(0.25f, 1.75f)]
    public float pitch;    

    private AudioSource source;

    public void SetSource(AudioSource source) {
        this.source = source;
        source.clip = clip;
    }

    public void Play() {
        if (!source.isPlaying ) {
            source.volume = volume;
            source.pitch = pitch;
            source.time = startTime;
            source.Play();
        }
    }

    public void Stop() {
        source.Stop();
    }
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    Sound[] sounds;

    private void Awake() {
        if (instance != null) {
            if (instance == this) Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start() {
        for (int i = 0; i < sounds.Length; i++) {
            GameObject soundObject = new GameObject("Sound_" + sounds[i].name);
            soundObject.transform.SetParent(this.transform);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>()); 
        }
    }

    public void PlaySound(string name) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == name) {
                sounds[i].Play();
                return;
            }
        }
        Debug.Log("No sound found: " + name);
    }
}
