using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioSource[] soundEffects;
    private void Awake()
    {
        instance = this;
    }

    public void PlaySfx(int sfxToPlay)
    {
        //soundEffects[sfxToPlay].Stop();
        soundEffects[sfxToPlay].Play();
    }

    public void PlaySfxPitched(int sfxToPlay)
    {
        soundEffects[sfxToPlay].pitch = Random.Range(0.6f, 1.2f);

        PlaySfx(sfxToPlay);
    }

    public void StopSfx()
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].Stop();
        }
    }

    public void StopOneSfx()
    {
        soundEffects[5].Stop();
    }
}
