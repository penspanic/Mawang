using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private CastleUpgrade upgrade;
    private CastleInfo info;

    private GameEventReceiver stageClearEventReceiver;
    private GameEventReceiver chapter0ClearEventReceiver;
    private GameEventReceiver appRatingEventReceiver;

    private bool eventHandling = false;

    private bool isChanging = false;

    private void Awake()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeProcess();
        }
    }

    private void EscapeProcess()
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

    private void CheckEvents()
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

    private void OnFirstC0S1Cleared() // 마왕성 업그레이드 알려주기
    {
        StartCoroutine(ExplainCastleUpgrade());
    }

    private void OnFirstChapter0Cleared() // 공주 납치한 사실 알려주기
    {
        StartCoroutine(ExplainPrincess());
    }

    private void ShowAppRatingPopup()
    {
        if (PlayerData.instance.appRated)
            return;
        StartCoroutine(ShowAppRatingProcess());
    }

    private IEnumerator ExplainCastleUpgrade()
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

    private IEnumerator ExplainPrincess()
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

    private IEnumerator ShowAppRatingProcess()
    {
        while (true)
        {
            if (eventHandling)
                yield return null;
            else
                break;
        }
        appRatingPopup.ShowPopup();
    }

    private void GameStart()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, "StageSelect"));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }
}