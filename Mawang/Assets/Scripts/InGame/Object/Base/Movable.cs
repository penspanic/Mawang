using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MovableState
{
    Advance = 0,
    Idle,
    Attack,
    Death,
    Skill
}


/// <summary>
/// 죽을땐 MovableState.Death 를바꾼다.
/// </summary>
public class Movable : ObjectBase, System.IComparable<Movable>
{
    #region GameDesign

    [SerializeField]    private float           moveSpeed   = 1;
    [SerializeField]    private float           attackSpeed = 1;
    [SerializeField]    private int             unitCost    = 50;
    [SerializeField]    private float           acCoolTime  =  0;
    [SerializeField]    private Vector2         adjustPos;
    [SerializeField]    protected AudioClip     attackSound;
    [SerializeField]    protected AudioClip     skillSound;
    [SerializeField]    protected int           deathReward = 50;

    #endregion

    #region Property

    public MovableState state
    {
        get;
        protected set;
    }

    public bool isMoveRight
    {
        get;
        protected set;
    }

    public float attackInterval
    {
        get;
        protected set;
    }
    public bool isEffecting
    {
        get;
        set;
    }
    public bool isFreezed
    {
        get;
        protected set;
    }
    #endregion

    #region Obj Variable

    protected SpawnManager              spawnMgr;
    protected GoldManager               goldMgr;
    protected Animator                  animator;
    protected Collider2D                touchCollider;
    protected SpriteOrderLayerManager   orderMgr;
    protected PrincessManager           princessMgr;
    protected AudioSource               audioSource;

    protected bool  canAttack           = true;
    protected float attackElapsedTime   = 0;
    protected bool  isSkillMotion       = false;
    protected bool  isOneShotSound      = false;
    protected SpriteRenderer[] sprs;
    private   float disappearDuration   = 0.6f;
    #endregion 


    protected override void Awake()
    {
        base.Awake();
        List<SpriteRenderer> sprList = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>(true));
        sprList.RemoveAll((obj) =>
        {
            if (obj.CompareTag("Skill Effect"))
                return true;
            else
                return false;
        });
        sprs = sprList.ToArray();

        if (forDecoration)
            return;

        battleMgr.AddObject(this);

        goldMgr             =   GameObject.FindObjectOfType<GoldManager>();
        orderMgr            =   GameObject.FindGameObjectWithTag("Manager").GetComponent<SpriteOrderLayerManager>();
        spawnMgr            =   GameObject.FindGameObjectWithTag("Manager").GetComponent<SpawnManager>();
        princessMgr         =   GameObject.FindGameObjectWithTag("Manager").GetComponent<PrincessManager>();
        animator            =   GetComponent<Animator>();
        audioSource         =   GetComponent<AudioSource>();
        attackInterval      =   (float)1f / attackSpeed;
        isMoveRight         =   isOurForce ? true : false;
        state               =   MovableState.Advance;

        if (isOurForce)
            touchCollider   =   this.GetComponent<Collider2D>();


        SettingLine();
        StartCoroutine(UnitProcess());
    }


    protected virtual IEnumerator UnitProcess()
    {
        while (!isDestroyed)
        {
            if (isFreezed)
            {
                yield return null;
                continue;
            }

            FindTarget();

            attackInterval = (float)1f / attackSpeed;

            WaitForAttack();
            WaitForActive();


            switch (state)
            {
                case MovableState.Advance:
                    Advance();
                    break;
                case MovableState.Idle:
                    Idle();
                    break;
                case MovableState.Attack:
                    Attack();
                    break;
                case MovableState.Skill:
                    Skill();
                    break;
            }
            yield return null;
        }

        // 죽는행동
        Death();
    }

    protected virtual void FindTarget()
    {
        ObjectBase[] targets = GetTargets();

        if (isSkillMotion)
        {
            state = MovableState.Skill;
            return;
        }

        if (targets == null)
            state   =   MovableState.Advance;
        else
        {
            if (canAttack)
                state   =   MovableState.Attack;
            else
                state   =   MovableState.Idle;
        }
    }

    void WaitForActive()
    {
        if(!isOurForce)
            return;

        if (canUseSkill == false)
        {
            skillElapsedTime += Time.deltaTime;
            if(skillElapsedTime >= acCoolTime)
            {
                canUseSkill         =   true;
                skillElapsedTime    =   0;
            }
        }
    }



    Vector3 deltaMove;
    protected void Advance()
    {
        animator.Play("Advance", 0);
        deltaMove = Vector2.right * Time.deltaTime * moveSpeed * (isMoveRight ? 1 : -1) * Time.timeScale;
        transform.position = transform.position + deltaMove;
    }

    protected void Idle()
    {
        animator.Play("Idle",0);
    }

    protected virtual void Attack()
    {
        animator.Play("Attack",0);

        if (!isOneShotSound)
        {
            isOneShotSound = true;
            PlaySound(attackSound);
        }
    }

    protected void Death()
    {
        animator.Play("Dead",0);
    }

    protected virtual void Skill()
    {
        animator.Play("Skill",0);
    }
    // 공격 대기시간
    protected void WaitForAttack()
    {
        if (canAttack == false)
        {
            attackElapsedTime += Time.deltaTime * Time.timeScale;
            if (attackElapsedTime >= attackInterval)
            {
                canAttack = true;
                attackElapsedTime = 0;
            }
        }
    }

    public override void Attacked(int damage)
    {
        damage = ApplyDefensive(damage);

        SetMinusHP(damage);
        if (GetHP() <= 0)
            isDestroyed = true;

        if (!isBleed)
            StartCoroutine(ChangeDamageColor());
    }

    protected void PlaySound(AudioClip sound)
    {
        if (audioSource == null)
            return;

        if (!audioSource.isPlaying && Time.timeScale == 1)
            audioSource.PlayOneShot(sound);
    }


    // 피격당했을 때 호출
    protected override IEnumerator ChangeDamageColor()
    {

        isBleed = true;

        SetColor(sprs, Color.red);

        yield return new WaitForSeconds(0.2f);

        if (isEffecting)
            SetColor(sprs, princessMgr.buffColor);
        else
            SetColor(sprs, Color.white);
        if (isFreezed)
            SetColor(sprs, FreezeItem.freezedColor);
        isBleed = false;
       
    }

    // FreezeItem 사용과 끝났을 때
    public void Freeze(bool value)
    {
        if (value)
        {
            SetColor(sprs, FreezeItem.freezedColor);
            isFreezed = true;
            animator.enabled = false;
        }
        else
        {
            SetColor(sprs, Color.white);
            isFreezed = false;
            animator.enabled = true;
        }
    }

    void SetColor(SpriteRenderer[] sprs, Color color)
    {
        for (int i = 0; i < sprs.Length; i++)
        {
            if(sprs[i].name.Contains("Effect"))
                continue;

            sprs[i].color = color;
        }
    }

    #region 애니메이션 이벤트 메서드

    // 공격모션 끝날때
    public virtual void AttackEnd()
    {
        isOneShotSound  =   false;
    }



    // 죽는모션 끝날때
    public void OnDeathEnd()
    {
        if(!isOurForce)
            goldMgr.AddGold(deathReward);

        battleMgr.RemoveObject(this);
        StartCoroutine(WaitForDissappear());
    }

    #endregion 

    // 지정된 시간만큼 죽고난뒤 투명화 되는거
    IEnumerator WaitForDissappear()
    {
        float beginTime = Time.time;
        float alpha = 1;
        while (alpha != 0)
        {
            alpha = EasingUtil.linear(1, 0, (Time.time - beginTime) / disappearDuration);

            for (int i = 0; i < sprs.Length; i++)
                sprs[i].color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }


    void SettingLine()
    {
        if (transform.position.y > -1)
            this.line = 1;
        else if (transform.position.y > -2)
            this.line = 2;
        else if (transform.position.y > -3)
            this.line = 3;

        SetSortingLayer("Line" + line + " Object");

        transform.position += GetAdjustPos();
        transform.position += new Vector3(0, Random.Range(0, 0.5f), 0);
    }


    public void SetSortingLayer(string layerName)
    {
        for(int i =0;i<sprs.Length;i++)
        {
            sprs[i].sortingLayerName = layerName;
        }
    }

    #region Skill Unit Func

    protected void SkillMotionStart()
    {
        AudioClip sound;
        isSkillMotion   =   true;
        canUseSkill     =   false;
        if (skillSound == null)
            sound = attackSound;
        else
            sound = skillSound;
        PlaySound(sound);
    }

    protected void SkillMotionEnd()
    {
        isSkillMotion   =   false;
    }

    #endregion


    // gett
    #region Get

    public override ObjectBase[] GetTargets()
    {
        return battleMgr.GetTargets(this, this.GetAttackRange(), this.GetCanHitNum());
    }

    public int GetUnitCost()
    {
        return this.unitCost;
    }

    public Vector3 GetAdjustPos()
    {
        return this.adjustPos;
    }

    public SpriteRenderer[] GetSprs()
    {
         return sprs;
    }

    public void AddAttackSpeed(float percent)
    {
        attackSpeed += (percent / 100);
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void AddMoveSpeed(float add)
    {
        moveSpeed += add;
    }

    #endregion

    public int CompareTo(Movable other)
    {
        if (this.unitCost > other.unitCost)
            return 1;
        else if (this.unitCost < other.unitCost)
            return -1;
        return 0;
    }
}
