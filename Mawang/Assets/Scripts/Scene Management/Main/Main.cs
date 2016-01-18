using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Main : MonoBehaviour
{

    public Button upgradeButton;
    public Button infoButton;
    public Image blurImage;

    CastleUpgrade upgrade;
    CastleInfo info;

    void Awake()
    {
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        upgrade = GameObject.FindObjectOfType<CastleUpgrade>();
        info = GameObject.FindObjectOfType<CastleInfo>();
    }



    public void OnBookButtonDown()
    {
        if (!isChanging)
            ToBookScene();
    }

    public void OnStartButtonDown()
    {
        if (!isChanging)
            GameStart();
    }

    public void OnCastleUpgradeButtonDown()
    {
        if (!upgrade.isShowing && !info.isShowing)
            upgrade.ShowUpgrade();
    }

    public void OnCastleInfoButtonDown()
    {
        if (!info.isShowing && !upgrade.isShowing)
            info.ShowInfo();
    }

    public void BlurBackground(bool isBlear)
    {
        StartCoroutine(BlurProcess(isBlear));
    }

    IEnumerator BlurProcess(bool isBlear)
    {
        float blurAlpha = isBlear ? 0 : 1;
        
        if(isBlear)
        {
            while(blurAlpha != 1)
            {
                blurAlpha = Mathf.MoveTowards(blurAlpha, 1, Time.deltaTime);
                blurImage.color = new Color(1, 1, 1, blurAlpha);
                yield return null;
            }
            blurImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            while (blurAlpha != 0)
            {
                blurAlpha = Mathf.MoveTowards(blurAlpha, 0, Time.deltaTime);
                blurImage.color = new Color(1, 1, 1, blurAlpha);
                yield return null;
            }
            blurImage.color = new Color(1, 1, 1, 0);
        }
    }
    bool isChanging = false;
    void ToBookScene()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeOut(1f, "Book"));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }

    void GameStart()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeOut(1f, "StageSelect"));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }
}