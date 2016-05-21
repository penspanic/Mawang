using UnityEngine;
using UnityEngine.UI;

public class NetworkState : MonoBehaviour
{
    public Text stateText;

    private void Awake()
    {
        SetText("방 찾는 중...");

        if (NetworkManager.instance.HasRoom()) // 생성된 방이 있을 때
        {
            SetText("방에 들어가는 중");
        }
    }

    private void SetText(string text)
    {
        stateText.text = text;
    }
}