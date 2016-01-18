using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Main_Princess : MonoBehaviour
{
    string[] scripts;
    Text scriptText;

    const float scriptDuration = 2f;
    void Awake()
    {
        scriptText = transform.GetComponentInChildren<Text>();
    }

    public void SetScripts(string[] scripts)
    {
        Debug.Log(scripts);
        this.scripts = scripts;
    }


    bool isShowing;
    void ShowScript()
    {
        isShowing = true;
        scriptText.text = scripts[Random.Range(0, scripts.Length)];
        Invoke("OnHideScript", scriptDuration);
    }

    void OnHideScript()
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