using System.Collections.Generic;
using UnityEngine;

public class UnitHpBar : MonoBehaviour
{
    private Movable target;

    private GameObject hpRed;
    private GameObject hpBack;

    private Vector3 newScale;

    private void Awake()
    {
        if (transform.parent != null)
        {
            Init();
        }
    }

    private readonly Color ourForceBackColor = new Color(1, 234f / 255f, 60f / 255f, 1);
    private readonly Color enemyBackColor = new Color(144f / 255f, 221f / 255f, 70f / 255f, 1);

    public void Init()
    {
        target = transform.parent.GetComponent<Movable>();
        if (target.forDecoration)
        {
            Destroy(this.gameObject);
            return;
        }
        hpRed = transform.FindChild("Red").gameObject;
        hpBack = transform.FindChild("Hp Back").gameObject;

        hpBack.GetComponent<SpriteRenderer>().color = target.isOurForce ? ourForceBackColor : enemyBackColor;
        SetHpBar();
    }

    private void Update()
    {
        SetHpBar();
    }

    private void SetHpBar()
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