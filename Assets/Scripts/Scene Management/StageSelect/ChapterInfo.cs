using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct ChapterData
{
    public string chapterName;
    public string chapterDescription;
    public string skillName;
    public string skillDescription;
}
public class ChapterInfo : MonoBehaviour
{
    StageSelect stageSelect;

    public bool isShowing;
    public Image princessIllust;
    public Text chapterNameText;
    public Text chapterDescriptionText;
    public Text skillNameText;
    public Text skillDescriptionText;

    ChapterData selectedChapterData;
    void Awake()
    {
        stageSelect = GameObject.FindObjectOfType<StageSelect>();
        JsonManager.instance.CheckInstance();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowChapterInfo()
    {
        string chapterName = stageSelect.chapterName;
        if(isShowing)
        {
            return;
        }
        else
        {
            isShowing = true;
            gameObject.SetActive(true);
            selectedChapterData = JsonManager.instance.GetChapterData(chapterName);

            princessIllust.sprite = SpriteManager.Instance.GetSprite(PackingType.Princess, chapterName + "_L");
            princessIllust.SetNativeSize();

            chapterNameText.text = selectedChapterData.chapterName;
            chapterDescriptionText.text = selectedChapterData.chapterDescription;
            skillNameText.text = selectedChapterData.skillName;
            skillDescriptionText.text = selectedChapterData.skillDescription;

        }
    }
    public void HideChapterInfo()
    {
        if(isShowing)
        {
            isShowing = false;
            gameObject.SetActive(false);
        }
    }
}