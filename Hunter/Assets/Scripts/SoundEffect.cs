using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private float timeSinceLastSound;
    [SerializeField]
    private float timeBeforeNextSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayAudio();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            timeSinceLastSound += Time.deltaTime;
        }

        if (timeSinceLastSound > timeBeforeNextSound && !audioSource.isPlaying)
        {
            timeSinceLastSound = 0;
            PlayAudio();
        }
    }

    void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        timeBeforeNextSound = Random.Range(5, 20);
    }
}
