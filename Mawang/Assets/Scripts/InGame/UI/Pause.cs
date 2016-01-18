using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour
{
    private BgmManager  bgmMgr;
    private Button      pauseBtn;
    private GameObject  pauseUI;
    void Awake()
    {
        bgmMgr      =   GameObject.FindGameObjectWithTag("Manager").GetComponent<BgmManager>();
        pauseBtn    =   GameObject.Find("PauseButton").GetComponent<Button>();
        pauseUI     =   transform.FindChild("PauseUI").gameObject;

        pauseBtn.onClick.AddListener(PauseButtonDown);
    }

    void PauseButtonDown()
    {
        Time.timeScale  =   0;
        bgmMgr.Pause();

        pauseUI.SetActive(true);
    }

    // onCickEvent in Pause

    public void ResumeButtonDown()
    {
        Time.timeScale  =   1;
        bgmMgr.Resume();
        pauseUI.SetActive(false);
    }
    public void GoMainButtonDown()
    {
        Time.timeScale = 1;
        // 메인화면으로 돌아가기

    }
    public void RetryButtonDown()
    {
        Time.timeScale = 1;
        // 게임 재시작 하기.
        Application.LoadLevel("InGame");
    }
}

