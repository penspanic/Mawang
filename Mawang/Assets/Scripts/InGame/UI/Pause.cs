using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour
{
    private BgmManager  bgmMgr;
    private Button      pauseBtn;
    private GameObject  pauseUI;


    private AudioSource[] sources;

    public bool isSceneChanging
    {
        get;
        private set;
    }
    void Awake()
    {
        bgmMgr      =   GameObject.FindGameObjectWithTag("Manager").GetComponent<BgmManager>();
        pauseBtn    =   GameObject.Find("PauseButton").GetComponent<Button>();
        pauseUI     =   transform.FindChild("PauseUI").gameObject;

        pauseBtn.onClick.AddListener(PauseButtonDown);
    }

    void PauseButtonDown()
    {
        Time.timeScale = 0;
        sources = GameObject.FindObjectsOfType<AudioSource>();
        for (int i = 0; i < sources.Length;i++)
        {
            sources[i].Pause();
        }

        pauseUI.SetActive(true);
    }

    // onCickEvent in Pause

    public void ResumeButtonDown()
    {
        Time.timeScale = 1;
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].Play();
        }
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
        
        StartCoroutine(SceneFader.Instance.FadeOut(1f, "Main"));

    }
    public void RetryButtonDown()
    {
        if (isSceneChanging)
            return;

        // 게임 재시작 하기
        isSceneChanging = true;
        Time.timeScale = 1;

        StartCoroutine(SceneFader.Instance.FadeOut(1f, "InGame"));
    }
}

