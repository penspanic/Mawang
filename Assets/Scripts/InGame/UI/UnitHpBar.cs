using UnityEngine;
using System.Collections.Generic;

public class UnitHpBar : MonoBehaviour
{

    Movable target;

    GameObject hpRed;
    GameObject hpBack;

    Vector3 newScale;

    void Awake()
    {

        if (transform.parent != null)
        {
            Init();
        }
    }

    public void Init()
    {
        target = transform.parent.GetComponent<Movable>();
        if(target.forDecoration)
        {
            Destroy(this.gameObject);
            return;
        }
        hpRed = transform.FindChild("Red").gameObject;
        hpBack = transform.FindChild("Hp Back").gameObject;

        hpBack.GetComponent<SpriteRenderer>().color = target.isOurForce ? Color.yellow : Color.green;
        SetHpBar();
    }
    void Update()
    {
        SetHpBar();
    }

    void SetHpBar()
    {
        newScale = Vector3.one;
        newScale.x = 1f - ((float)target.GetHP() / (float)target.maxHP);
        hpRed.transform.localScale = newScale;
    }

    public SpriteRenderer[] GetSpriteRenderers()
    {
        List<SpriteRenderer> rendererList = new List<SpriteRenderer>();
        rendererList.Add(GetComponent<SpriteRenderer>());
        rendererList.AddRange(GetComponentsInChildren<SpriteRenderer>());

        return rendererList.ToArray();
    }
}