using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotifyBar : MonoBehaviour
{
    public bool isShowing
    {
        get;
        private set;
    }

    const float notifyShowTime = 2f;
    Text notifyText;

    void Awake()
    {
        notifyText = GetComponent<Text>();
        notifyText.enabled = false;
    }

    float waitTime = 0f;
    float elapsedTime = 0f;
    void Update()
    {
        if (isShowing)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > waitTime)
            {
                isShowing = false;
                notifyText.enabled = false;
            }
        }
    }

    public void ShowMessage(string message)
    {
        isShowing = true;
        notifyText.enabled = true;
        notifyText.text = message;

        elapsedTime = 0f;
        waitTime = 2f;
    }
}
