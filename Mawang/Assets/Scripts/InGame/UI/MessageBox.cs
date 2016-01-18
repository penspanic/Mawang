using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour
{
    [SerializeField]
    private Text[] messages;
    [SerializeField]
    private Image frame;
    Vector2[] messagesPos = { new Vector2(96, 28), new Vector2(96, -26.5f) };
    Vector2 middlePos = new Vector2(96, 0);
    Queue<string> messageQueue = new Queue<string>();
    GameObject[] children;
    bool isMessageBoxActive = false;
    float elaspedTime;
    float waitingTime;
    void Awake()
    {

        InactiveMessageBox();
    }

    void Update()
    {
        if (isMessageBoxActive)
        {
            elaspedTime += Time.deltaTime;
            if (elaspedTime >= waitingTime)
            {
                elaspedTime = 0;
                InactiveMessageBox();
            }

        }
    }

    public void PushMessage(string message)
    {
        messageQueue.Enqueue(message);
        if (messageQueue.Count == 3)
            messageQueue.Dequeue();
        ResetMessageBox();
        waitingTime += 2;
        if (waitingTime > 4)
            waitingTime = 4;
    }

    void ResetMessageBox()
    {
        ActiveMessageBox();
        messages[0].text = "";
        messages[1].text = "";
        if (messageQueue.Count == 1)
        {
            messages[0].transform.localPosition = middlePos;
            messages[0].text = messageQueue.ToArray()[0];
        }
        else
        {
            messages[0].transform.localPosition = messagesPos[0];
            messages[0].text = messageQueue.ToArray()[0];
            messages[1].transform.localPosition = messagesPos[1];
            messages[1].text = messageQueue.ToArray()[1];
        }
    }

    void ActiveMessageBox()
    {
        isMessageBoxActive = true;
        messages[0].enabled = true;
        messages[1].enabled = true;
        frame.enabled = true;
    }

    void InactiveMessageBox()
    {
        messageQueue.Clear();
        isMessageBoxActive = false;
        waitingTime = 0;
        messages[0].enabled = false;
        messages[1].enabled = false;
        frame.enabled = false;
    }
}
