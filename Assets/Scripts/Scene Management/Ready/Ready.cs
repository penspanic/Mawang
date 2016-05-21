using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ready : MonoBehaviour
{
    private Text obsidianText;

    private Ready_UnitSelect unitSelect;

    private void Awake()
    {
        StartCoroutine(FadeIn());
        PlayerData.instance.CheckInstance();
        unitSelect = GameObject.FindObjectOfType<Ready_UnitSelect>();

        obsidianText = GameObject.Find("Obsidian").GetComponentInChildren<Text>();

        ResetObsidianText();
    }

    private IEnumerator FadeIn()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeIn(0.6f));
        yield return new WaitForSeconds(1f);
        isChanging = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EscapeProcess();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerData.instance.obsidian += 100;
            ResetObsidianText();
        }
    }

    private bool isChanging = false;

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
        ChangeScene(PlayerData.instance.GetInGameSceneName());
    }

    private void ChangeScene(string sceneName)
    {
        PlayerData.instance.selectedUnitList = new List<string>(unitSelect.GetSelectedUnit());
        PlayerData.instance.selectedUnitList.Sort((a, b) =>
        {
            Movable aUnit = Resources.Load<Movable>("Prefabs/OurForce/" + a);
            Movable bUnit = Resources.Load<Movable>("Prefabs/OurForce/" + b);
            return Comparer<Movable>.Default.Compare(aUnit, bUnit);
        });

        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, sceneName));
        StartCoroutine(SceneFader.Instance.SoundFadeOut(1f, GameObject.FindObjectsOfType<AudioSource>()));
    }
}