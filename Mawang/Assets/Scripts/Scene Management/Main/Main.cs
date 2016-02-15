using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Main : MonoBehaviour
{

    public Button upgradeButton;
    public Button infoButton;

    public Main_Book book;
    CastleUpgrade upgrade;
    CastleInfo info;

    BlurControl blurCtrl;

    void Awake()
    {
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        upgrade = GameObject.FindObjectOfType<CastleUpgrade>();
        info = GameObject.FindObjectOfType<CastleInfo>();
        blurCtrl = GameObject.FindObjectOfType<BlurControl>();
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
    public void OnBookButtonDown()
    {
        book.gameObject.SetActive(true);
    }

    public void BlurBackground(bool isBlear)
    {
        StartCoroutine(BlurProcess(isBlear));
    }

    IEnumerator BlurProcess(bool isBlear)
    {
        float elapsedTime = 0f;
        const float blurTime = 1f;
        const float maxBlur = 3f;
       
        while (elapsedTime < blurTime)
        {
            elapsedTime += Time.deltaTime;
            float blurValue = isBlear ? EasingUtil.easeInOutQuint(0, maxBlur, elapsedTime / blurTime) : EasingUtil.easeInOutQuint(maxBlur, 0, elapsedTime / blurTime);

            blurCtrl.SetValue(blurValue);

            yield return null;
        }
        blurCtrl.SetValue(isBlear ? maxBlur : 0);
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