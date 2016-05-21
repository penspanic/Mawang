using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgm;

    private AudioSource audioSorce;

    private void Awake()
    {
        audioSorce = GetComponent<AudioSource>();
        StartBGM();
    }

    public void StartBGM()
    {
        audioSorce.clip = bgm;
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