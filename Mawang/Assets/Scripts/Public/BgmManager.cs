using UnityEngine;
using System.Collections;



public class BgmManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;
    private AudioSource audioSorce;

    void Awake()
    {
        audioSorce   =   GetComponent<AudioSource>();
        StartBGM();
    }


    public void StartBGM()
    {
        audioSorce.clip  =   bgm;
        audioSorce.Play();
    }

    public void Pause()
    {
        audioSorce.Pause();
    }

    public void Resume()
    {
        audioSorce.UnPause();
    }
    
}
