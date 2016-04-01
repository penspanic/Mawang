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

    public Sprite castleInfoExplainSprite;

    public GameObject gameQuit;
    public AppRatingPopup appRatingPopup;

    CastleUpgrade upgrade;
    CastleInfo info;

    GameEventReceiver stageClearEventReceiver;
    GameEventReceiver chapter0ClearEventReceiver;
    GameEventReceiver appRatingEventReceiver;

    bool eventHandling = false;

    bool isChanging = false;
    void Awake()
    {
        PlayerData.instance.CheckInstance();

        Time.timeScale = 1f;

        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        upgrade = GameObject.FindObjectOfType<CastleUpgrade>();
        info = GameObject.FindObjectOfType<CastleInfo>();

        stageClearEventReceiver = new GameEventReceiver(GameEvent.FirstC0S1Cleared, OnFirstC0S1Cleared);
        chapter0ClearEventReceiver = new GameEventReceiver(GameEvent.FirstChapter0Cleared, OnFirstChapter0Cleared);
        appRatingEventReceiver = new GameEventReceiver(GameEvent.AppRating, ShowAppRatingPopup);

        Invoke("CheckEvents", 1f);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeProcess();
        }
    }

    void EscapeProcess()
    {
        if (isChanging)
            return;
        gameQuit.SetActive(true);

    }

    public void GameQuitYes()
    {
        Application.Quit();
    }

    public void GameQuitNo()
    {
        gameQuit.SetActive(false);
    }

    void CheckEvents()
    {
        stageClearEventReceiver.CheckEvent();
        chapter0ClearEventReceiver.CheckEvent();
        appRatingEventReceiver.CheckEvent();
    }

    public void OnStartButtonDown()
    {
        if (!isChanging && !gameQuit.activeSelf)
            GameStart();
    }

    public void OnCastleUpgradeButtonDown()
    {
        if (!upgrade.isShowing && !info.isShowing && !gameQuit.activeSelf)
            upgrade.ShowUpgrade();
    }

    public void OnCastleInfoButtonDown()
    {
        if (!info.isShowing && !upgrade.isShowing && !gameQuit.activeSelf)
            info.ShowInfo();
    }

    public void OnBookButtonDown()
    {
        if (gameQuit.activeSelf)
            return;

        book.gameObject.SetActive(true);
    }

    void OnFirstC0S1Cleared() // 마왕성 업그레이드 알려주기
    {
        StartCoroutine(ExplainCastleUpgrade());
    }

    void OnFirstChapter0Cleared() // 공주 납치한 사실 알려주기
    {
        StartCoroutine(ExplainPrincess());
    }

    void ShowAppRatingPopup()
    {
        if (PlayerData.instance.appRated)
            return;
        StartCoroutine(ShowAppRatingProcess());
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
        
        Image upgradeExplainImage = Instantiate(explainUpgradePrefab).GetComponent<Image>();
        upgradeExplainImage.transform.SetParent(GameObject.Find("Canvas").transform, false);
        
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        upgradeExplainImage.sprite = castleInfoExplainSprite;
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Destroy(upgradeExplainImage);
    }

    IEnumerator ExplainPrincess()
    {
        eventHandling = true;
        Image princessExplainImage = Instantiate(explainPrincessPrefab).GetComponent<Image>();
        princessExplainImage.transform.SetParent(GameObject.Find("Canvas").transform, false);

        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        Destroy(princessExplainImage);
        eventHandling = false;
    }

    IEnumerator ShowAppRatingProcess()
    {
        while(true)
        {
            if (eventHandling)
                yield return null;
            else
                break;
        }
        appRatingPopup.ShowPopup();
    }

    void GameStart()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, "StageSelect"));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }
}
