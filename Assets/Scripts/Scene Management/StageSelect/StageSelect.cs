using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public string chapterName;

    private ChapterInfo chapterInfo;

    private Button[][] stageButtons = new Button[4][];

    private void Awake()
    {
        StartCoroutine(FadeIn());
        PlayerData.instance.CheckInstance();

        chapterInfo = GameObject.FindObjectOfType<ChapterInfo>();

        for (int c = 0; c < 4; c++)
        {
            List<Button> stageButtonList = new List<Button>();
            for (int s = 1; s <= 3; s++)
            {
                stageButtonList.Add(GameObject.Find("C" + c.ToString() + "S" + s.ToString()).GetComponent<Button>());
            }
            stageButtons[c] = stageButtonList.ToArray();
        }

        ButtonSet();
        GameObject.FindObjectOfType<SceneFader>().transform.SetParent(Camera.main.transform, true);
        GameObject.FindObjectOfType<SceneFader>().transform.localPosition = new Vector3(0, 0, 10);
    }

    private IEnumerator FadeIn()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        yield return new WaitForSeconds(1f);
        isChanging = false;
    }

    private void ButtonSet()
    {
        foreach (Button[] buttons in stageButtons)
        {
            foreach (Button eachButton in buttons)
                eachButton.interactable = false;
        }

        if (PlayerData.instance.lastClearedStage == null || PlayerData.instance.lastClearedStage == "") // 클리어 한게 아무것도 없을 경우
        {
            stageButtons[0][0].interactable = true;
            return;
        }

        string lastClearedStage = PlayerData.instance.lastClearedStage;

        string lastOpenStage = PlayerData.GetNextStageName(lastClearedStage);

        int chapter = int.Parse(lastOpenStage[1].ToString());
        int stage = int.Parse(lastOpenStage[3].ToString());

        for (int c = 0; c <= chapter; c++)
        {
            for (int s = 0; s < 3; s++)
            {
                if (c < chapter)
                    stageButtons[c][s].interactable = true;
                else
                {
                    if (s < stage)
                        stageButtons[c][s].interactable = true;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EscapeProcess();
    }

    private bool isChanging = false;

    public void EscapeProcess()
    {
        if (isChanging)
            return;
        if (chapterInfo.isShowing)
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

    private void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, sceneName));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }

    public void OnInfoCloseButtonDown()
    {
        EscapeProcess();
    }
}