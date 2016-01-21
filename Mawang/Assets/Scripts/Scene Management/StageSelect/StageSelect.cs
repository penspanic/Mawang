using UnityEngine;
using System.Collections;

public class StageSelect : MonoBehaviour
{
    public string chapterName;

    ChapterInfo chapterInfo;
    void Awake()
    {
        StartCoroutine(FadeIn());
        PlayerData.instance.CheckInstance();

        chapterInfo = GameObject.FindObjectOfType<ChapterInfo>();
    }

    IEnumerator FadeIn()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        yield return new WaitForSeconds(1f);
        isChanging = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EscapeProcess();
    }

    bool isChanging = false;

    public void EscapeProcess()
    {
        if (isChanging)
            return;
        if(chapterInfo.isShowing)
        {
            chapterInfo.HideChapterInfo();
            return;
        }
        isChanging = true;
        ChangeScene("Main");
        ButtonSound.PlaySound(ButtonSound.SoundType.BackSound);
    }

    public void OnStageButtonDown(string stageName)
    {
        if (isChanging)
            return;
        isChanging = true;
        PlayerData.instance.selectedStage = stageName;

        ChangeScene("Ready");
    }

    public void ChapterButtonDown(string chapterName)
    {
        this.chapterName = chapterName;
        chapterInfo.ShowChapterInfo();
        
    }

    void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneFader.Instance.FadeOut(1f, sceneName));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }
}
