using UnityEngine;

public class AppRatingPopup : MonoBehaviour
{
    private void Awake()
    {
        PlayerData.instance.CheckInstance();
    }

    public void ShowPopup()
    {
        gameObject.SetActive(true);
    }

    public void AcceptButtonDown()
    {
        PlayerData.instance.appRated = true;
        Application.OpenURL("market://search?q=pname:com.advergamekorea.mawang");
        gameObject.SetActive(false);
    }

    public void CancelButtonDown()
    {
        gameObject.SetActive(false);
    }
}