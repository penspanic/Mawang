using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneFader : MonoBehaviour
{
    Image img;
    Sprite black;
    Sprite white;
    #region Singleton
    static SceneFader _instance;
    public static SceneFader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Scene Fader").AddComponent<SceneFader>();
                GameObject newObj   =   Instantiate(Resources.Load<GameObject>("Prefabs/UI/Fade"));
                newObj.transform.SetParent(GameObject.Find("Canvas").transform,false);
                _instance.img       =   newObj.GetComponent<Image>();
            }
            return _instance;
        }
    }
    #endregion

    void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
        black = Resources.Load<Sprite>("Sprite/Black");
        white = Resources.Load<Sprite>("Sprite/White");
    }

    public void FillColor(Color color)
    {
        img.sprite = white;
        img.color = color;
    }

    public IEnumerator FadeOut(float duration, string nextScene = null)
    {
        float fadeAlpha = 0;

        img.enabled     =   true;
        img.sprite      =   black;

        while (fadeAlpha != 1)
        {
            // 시작시간과 지나가는 시간의 차이 / 지속시간 
            fadeAlpha = Mathf.MoveTowards(fadeAlpha, 1, Time.unscaledDeltaTime * (1 / duration));
            img.color = new Color(0, 0, 0, fadeAlpha);
            yield return null;
        }

        fadeAlpha = 1;
        img.color = new Color(0, 0, 0, fadeAlpha);

        if (nextScene != null)
            Application.LoadLevel(nextScene);
    }

    public IEnumerator FadeIn(float duration, string nextScene = null)
    {
        float fadeAlpha = 1;
        
        img.enabled     =   true;
        img.sprite      =   black;

        while (fadeAlpha != 0)
        {
            // 시작시간과 지나가는 시간의 차이 / 지속시간 
            fadeAlpha = Mathf.MoveTowards(fadeAlpha, 0, Time.unscaledDeltaTime * (1 / duration));
            img.color = new Color(0, 0, 0, fadeAlpha);
            yield return null;
        }

        img.enabled = false;

        if (nextScene != null)
            Application.LoadLevel(nextScene);
    }

    public IEnumerator SoundFadeOut(float duration, AudioSource[] audioSources) // 점점 작아지게
    {
        float fadeVolume = 1;

        List<float> originalVolumeList = new List<float>();
        for (int i = 0; i < audioSources.Length; i++)
            originalVolumeList.Add(audioSources[i].volume);

        // 0.5f~ 0
        while (fadeVolume != 0)
        {
            fadeVolume = Mathf.MoveTowards(fadeVolume, 0, Time.unscaledDeltaTime * (1 / duration));
            for (int i = 0; i < audioSources.Length; i++)
            {
                try
                {
                    audioSources[i].volume = originalVolumeList[i] * fadeVolume;
                }
                catch (System.Exception)
                {
                    continue;
                }
            }
            yield return null;
        }
    }
}