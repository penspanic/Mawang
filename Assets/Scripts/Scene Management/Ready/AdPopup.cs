using UnityEngine;

public class AdPopup : MonoBehaviour
{
    private Ready ready;

    private void Awake()
    {
        ready = GameObject.FindObjectOfType<Ready>();
    }

    public void ShowPopup()
    {
        gameObject.SetActive(true);
    }

    public void OnYesButtonDown()
    {
        // 광고 구현

        //

        // 광고 다 봤을시 ->
        //PlayerData.instance.obsidian += 30;
        //ready.ResetObsidianText();

        gameObject.SetActive(false);
    }

    public void OnNoButtonDown()
    {
        gameObject.SetActive(false);
    }
}