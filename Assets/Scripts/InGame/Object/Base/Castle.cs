using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : ObjectBase
{
    private SpriteRenderer front;
    [SerializeField] private Image      hpBar;
    private float currAlpha;

    GameManager gameMgr;
    protected SpriteRenderer spr;
    private Vector3 addRange;

    protected override void Awake()
    {
        base.Awake();
        gameMgr = GameObject.FindObjectOfType<GameManager>();
        front   =   transform.FindChild("Front").GetComponent<SpriteRenderer>();
        spr     =   this.GetComponent<SpriteRenderer>();

        if (isOurForce)
            addRange = Vector3.left;
        else
            addRange = Vector3.right;
         
        StartCoroutine(CastleProcess());
    }

    protected IEnumerator CastleProcess()
    {
        while (true)
        {
            ValueUpdate(hp, maxHP);
            CoveringCastle();
            yield return null;

        }

    }

    public override ObjectBase[] GetTargets()
    {
        return battleMgr.GetTargets(this, this.attackRange, this.canHitNum);
    }

    private void ValueUpdate(float currValue, float maxValue)
    {
        hpBar.fillAmount = currValue / maxValue;
    }

    private void CoveringCastle()
    {
        if (battleMgr.SelectInRange(battleMgr.GetOpposite(!isOurForce), transform.position + addRange, 0.8f).Count != 0)
            currAlpha -= Time.deltaTime * 5f;
        else
            currAlpha += Time.deltaTime * 3f;

        front.color = new Color(1, 1, 1, Mathf.Clamp(currAlpha, 0.5f, 1f));
        currAlpha   = Mathf.Clamp(currAlpha, 0.5f, 1f);
    }

    public override void Attacked(int damage)
    {
        damage = ApplyDefensive(damage);
        hp -= damage;
        if (hp <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            gameMgr.CastleDestroyed(this);
        }

        if (!isBleed)
            StartCoroutine(ChangeDamageColor());

    }


    protected override IEnumerator ChangeDamageColor()
    {
        isBleed  =   true;

        // 성 앞부분이 빨갛게 안되서.
        spr.color   = Color.red;
        front.color = Color.red;

        yield return new WaitForSeconds(damagedColorDuration);

        spr.color   = Color.white;
        front.color = Color.white;

        isBleed  =   false;
    }



}
