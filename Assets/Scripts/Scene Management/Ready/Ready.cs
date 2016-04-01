using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ready : MonoBehaviour
{

    Text obsidianText;

    void Awake()
    {
        StartCoroutine(FadeIn());
        PlayerData.instance.CheckInstance();

        obsidianText = GameObject.Find("Obsidian").GetComponentInChildren<Text>();

        ResetObsidianText();
    }

    IEnumerator FadeIn()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeIn(0.6f));
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
            ResetObsidianText();
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

    public void ResetObsidianText()
    {
        obsidianText.text = PlayerData.instance.obsidian.ToString();
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
        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, sceneName));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }

}