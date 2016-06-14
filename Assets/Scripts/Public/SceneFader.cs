using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    private SpriteRenderer sprRenderer;
    private new BoxCollider2D collider;
    private Sprite black;
    private Sprite white;

    #region Singleton

    private static SceneFader _instance;

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

    #endregion Singleton

    private void Awake()
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
        float elapsedTime = 0f;

        sprRenderer.enabled = true;
        sprRenderer.sprite = black;
        collider.enabled = true;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.MoveTowards(0, 1, elapsedTime / duration);
            sprRenderer.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        sprRenderer.color = new Color(0, 0, 0, 1);
        collider.enabled = true;
        if (nextScene != null)
            SceneManager.LoadScene(nextScene);
    }

    public IEnumerator FadeIn(float duration, string nextScene = null)
    {
        float elapsedTime = 0f;

        sprRenderer.enabled = true;
        sprRenderer.sprite = black;
        collider.enabled = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = 1f - Mathf.MoveTowards(0, 1, elapsedTime / duration);
            sprRenderer.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        sprRenderer.color = new Color(0, 0, 0, 0);
        sprRenderer.enabled = false;
        collider.enabled = false;
        if (nextScene != null)
            SceneManager.LoadScene(nextScene);
    }

    public IEnumerator SoundFadeOut(float duration, AudioSource[] audioSources) // 점점 작아지게
    {
        float fadeVolume = 1;

        List<float> originalVolumeList = new List<float>();
        for (int i = 0; i < audioSources.Length; ++i)
            originalVolumeList.Add(audioSources[i].volume);

        // 0.5f~ 0
        while (fadeVolume != 0)
        {
            fadeVolume = Mathf.MoveTowards(fadeVolume, 0, Time.unscaledDeltaTime * (1 / duration));
            for (int i = 0; i < audioSources.Length; ++i)
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