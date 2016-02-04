using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SceneFader : MonoBehaviour
{

    SpriteRenderer sprRenderer;
    BoxCollider2D collider;
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
                _instance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Fade")).AddComponent<SceneFader>();
                _instance.sprRenderer = _instance.GetComponent<SpriteRenderer>();
                _instance.collider = _instance.GetComponent<BoxCollider2D>();
                _instance.collider.enabled = false;
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
        sprRenderer.sprite = white;
        sprRenderer.color = color;
    }

    public IEnumerator FadeOut(float duration, string nextScene = null)
    {
        float fadeAlpha = 0;

        sprRenderer.enabled     =   true;
        sprRenderer.sprite      =   black;
        collider.enabled        =   true;
        while (fadeAlpha != 1)
        {
            // 시작시간과 지나가는 시간의 차이 / 지속시간 
            fadeAlpha = Mathf.MoveTowards(fadeAlpha, 1, Time.unscaledDeltaTime * (1 / duration));
            sprRenderer.color = new Color(0, 0, 0, fadeAlpha);
            yield return null;
        }

        fadeAlpha = 1;
        sprRenderer.color = new Color(0, 0, 0, fadeAlpha);
        collider.enabled = true;
        if (nextScene != null)
            Application.LoadLevel(nextScene);
    }

    public IEnumerator FadeIn(float duration, string nextScene = null)
    {
        float fadeAlpha = 1;
        
        sprRenderer.enabled     =   true;
        sprRenderer.sprite      =   black;
        collider.enabled        =   true;

        while (fadeAlpha != 0)
        {
            // 시작시간과 지나가는 시간의 차이 / 지속시간 
            fadeAlpha = Mathf.MoveTowards(fadeAlpha, 0, Time.unscaledDeltaTime * (1 / duration));
            sprRenderer.color = new Color(0, 0, 0, fadeAlpha);
            yield return null;
        }

        sprRenderer.enabled = false;
        collider.enabled = false;
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