using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] destroyNoise;
    
    public void PlayRandomDestroyNoise()
    {
        int clipToPlay = Random.Range(0, destroyNoise.Length);

        // Play tha clip
        destroyNoise[clipToPlay].Play();
    }

}
