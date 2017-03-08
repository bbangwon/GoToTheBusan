using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie : MonoBehaviour {

    public enum ZombieState
    {
        None = -1,
        Idle = 0,
        Move,
        Follow,
        Attack,
        Damage,
        Dead
    }

    public enum ZombieColor
    {
        Red,
        Green,
        Blue,
        SkyBlue,
        Yellow
    }

    public ZombieState currentState = ZombieState.None;
    public ZombieColor currentColor;

    delegate void FSMFunc();

    Dictionary<ZombieState, FSMFunc> dicZombieState = new Dictionary<ZombieState, FSMFunc>();
    Transform target;

    public float followDistance;
    float moveSpeed;

    Vector3 idleMovePos;

    int followOrder;

    ZombieManager zombieManager;

    SpriteRenderer spriteRenderer;

    public Sprite[] sprites;

    public float animDelayTime;

    float animTime;
    int spriteIndex;

    Transform followPos;

    public float attackDelayTime;
    float attackTime;

    RoadManager roadManager;
    bool isAttacking;

    Transform attackPos;

    public float deadTime;

    void Start () {
        GameObject findObj = GameObject.FindGameObjectWithTag("ZombieManager");
        if (findObj != null)
            zombieManager = findObj.GetComponent<ZombieManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        dicZombieState[ZombieState.None] = None;
        dicZombieState[ZombieState.Idle] = Idle;
        dicZombieState[ZombieState.Move] = Move;
        dicZombieState[ZombieState.Follow] = Follow;
        dicZombieState[ZombieState.Attack] = Attack;
        dicZombieState[ZombieState.Damage] = Damage;
        dicZombieState[ZombieState.Dead] = Dead;

        FindTarget();

        roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
    }
	
	void Update () {

        animTime -= Time.deltaTime;
        if (animTime < 0)
        {
            animTime = animDelayTime;
            if (spriteIndex == 0)
            {
                spriteIndex = 1;

            }
            else
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = sprites[spriteIndex];
        }

        dicZombieState[currentState]();

        deadTime -= Time.deltaTime;
        if (transform.position.y < -10 || deadTime < 0)
            currentState = ZombieState.Dead;            

    }



    void OnEnable()
    {
        Init();
        animTime = animDelayTime;
        spriteIndex = 0;
    }

    void Init()
    {
        currentState = ZombieState.Idle;
        idleMovePos = Vector3.zero;
        isAttacking = false;
        moveSpeed = Random.Range(0.0f, 0.7f);
    }

    void FindTarget()
    {
        GameObject findObj = GameObject.FindWithTag("Player");
        if (null != findObj)
        {
            target = findObj.transform;
            attackPos = GameObject.FindWithTag("P.DamagePos").transform;
        }
    }

    void CheckFollow()
    {
        if (followOrder > 5)
        {
            currentState = ZombieState.Dead;
            return;
        }

        float distance = (target.position - transform.position).magnitude;

        if (distance < followDistance)
        {
            zombieManager.AddFollowList(gameObject);
            int randOrder = Random.Range(1, 5);
            SetFollowOrder(randOrder);
            if (followOrder==1)
            {
                attackTime = 0;
                currentState = ZombieState.Attack;
            }
            else
            {
                currentState = ZombieState.Follow;
            }
            
        }
    }

    public void SetFollowOrder(int order)
    {
        if (order < 1)
        {
            followOrder = 1;
        }
        else
        {
            followOrder = order;
        }
        
        followPos = zombieManager.followPosArr[order - 1];
    }

    public int GetFollowOrder()
    {
        return followOrder;
    }

    void Idle()
    {
        currentState = ZombieState.Move;
    }

    void None()
    {

    }

    void Damage()
    {

    }

    void Move()
    {
        if (idleMovePos == Vector3.zero)
            idleMovePos = new Vector3(transform.position.x + Random.Range(-1.0f, 1.0f), transform.position.y);
        Vector3 dir = idleMovePos - transform.position;
        transform.position += dir * (moveSpeed / 2) * Time.deltaTime;

        float distance = (idleMovePos - transform.position).magnitude;
        if (distance < 0.1f)
        {
            idleMovePos = new Vector3(transform.position.x + Random.Range(-1.0f, 1.0f), transform.position.y);
        }

        if(currentState != ZombieState.Attack && currentState != ZombieState.Follow)
        {
            transform.position += Vector3.down * Time.deltaTime * roadManager.roadMoveSpeed;
        }

        CheckFollow();
    }

    void Follow()
    {
        Vector3 dir = followPos.position - transform.position;
        dir.x += Random.Range(-1.5f, 1.5f);
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void Attack()
    {
        attackTime += Time.deltaTime;
        if (attackTime > attackDelayTime)
        {
            attackTime = 0;
            isAttacking = true;
            StartCoroutine(AttackProcess());
        }
        
        if (!isAttacking)
        {
            Vector3 dir = followPos.position - transform.position;
            dir.x += Random.Range(-1.5f, 1.5f);
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator AttackProcess()
    {
        Collider2D collider = GetComponent<BoxCollider2D>();
        while (!collider.OverlapPoint(attackPos.position))
        {
            Vector3 dir = attackPos.position - transform.position;
            transform.position += dir * (moveSpeed * 2) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isAttacking = false;
        StopCoroutine(AttackProcess());
    }

    void Dead()
    {
        zombieManager.GetFollowList().Remove(gameObject);
        Destroy(gameObject);
    }

}
