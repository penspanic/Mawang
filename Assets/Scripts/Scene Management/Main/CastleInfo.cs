using UnityEngine;
using UnityEngine.UI;

public class CastleInfo : MonoBehaviour
{
    public Button infoButton;
    public Button closeButton;

    public Text hpText;
    public Text skillText;
    public Text coolTimeText;

    public Image[] castleIcons;

    private Animator animator;
    private CastleUpgrade upgrade;
    private Main main;
    public bool isShowing;
    public bool isMoving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        upgrade = GameObject.FindObjectOfType<CastleUpgrade>();
        main = GameObject.FindObjectOfType<Main>();
    }

    private void ResetInfo()
    {
        hpText.text = "마왕성 체력 : 500 + " + (upgrade.allocatedPoints[0] * CastleUpgrade.upgradeIncreaseValues[0]).ToString();
        skillText.text = "마왕성 스킬 공격력 : 100 + " + (upgrade.allocatedPoints[1] * CastleUpgrade.upgradeIncreaseValues[1]).ToString();
        coolTimeText.text = "마왕성 스킬 쿨타임 : 130 - " + (upgrade.allocatedPoints[2] * CastleUpgrade.upgradeIncreaseValues[2]).ToString();
        int upgradeValue = upgrade.usableMaxPoint - upgrade.usablePoint;

        int castleIndex = upgradeValue / 5;
        if (castleIndex > 2)
            castleIndex = 2;
        for (int i = 0; i < 3; i++)
        {
            castleIcons[i].color = new Color(0.3f, 0.3f, 0.3f, 1f);
        }
        castleIcons[castleIndex].color = new Color(1, 1, 1, 1);
    }

    //Event
    public void OnCloseButtonDown()
    {
        if (!isMoving)
        {
            animator.Play("Castle Info Rise");
            isMoving = true;
        }
    }

    public void ShowInfo()
    {
        if (isMoving)
            return;
        isShowing = true;
        isMoving = true;
        animator.Play("Castle Info Fall");
        ResetInfo();
    }

    public void OnMoveEnd()
    {
        isMoving = false;
    }

    public void OnRiseEnd()
    {
        isShowing = false;
    }

    public static int GetCastleLevel()
    {
        int upgradePoint = PlayerData.instance.upgradePoint["Hp"] + PlayerData.instance.upgradePoint["Damage"] +
            PlayerData.instance.upgradePoint["Cool Time"];
        return (upgradePoint / 5) + 1;
    }
}