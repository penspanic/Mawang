using UnityEngine;
using System.Collections;

public class Ready : MonoBehaviour
{
    Ready_UnitSelect unitSelect;
    void Awake()
    {
        StartCoroutine(FadeIn());
        PlayerData.instance.CheckInstance();

        unitSelect = GameObject.FindObjectOfType<Ready_UnitSelect>();
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerData.instance.obsidian += 100;
        }
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

        isChanging = true;
        ChangeScene("InGame");

    }

    void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneFader.Instance.FadeOut(1f, sceneName));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }

}