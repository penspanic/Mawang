using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    public GameObject touchTheScreen;

    void Awake()
    {
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        
        StartCoroutine(Twinkle());

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TouchProcess();
        }
    }

    IEnumerator Twinkle()
    {
        bool isTurned = true;
        while (true)
        {
            yield return new WaitForSeconds(1.2f);

            if (isTurned)
                touchTheScreen.SetActive(false);
            else
                touchTheScreen.SetActive(true);
            isTurned = !isTurned;
        }
    }

    void TouchProcess()
    {
        StopCoroutine(Twinkle());
        touchTheScreen.SetActive(false);
        StartCoroutine(SceneFader.Instance.FadeOut(1f, "Main"));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }
}