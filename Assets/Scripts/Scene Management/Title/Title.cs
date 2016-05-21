using System.Collections;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject touchTheScreen;
    private InfoPanel panel;
    private bool isChanging = false;

    private void Awake()
    {
        panel = GameObject.FindObjectOfType<InfoPanel>();
        StartCoroutine(SceneFader.Instance.FadeIn(1f));

        StartCoroutine(Twinkle());
    }

    private IEnumerator Twinkle()
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

    private void TouchProcess()
    {
        if (isChanging)
            return;
        isChanging = true;
        StopCoroutine(Twinkle());
        touchTheScreen.SetActive(false);
        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, "Main"));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }

    public void OnScreenTouched()
    {
        if (panel.isShowing)
        {
            panel.ShowPanel();
            return;
        }
        TouchProcess();
    }

    public void OnInfoButtonDown()
    {
        panel.ShowPanel();
    }
}