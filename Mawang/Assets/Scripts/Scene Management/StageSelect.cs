using UnityEngine;
using System.Collections;

public class StageSelect : MonoBehaviour
{

    void Awake()
    {
        StartCoroutine(FadeIn());
        PlayerData.instance.CheckInstance();
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

    void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneFader.Instance.FadeOut(1f, sceneName));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }
}
