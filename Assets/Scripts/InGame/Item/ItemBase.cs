using UnityEngine;
using UnityEngine.UI;

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
}