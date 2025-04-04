using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Music_Manager : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip combatBG;
    public AudioClip gardenBG;
    bool isGarden = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayGarden();
    }

    public void Playcomabt()
    {
        if (!isGarden)
        {
            return;
        }
        isGarden = false;
        audioSource.Stop();
        audioSource.clip = gardenBG;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void PlayGarden()
    {
        if(isGarden)
        {
            return;
        }
        isGarden = true;
        audioSource.Stop();

        audioSource.clip = gardenBG;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
   
}
