using UnityEngine;
using UnityEngine.UI;

public class NotifyBar : MonoBehaviour
{
    public bool isShowing
    {
        get;
        private set;
    }

    private const float notifyShowTime = 2f;
    private Text notifyText;

    private void Awake()
    {
        notifyText = GetComponent<Text>();
        notifyText.enabled = false;
    }

    private float waitTime = 0f;
    private float elapsedTime = 0f;

    private void Update()
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