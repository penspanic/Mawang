using UnityEngine;
using System.Collections;

public class Ready : MonoBehaviour
{
    Ready_UnitSelect unitSelect;
    NotifyBar notifyBar;
    void Awake()
    {
        StartCoroutine(FadeIn());
        PlayerData.instance.CheckInstance();

        unitSelect = GameObject.FindObjectOfType<Ready_UnitSelect>();
        notifyBar = FindObjectOfType<NotifyBar>();
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
        isChanging = true;

        ChangeScene("StageSelect");
        ButtonSound.PlaySound(ButtonSound.SoundType.BackSound);
    }

    public void OnStartButtonDown()
    {
        if (isChanging)
            return;

        if (unitSelect.OnGameStart())
        {
            isChanging = true;
            ChangeScene("InGame");
        }
        else
            notifyBar.ShowMessage("유닛을 한 종류 이상 선택하세요");

    }

    void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneFader.Instance.FadeOut(1f, sceneName));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }

}