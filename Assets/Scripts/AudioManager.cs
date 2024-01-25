using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance;
    public static AudioManager Instance => instance;

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

    private void Start() {
        StartCoroutine(EngineSound());
    }

    public void PlaySound(string clipName) {
        foreach (var clip in clips) {
            if (clip.name == clipName) {
                source.PlayOneShot(clip);
                return;
            }
        }
    }

    private IEnumerator EngineSound() {
        PlaySound("engine_sound");
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(EngineSound());
    }
}
