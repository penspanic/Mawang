using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemBase : MonoBehaviour
{
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

        amount = PlayerData.instance.itemStorage[name];

    }
    protected abstract void Useitem();
    void SetAmountText()
    {
        if (amountText == null)
            amountText = GetComponentInChildren<Text>();
        amountText.text = _amount.ToString();

    }
    public void TryUseItem()
    {
        if (amount > 0)
        {
            PlayerData.instance.UseItem(name);
            Useitem();
        }
        else
        {
            msgBox.PushMessage("아이템이 없습니다!");
        }
    }
}

