using UnityEngine;
using UnityEngine.UI;

public class CastleUpgrade : MonoBehaviour
{
    public static readonly string[] upgradeDescriptions = new string[3]{
        "마왕성의 체력이 증가합니다!",
        "스킬의 공격력이 상승합니다!",
        "스킬의 쿨타임이 감소합니다!"
    };

    // hp , damage, cooltime 순

    // 기본 값
    public static readonly int[] originalValues = new int[3]{
        500, 100, 100
    };

    // 증가하는 업그레이드 수치
    public static readonly int[] upgradeIncreaseValues = new int[3]{
        100, 20, 5
    };

    public Text remainingPointText; // 남은 포인트 텍스트
    public Text upgradeDescriptionText;
    public Text allocateButtonText;
    public Button[] upgradeButtons;
    public Button allocateButton;
    private Image[,] gaugeImages;

    public bool isMoving;
    public bool isShowing;

    private Animator animator;
    private Main main;

    public int usableMaxPoint
    {
        get;
        private set;
    }

    public int usablePoint
    {
        get;
        private set;
    }

    public int[] allocatedPoints = new int[3];
    private int selectedIndex;

    private void Awake()
    {
        PlayerData.instance.CheckInstance();

        usableMaxPoint = PlayerData.instance.GetClearedStageCount();
        if (usableMaxPoint < 12) // World2에선 StageClear해도 포인트 안 줌
            usableMaxPoint = 12;
        usablePoint = usableMaxPoint;

        animator = GetComponent<Animator>();
        main = GameObject.FindObjectOfType<Main>();

        // 포인트 배분 로드하는 부분 구현

        gaugeImages = new Image[upgradeButtons.Length, 6];
        int alreadyAllocatedPoints = 0;
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            allocatedPoints[i] = PlayerData.instance.upgradePoint[upgradeButtons[i].name];
            alreadyAllocatedPoints += allocatedPoints[i];
            for (int j = 0; j < 6; j++)
            {
                gaugeImages[i, j] = upgradeButtons[i].transform.FindChild("Gauge" + (j + 1).ToString()).GetComponent<Image>();
            }
            SetGauge(i, allocatedPoints[i]);
        }
        usablePoint -= alreadyAllocatedPoints;
        SetAllocateButtonInteractive(false);

        remainingPointText.text = usablePoint.ToString();
    }

    private void SetGauge(int index, int value)
    {
        for (int i = 0; i < 6; i++)
        {
            gaugeImages[index, i].gameObject.SetActive(false);
        }
        for (int i = 0; i < value; i++)
        {
            gaugeImages[index, i].gameObject.SetActive(true);
        }
    }

    private void SetAllocateButtonInteractive(bool value)
    {
        if (value)
        {
            allocateButton.interactable = true;
            allocateButtonText.color = Color.black;
        }
        else
        {
            allocateButton.interactable = false;
            allocateButtonText.color = new Color(0f, 0f, 0f, 0.5f);
        }
    }

    private void SaveUpgrade()
    {
        PlayerData.instance.upgradePoint["Hp"] = allocatedPoints[0];
        PlayerData.instance.upgradePoint["Damage"] = allocatedPoints[1];
        PlayerData.instance.upgradePoint["Cool Time"] = allocatedPoints[2];
    }

    //Event
    public void OnInitButtonDown()
    {
        usablePoint = usableMaxPoint;

        remainingPointText.text = usablePoint.ToString();

        for (int i = 0; i < 3; i++)
        {
            allocatedPoints[i] = 0;
            SetGauge(i, 0);
        }
        if (usablePoint > 0)
            SetAllocateButtonInteractive(true);
    }

    public void OnUpgradeButtonDown(int index)
    {
        selectedIndex = index;
        upgradeDescriptionText.text = upgradeDescriptions[selectedIndex];
        if (allocatedPoints[selectedIndex] < 6 && usablePoint != 0)
            SetAllocateButtonInteractive(true);
        else
            SetAllocateButtonInteractive(false);
    }

    public void OnAllocateButtonDown()
    {
        allocatedPoints[selectedIndex]++;

        SetGauge(selectedIndex, allocatedPoints[selectedIndex]);
        remainingPointText.text = (--usablePoint).ToString();

        if (allocatedPoints[selectedIndex] >= 6 || usablePoint == 0)
            SetAllocateButtonInteractive(false);
    }

    public void OnDoneButtonDown()
    {
        if (!isMoving)
        {
            SaveUpgrade();
            animator.Play("Castle Upgrade Rise");
            isMoving = true;
            isShowing = false;
        }
    }

    public void ShowUpgrade()
    {
        if (isMoving)
            return;
        isShowing = true;
        isMoving = true;
        animator.Play("Castle Upgrade Fall");
    }

    public static int GetUpgradeIncreaseValue(string name)
    {
        if (name.Equals("Hp"))
            return upgradeIncreaseValues[0];
        else if (name.Equals("Damage"))
            return upgradeIncreaseValues[1];
        else if (name.Equals("Cool Time"))
            return upgradeIncreaseValues[2];
        throw new System.ArgumentException();
    }

    public static int GetOriginalValue(string name)
    {
        if (name.Equals("Hp"))
            return originalValues[0];
        else if (name.Equals("Damage"))
            return originalValues[1];
        else if (name.Equals("Cool Time"))
            return originalValues[2];
        throw new System.ArgumentException();
    }

    public static int GetUpgradeApplyedValue(string name)
    {
        PlayerData.instance.CheckInstance();
        if (name == "Cool Time")
            return GetOriginalValue(name) - PlayerData.instance.upgradePoint[name] * GetUpgradeIncreaseValue(name);
        return GetOriginalValue(name) + PlayerData.instance.upgradePoint[name] * GetUpgradeIncreaseValue(name);
    }

    public void OnMoveEnd()
    {
        isMoving = false;
    }

    public void OnRiseEnd()
    {
        isShowing = false;
    }
}