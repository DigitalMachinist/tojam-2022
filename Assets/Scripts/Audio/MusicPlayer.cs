using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource phase1Music;
    public AudioSource phase2Music;
    public AudioSource phase3Music;
    public AudioSource phase3Music_loopable;

    private int currentPhase;
    
    void Start()
    {
        currentPhase = GameManager.Get().CurrentPhase;
        GameManager.Get().PhaseChanged += OnPhaseChanged;
        phase1Music.Play();
    }

    void OnPhaseChanged(int number)
    {
        if (number == currentPhase)
        {
            return;
        }
        
        if ( number == 2 )
        {
            PlayPhase2();
        }
        else if ( number > 2 )
        {
            PlayPhase3();
        }
        else if ( number == 1 )
        {
            RestartMusic();
        }

        currentPhase = number;
    }

    public void PlayPhase2()
    {
        StopAllCoroutines();

        phase1Music.Stop();
        phase3Music.Stop();
        phase3Music_loopable.Stop();

        phase2Music.Play();
    }

    public void PlayPhase3()
    {
        StopAllCoroutines();

        phase1Music.Stop();
        phase2Music.Stop();
        phase3Music_loopable.Stop();

        phase3Music.Play();

        StartCoroutine( CoPlay() ); 
    }

    public void RestartMusic()
    {
        StopAllCoroutines();

        phase2Music.Stop();
        phase3Music.Stop();
        phase3Music_loopable.Stop();

        phase1Music.Play();
    }

    IEnumerator CoPlay()
    {
        while( phase3Music.isPlaying )
        {
            yield return null;
        }
        phase3Music_loopable.Play();
    }
}
