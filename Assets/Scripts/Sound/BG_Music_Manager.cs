using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Music_Manager : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip combatBG;
    public AudioClip gardenBG;
    public AudioClip mapBG;
    public AudioClip victoryBG;

    bool isCombat = false;

    bool isGarden = false;
    bool isVictory = false;
    bool isMap = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMap();
    }

    public void Playcomabt()
    {
        if (isCombat)
        {
            return;
        }
        isCombat = true;
        isVictory = false;
         isMap = false;
        isGarden = false;
        audioSource.Stop();
        audioSource.clip = combatBG;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PlayVictory()
    {
        if (isVictory)
        {
            return;
        }
        isVictory = true;
        isCombat = false;
        isMap = false;
        isGarden = false;
        audioSource.Stop();
        audioSource.clip = victoryBG;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void PlayMap()
    {
        if (isMap)
        {
            return;
        }
        isVictory = false;
        isCombat = false;
        isMap = true;
        isGarden = false;
        audioSource.Stop();
        audioSource.clip = mapBG;

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
        isVictory = false;
        isCombat = false;
        isMap = false;
        isGarden = true;
        audioSource.Stop();

        audioSource.clip = gardenBG;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
   
}
