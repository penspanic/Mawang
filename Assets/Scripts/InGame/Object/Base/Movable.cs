using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovableState
{
    Advance = 0,
    Idle,
    Attack,
    Death,
    Skill
}

/// <summary>
/// 움직이는 유닛들은 이 클래스를 상속받거나 사용한다.
///
/// 죽을땐 MovableState.Death 를 바꾼다.
/// </summary>
public class Movable : ObjectBase, System.IComparable<Movable>
{
    #region GameDesign

    [SerializeField]
    private float moveSpeed = 1;

    [SerializeField]
    private float attackSpeed = 1;

    [SerializeField]
    private int unitCost = 50;

    [SerializeField]
    private float acCoolTime = 0;

    [SerializeField]
    private Vector2 adjustPos;

    [SerializeField]
    protected AudioClip attackSound;

    [SerializeField]
    protected AudioClip skillSound;

    [SerializeField]
    protected int deathReward = 50;

    #endregion GameDesign

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

    #endregion Property

    #region Obj Variable

    protected SpawnManager spawnMgr;
    protected GoldManager goldMgr;
    protected Animator animator;
    protected Collider2D touchCollider;
    protected SpriteOrderLayerManager orderMgr;
    protected PrincessManager princessMgr;
    protected AudioSource audioSource;

    protected UnitHpBar hpBar;
    protected bool canAttack = true;
    protected float attackElapsedTime = 0;
    protected bool isSkillMotion = false;
    protected bool isOneShotSound = false;
    protected SpriteRenderer[] sprs;
    protected SpriteRenderer shadowRenderer;
    private float disappearDuration = 0.6f;
    private GameObject newShadow;
    private GameObject hpBarObj;

    #endregion Obj Variable

    #region static fields

    private static GameObject shadowPrefab;
    private static GameObject hpBarPrefab;

    #endregion static fields

    protected override void Awake()
    {
        base.Awake();
        List<SpriteRenderer> sprList = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>(true));
        sprList.RemoveAll((obj) =>
        {
            if (obj.CompareTag("Skill Effect") || obj.CompareTag("Shadow") || obj.CompareTag("Hp Bar"))
                return true;
            else
                return false;
        });
        sprs = sprList.ToArray();

        if (forDecoration)
        {
            return;
        }
        canUseSkill = true;

        battleMgr.AddObject(this);

        goldMgr = GameObject.FindObjectOfType<GoldManager>();
        orderMgr = GameObject.FindGameObjectWithTag("Manager").GetComponent<SpriteOrderLayerManager>();
        spawnMgr = GameObject.FindGameObjectWithTag("Manager").GetComponent<SpawnManager>();
        princessMgr = GameObject.FindGameObjectWithTag("Manager").GetComponent<PrincessManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        attackInterval = 1f / attackSpeed;
        isMoveRight = isOurForce ? true : false;
        state = MovableState.Advance;

        if (isOurForce)
            touchCollider = this.GetComponent<Collider2D>();

        SettingLine();
        if (!isOurForce)
        {
            CreateShadow();
            CreateHpBar();
        }

        hpBar = GetComponentInChildren<UnitHpBar>();

        SetSortingLayer("Line" + line + " Object", hpBar.GetSpriteRenderers());
        StartCoroutine(UnitProcess());
    }

    public void SetStat(UnitInfo info)
    {
        attackDamage = info.AttackDamage;
        attackSpeed = info.AttackSpeed;
        maxHP = info.HealthPoint;
        moveSpeed = info.MoveSpeed;
        canHitNum = info.HitNum;

        hp = maxHP;
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

            attackInterval = (float)1f / attackSpeed;

            WaitForAttack();
            WaitForActive();

            FindTarget();

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
            state = MovableState.Advance;
        else
        {
            if (canAttack)
                state = MovableState.Attack;
            else
                state = MovableState.Idle;
        }
    }

    private void WaitForActive()
    {
        if (!isOurForce)
            return;

        if (canUseSkill == false)
        {
            skillElapsedTime += Time.deltaTime;
            if (skillElapsedTime >= acCoolTime)
            {
                canUseSkill = true;
                skillElapsedTime = 0;
            }
        }
    }

    private Vector3 deltaMove;

    protected void Advance()
    {
        animator.Play("Advance", 0);
        deltaMove = Vector2.right * Time.deltaTime * moveSpeed * (isMoveRight ? 1 : -1) * Time.timeScale;
        transform.position = transform.position + deltaMove;
    }

    protected void Idle()
    {
        animator.Play("Idle", 0);
    }

    protected virtual void Attack()
    {
        if (name == "Medic")
            Debug.Log("attack");

        animator.Play("Attack", 0);

        if (!isOneShotSound)
        {
            isOneShotSound = true;
            PlaySound(attackSound);
        }
    }

    protected void Death()
    {
        if (isFreezed)
            animator.enabled = true;

        if (!isOurForce)
        {
            hpBarObj.SetActive(false);
            newShadow.SetActive(false);
        }

        animator.Play("Dead", 0);
    }

    protected virtual void Skill()
    {
        animator.Play("Skill", 0);
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

    private void SetColor(SpriteRenderer[] sprs, Color color)
    {
        for (int i = 0; i < sprs.Length; i++)
        {
            if (sprs[i].name.Contains("Effect"))
                continue;

            sprs[i].color = color;
        }
    }

    #region 애니메이션 이벤트 메서드

    // 공격모션 끝날때
    public virtual void AttackEnd()
    {
        isOneShotSound = false;
    }

    // 죽는모션 끝날때
    public void OnDeathEnd()
    {
        if (!isOurForce)
            goldMgr.AddGold(deathReward);

        battleMgr.RemoveObject(this);

        StartCoroutine(WaitForDissappear());
    }

    #endregion 애니메이션 이벤트 메서드



    // 지정된 시간만큼 죽고난뒤 투명화 되는거
    private IEnumerator WaitForDissappear()
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

    private void SettingLine()
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

    private void CreateShadow()
    {
        if (shadowPrefab == null)
            shadowPrefab = Resources.Load<GameObject>("Prefabs/Shadow Prefab");

        newShadow = Instantiate<GameObject>(shadowPrefab);
        newShadow.transform.SetParent(this.transform, false);
        float ratio = 1f / newShadow.transform.lossyScale.magnitude;
        newShadow.transform.localScale = Vector3.one * ratio;

        Vector2 localPos = Vector2.zero;

        if (name.Contains("Pawn"))
            localPos = new Vector2(-0.29f, -2f);
        else if (name.Contains("Archer"))
            localPos = new Vector2(-0.419f, -0.746f);
        else if (name.Contains("Bandsman"))
            localPos = new Vector2(0, -0.825f);
        else if (name.Contains("Shielder"))
            localPos = new Vector2(0, -0.757f);
        else if (name.Contains("Medic"))
            localPos = new Vector2(0, -0.757f);
        else if (name.Contains("Lanceman"))
            localPos = new Vector2(0.1f, -0.9f);
        newShadow.transform.localPosition = localPos;
    }

    private void CreateHpBar()
    {
        if (hpBarPrefab == null)
            hpBarPrefab = Resources.Load<GameObject>("Prefabs/UI/Unit Hp Bar");

        hpBarObj = Instantiate<GameObject>(hpBarPrefab);
        hpBarObj.transform.SetParent(transform, false);
        float ratio = 1f / hpBarObj.transform.lossyScale.magnitude;
        hpBarObj.transform.localScale = Vector3.one * ratio;

        Vector2 localPos = Vector2.zero;

        if (name.Contains("Pawn"))
            localPos = new Vector2(0.63f, 2.62f);
        else if (name.Contains("Archer"))
            localPos = new Vector2(0.109f, 1.979f);
        else if (name.Contains("Bandsman"))
            localPos = new Vector2(0.62f, 2.15f);
        else if (name.Contains("Shielder"))
            localPos = new Vector2(0.62f, 1.88f);
        else if (name.Contains("Medic"))
            localPos = new Vector2(0.28f, 1.3f);
        else if (name.Contains("Lanceman"))
            localPos = new Vector2(0.33f, 1.33f);
        hpBarObj.transform.localPosition = localPos;

        hpBarObj.GetComponent<UnitHpBar>().Init();
    }

    public void SetSortingLayer(string layerName, SpriteRenderer[] renderers = null)
    {
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].sortingLayerName = layerName;
            return;
        }
        for (int i = 0; i < sprs.Length; i++)
        {
            sprs[i].sortingLayerName = layerName;
        }
    }

    #region Skill Unit Func

    protected void SkillMotionStart()
    {
        AudioClip sound;
        isSkillMotion = true;
        canUseSkill = false;
        if (skillSound == null)
            sound = attackSound;
        else
            sound = skillSound;
        PlaySound(sound);
    }

    protected void SkillMotionEnd()
    {
        isSkillMotion = false;
    }

    #endregion Skill Unit Func

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

    #endregion Get

    public int CompareTo(Movable other)
    {
        if (this.unitCost > other.unitCost)
            return 1;
        else if (this.unitCost < other.unitCost)
            return -1;
        return 0;
    }
}