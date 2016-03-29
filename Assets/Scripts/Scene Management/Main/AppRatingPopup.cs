using UnityEngine;
using System.Collections;

public class AppRatingPopup : MonoBehaviour
{
    void Awake()
    {

    }   

    public void AcceptButtonDown()
    {
        PlayerData.instance.appRated = true;
        Application.OpenURL("market://search?q=pname:com.nerdgroup.WarControlPro");
        gameObject.SetActive(false);
    }

    public void CancelButtonDown()
    {
        gameObject.SetActive(false);
    }
}