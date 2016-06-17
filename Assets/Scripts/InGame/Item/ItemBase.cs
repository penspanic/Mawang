using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ItemBase : MonoBehaviour
{
    public Image coolTimeImage;

    private int _amount;

    public string message
    {
        get;
        protected set;
    }

    public bool isUsing
    {
        get;
        protected set;
    }

    public int amount
    {
        get
        {
            return _amount;
        }
        protected set
        {
            _amount = value;
            SetAmountText();
        }
    }

    public int coolTime
    {
        get;
        protected set;
    }

    protected MessageBox msgBox;
    protected Text amountText;


    protected virtual void Awake()
    {
        msgBox = GameObject.FindObjectOfType<MessageBox>();
        PlayerData.instance.CheckInstance();

        this.name = this.name.Remove(this.name.IndexOf("(")); // (Clone) 문자열 삭제
        amount = PlayerData.instance.itemStorage[name];
    }

    protected abstract void Useitem();

    private void SetAmountText()
    {
        if (amountText == null)
            amountText = GetComponentInChildren<Text>();
        amountText.text = _amount.ToString();
    }

    public void TryUseItem()
    {
        if (amount > 0)
        {
            Useitem();
        }
        else
        {
            msgBox.PushMessage("아이템이 없습니다!");
        }
    }

    protected IEnumerator CoolTimeProcess()
    {
        float currTime = 0.0f;

        while (currTime < coolTime)
        {
            currTime += Time.deltaTime;

            coolTimeImage.fillAmount = currTime / coolTime;
            yield return null;
        }

        isUsing = false;
    }
}