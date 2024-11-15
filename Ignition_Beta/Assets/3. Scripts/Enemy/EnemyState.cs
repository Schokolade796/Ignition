using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyState : MonoBehaviour, IHitAble
{
    EnemyAnimation enemyAnim;
    EnemyMove enemyMove;

    private float maxHP = 100;
    private float currentHP;
    private float dmg = 10;

    private int playerLayer;
    private int barrierLayer;

    public enum State{Idle, Attack, AttackWait}
    public State state = State.Idle;
    private float currentTime;
    private float attackDelay = 2.5f;
    private float attackDistance = 5f;
    private float distance;
    private Vector3 lookPostion;
    private bool isDead;
    private bool inDistance;

    public bool IsDead {  get { return isDead; } }
    public bool IsStuned { get; set; }
    public bool IsAttack { get; set; }
    public bool InDistance { get { return inDistance; } }

    // Start is called before the first frame update
    void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyMove = GetComponent<EnemyMove>();
        currentHP = maxHP;
        currentTime = attackDelay;
        isDead = false;
        IsStuned = false;

        playerLayer = LayerMask.NameToLayer("Player");
        barrierLayer = LayerMask.NameToLayer("Barrier");
    }
    private void OnEnable()
    {
        StartCoroutine(this.CheckState());
        StartCoroutine(this.StateForAction());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (currentTime <= attackDelay)
        {
            currentTime += Time.deltaTime;
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    StartCoroutine(enemyMove.BackMove(-lookPostion));
        //}
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            if (enemyMove.target.transform.TryGetComponent(out Collider col))
            {
                lookPostion = col.ClosestPoint(transform.position) - transform.localPosition;
                distance = Vector3.Distance(col.ClosestPoint(transform.position), transform.position);
            }
                
            else 
                distance = Vector3.Distance(enemyMove.target.transform.position, this.transform.position);
            if (distance <= attackDistance)
            {
                state = State.Attack;
                inDistance = true;
            }
            else
            {
                state = State.Idle;
                inDistance = false;
                currentTime = attackDelay;
            }
        }
    }

    IEnumerator StateForAction()
    {
        while (!isDead)
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Attack:
                    Attack();
                    break;
            }
            yield return null;
        }
    }

    private void Attack()
    {
        if (currentTime >= attackDelay)
        {
            IsAttack = true;
            if (enemyMove.target.layer == playerLayer)
            {
                enemyAnim.SetTrigger("Bite");
                if (distance <= attackDistance)
                {
                    if (enemyMove.target.transform.TryGetComponent<IHitAble>(out var h))
                        h.Hit(dmg,"");
                }
            }
            else if (enemyMove.target.layer == barrierLayer)
            {
                enemyAnim.SetTrigger("stinger");
                if (distance <= attackDistance)
                {
                    if (GameManager.Instance.barrier.transform.TryGetComponent<IHitAble>(out var h))
                        h.Hit(dmg, "");
                }
            }
            currentTime = 0;
        }
    }

    public void Hit(float dmg, string coliName)
    {
        if (coliName == "WeakPoint")
        {
            dmg = 100;
            enemyAnim.SetTrigger("getHitHead");
        }
        else
        {
            enemyAnim.SetTrigger("getHit");
        }
        currentHP -= dmg;
        IsStuned = true;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        enemyAnim.isDie(true);
        transform.GetChild(0).gameObject.SetActive(false);
        currentHP = maxHP;
    }

    public void Respawn()
    {
        if (!GameManager.Instance.enemyGenerate.canSpawn) return;
        isDead = false;
        enemyAnim.isDie(false);
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.position = GameManager.Instance.enemyGenerate.GenEnemy();
        StartCoroutine(this.CheckState());
        StartCoroutine(this.StateForAction());
    }
}
