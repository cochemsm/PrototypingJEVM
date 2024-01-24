using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    private static MusicManager instance;
    public static MusicManager Instance => instance;

    private AudioSource source;
    [SerializeField] private AudioClip[] clips;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }

    public void ChangeMusic(string musicName) {
        foreach (var clip in clips) {
            if (clip.name == musicName) {
                source.clip = clip;
                source.Play();
            }
        }
    }
}
