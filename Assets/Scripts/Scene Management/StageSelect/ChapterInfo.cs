﻿using UnityEngine;
using UnityEngine.UI;

public struct ChapterData
{
    public string chapterName;
    public string chapterDescription;
    public string skillName;
    public string skillDescription;
}

public class ChapterInfo : MonoBehaviour
{
    private StageSelect stageSelect;

    public bool isShowing;
    public Image princessIllust;
    public Text chapterNameText;
    public Text chapterDescriptionText;
    public Text skillNameText;
    public Text skillDescriptionText;

    private ChapterData selectedChapterData;

    private TipUI tipUI;

    private void Awake()
    {
        stageSelect = GameObject.FindObjectOfType<StageSelect>();
        tipUI = GameObject.FindObjectOfType<TipUI>();
        JsonManager.instance.CheckInstance();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowChapterInfo()
    {
        string chapterName = stageSelect.chapterName;
        if (isShowing)
        {
            return;
        }
        else
        {
            isShowing = true;
            gameObject.SetActive(true);
            tipUI.gameObject.SetActive(false);
            selectedChapterData = JsonManager.instance.GetChapterData(chapterName);

            princessIllust.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, chapterName + "_L");
            princessIllust.SetNativeSize();

            chapterNameText.text = selectedChapterData.chapterName;
            chapterDescriptionText.text = selectedChapterData.chapterDescription;
            skillNameText.text = selectedChapterData.skillName;
            skillDescriptionText.text = selectedChapterData.skillDescription;
        }
    }

    public void HideChapterInfo()
    {
        if (isShowing)
        {
            isShowing = false;
            gameObject.SetActive(false);
            tipUI.gameObject.SetActive(true);
        }
    }
}