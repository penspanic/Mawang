using UnityEngine;
using System.Collections;

public class ObjectBase : MonoBehaviour
{
    #region GameDesign

    [SerializeField] protected float   attackRange     =   1;
    [SerializeField] protected int     hp              =   100;
    [SerializeField] protected int     canHitNum       =   1;
    [SerializeField] protected float   defensivePower =   0; // 1일때 받는 대미지 0
    [SerializeField] protected int     attackDamage = 1;

    #endregion

    #region Obj Variable

    protected BattleManager  battleMgr;

    protected float skillElapsedTime     = 0;
    protected bool  canUseSkill          = false;
    protected float damagedColorDuration = 0.2f;
    protected bool  isBleed = false; // 빨갛게 변해있는지 유무

    #endregion 

    #region Property
    public bool isOurForce
    {
        get;
        protected set;
    }

    public bool isDestroyed
    {
        get;
        protected set;
    }

    public int line
    {
        get;
        set;
    }

    public bool forDecoration
    {
        get;
        protected set;
    }

    public int maxHP
    {
        get;
        protected set;
    }
    #endregion

    protected virtual void Awake()
    {
        if (GameObject.FindObjectOfType<GameManager>() == null) // InGame 에서 유닛이 사용중이지 않을 때
        {
            forDecoration = true;
            return;
        }
        battleMgr = GameObject.FindGameObjectWithTag("Manager").GetComponent<BattleManager>();

        if (this.CompareTag("OurForce"))
            isOurForce = true;
        else
            isOurForce = false;

        

        maxHP = hp;
    }

    public int ApplyDefensive(int damage)
    {
        return (int)(damage - damage * defensivePower);
    }

    public void SetDefensivePower(float value)
    {
        defensivePower = value;
    }

    protected virtual IEnumerator ChangeDamageColor()
    {
        yield break;
    }

    public virtual void Attacked(int damage) { }

    public virtual ObjectBase[] GetTargets()
    {
        return null;
    }





    //--------------------------------------------------------
    // 프로퍼티로 보호하면서 인스펙터에 보여지게 하면 Delete
    public float GetAttackRange()
    {
        return attackRange;
    }
    public int   GetCanHitNum()
    {
        return this.canHitNum;
    }
    public int GetAttackDamage()
    {
        return attackDamage;
    }
    public void SetAddAttackDmg(int addDmg)
    {
        if(attackDamage + addDmg <= 0)
            attackDamage = 0;
        else
            attackDamage += addDmg;
    }
    public void SetMinusHP(int minusHP)
    {
        if (hp - minusHP >= 0)
            hp -= minusHP;
        else
            hp = 0;

        if (hp >= maxHP)
            hp = maxHP;
    }

    public void SetFullHP()
    {
        hp = maxHP;
    }
    public int GetHP()
    {
        return hp;
    }
}