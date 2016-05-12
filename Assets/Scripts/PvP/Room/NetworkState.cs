using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkState : MonoBehaviour
{
    public Text stateText;

    void Awake()
    {
        SetText("방 찾는 중...");

        if(NetworkManager.instance.HasRoom()) // 생성된 방이 있을 때
        {
            SetText("방에 들어가는 중");
        }
    }

    void SetText(string text)
    {
        stateText.text = text;
    }
}
