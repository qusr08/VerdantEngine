using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Music_Manager : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip combatBG;
    public AudioClip gardenBG;
    bool isGarden;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayGarden();
    }

    public void Playcomabt()
    {
        audioSource.Stop();
        audioSource.clip = gardenBG;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void PlayGarden()
    {
        
        audioSource.Stop();

        audioSource.clip = gardenBG;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
   
}
