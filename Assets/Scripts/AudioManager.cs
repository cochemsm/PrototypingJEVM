using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    
    // AudioClip index
    public const int EngineSound = 0;
    public const int PlayerWalking = 1;
    public const int PlayerDamage = 2;
    public const int Exposion = 3;

    public const int BackgroundMusic = 4;
    public const int BossMusic = 5;

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
        StartCoroutine(EngineSoundPlay());
    }

    public void PlaySound(int index) {
        source.PlayOneShot(clips[index]);
    }

    public void PlayMusic(int index) {
        source.clip = clips[index];
        source.Play();
    }

    private IEnumerator EngineSoundPlay() {
        PlaySound(AudioManager.EngineSound);
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(EngineSoundPlay());
    }
}
