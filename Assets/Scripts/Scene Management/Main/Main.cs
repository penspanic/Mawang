using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Main : MonoBehaviour
{

    public Button upgradeButton;
    public Button infoButton;

    public Main_Book book;

    public GameObject explainUpgradePrefab;
    public GameObject explainPrincessPrefab;

    CastleUpgrade upgrade;
    CastleInfo info;

    //BlurControl blurCtrl;

    GameEventReceiver stageClearEventReceiver;
    GameEventReceiver chapterClearEventReceiver;

    void Awake()
    {

        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        upgrade = GameObject.FindObjectOfType<CastleUpgrade>();
        info = GameObject.FindObjectOfType<CastleInfo>();

        stageClearEventReceiver = new GameEventReceiver(GameEvent.FirstC0S1Cleared, OnFirstC0S1Cleared);
        chapterClearEventReceiver = new GameEventReceiver(GameEvent.FirstChapter0Cleared, OnFirstChapter0Cleared);

        Invoke("CheckEvents", 1f);

        //blurCtrl = GameObject.FindObjectOfType<BlurControl>();
    }

    void CheckEvents()
    {
        stageClearEventReceiver.CheckEvent();
        chapterClearEventReceiver.CheckEvent();
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

    void OnFirstC0S1Cleared() // 마왕성 업그레이드 알려주기
    {
        Debug.Log("First C0S1 Cleared!");
        StartCoroutine(ExplainCastleUpgrade());
    }

    void OnFirstChapter0Cleared() // 공주 납치한 사실 알려주기
    {
        Debug.Log("First Chapter0 Cleared!");
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

            //blurCtrl.SetValue(blurValue);

            yield return null;
        }
        //blurCtrl.SetValue(isBlear ? maxBlur : 0);
    }
    
    IEnumerator ExplainCastleUpgrade()
    {
        Image upgradeExplainPrefab = Instantiate(explainUpgradePrefab).GetComponent<Image>();
        upgradeExplainPrefab.transform.SetParent(GameObject.Find("Canvas").transform, false);

        yield return new WaitForSeconds(1f);
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        Destroy(upgradeExplainPrefab);
    }

    IEnumerator ExplainPrincess()
    {
        Image princessExplainImage = Instantiate(explainPrincessPrefab).GetComponent<Image>();
        princessExplainImage.transform.SetParent(GameObject.Find("Canvas").transform, false);

        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        Destroy(princessExplainImage);
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