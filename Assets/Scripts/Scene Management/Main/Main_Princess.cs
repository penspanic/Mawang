using UnityEngine;
using UnityEngine.UI;

public class Main_Princess : MonoBehaviour
{
    private string[] scripts;
    private Text scriptText;

    private const float scriptDuration = 2f;

    private void Awake()
    {
        scriptText = transform.GetComponentInChildren<Text>();
    }

    public void SetScripts(string[] scripts)
    {
        this.scripts = scripts;
    }

    private bool isShowing;

    private void ShowScript()
    {
        isShowing = true;
        scriptText.text = scripts[Random.Range(0, scripts.Length)];
        Invoke("OnHideScript", scriptDuration);
    }

    private void OnHideScript()
    {
        isShowing = false;
        scriptText.text = string.Empty;
    }

    // Connected To Main Princess Prefab -> Event Trigger -> Pointer Down
    public void OnTouch()
    {
        if (!isShowing)
            ShowScript();
    }
}