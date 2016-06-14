using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private BgmManager bgmMgr;
    private GameManager gameMgr;
    private Button pauseBtn;
    private GameObject pauseUI;

    private AudioSource[] sources;

    public bool isSceneChanging
    {
        get;
        private set;
    }

    private void Awake()
    {
        bgmMgr = GameObject.FindObjectOfType<BgmManager>();
        gameMgr = GameObject.FindObjectOfType<GameManager>();
        pauseBtn = GameObject.Find("PauseButton").GetComponent<Button>();
        pauseUI = transform.FindChild("PauseUI").gameObject;

        pauseBtn.onClick.AddListener(PauseButtonDown);
    }

    public void PauseButtonDown()
    {
        Time.timeScale = 0;
        sources = GameObject.FindObjectsOfType<AudioSource>();
        for (int i = 0; i < sources.Length; ++i)
        {
            sources[i].Pause();
        }

        pauseUI.SetActive(true);
    }

    // onCickEvent in Pause

    public void ResumeButtonDown()
    {
        Time.timeScale = gameMgr.userTimeScale;

        for (int i = 0; i < sources.Length; ++i)
        {
            sources[i].Play();
        }
        ButtonSound.PlaySound(ButtonSound.SoundType.BasicSound);

        sources = null;
        pauseUI.SetActive(false);
    }

    public void GoMainButtonDown()
    {
        if (isSceneChanging)
            return;

        // 메인화면으로 돌아가기
        isSceneChanging = true;
        Time.timeScale = 1;
        ButtonSound.PlaySound(ButtonSound.SoundType.BackSound);

        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, "Main"));
    }

    public void RetryButtonDown()
    {
        if (isSceneChanging)
            return;

        // 게임 재시작 하기
        isSceneChanging = true;
        Time.timeScale = 1;
        ButtonSound.PlaySound(ButtonSound.SoundType.BasicSound);

        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, PlayerData.instance.GetInGameSceneName()));
    }
}