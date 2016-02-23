using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    public GameObject touchTheScreen;
    InfoPanel panel;
    bool isChanging = false;
    void Awake()
    {
        panel = GameObject.FindObjectOfType<InfoPanel>();
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        
        StartCoroutine(Twinkle());

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
        if (isChanging)
            return;
        isChanging = true;
        StopCoroutine(Twinkle());
        touchTheScreen.SetActive(false);
        StartCoroutine(SceneFader.Instance.FadeOut(1f, "Main"));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }

    public void OnScreenTouched()
    {
        if (panel.isShowing)
            return;
        TouchProcess();
    }

    public void OnInfoButtonDown()
    {
        panel.ShowPanel();
    }
}