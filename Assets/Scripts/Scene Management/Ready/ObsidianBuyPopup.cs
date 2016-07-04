using UnityEngine;

public class ObsidianBuyPopup : MonoBehaviour
{
    public void ShowPopup()
    {
        gameObject.SetActive(true);
    }

    public void OnYesButtonDown()
    {
        // 결제 구현

        // 결제 끝났을 시 ->
        //PlayerData.instance.obsidian += 100;
        //ready.ResetObsidianText();

        gameObject.SetActive(false);
    }

    public void OnNoButtonDown()
    {
        gameObject.SetActive(false);
    }
}