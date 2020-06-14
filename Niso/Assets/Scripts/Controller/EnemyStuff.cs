using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStuff : BaseEnemy
{
    [SerializeField]
    int damage;
    float normalSpeed = 2;
    public float distance;
    bool canMove;
    public Transform wallCheck;
    public LayerMask layerCheck;
    bool isOnGround = false;
    private bool movingRight = false;
    Vector3 currentPos;
    [SerializeField]
    Transform attackPoint;
    int detectCount;
    public float speedChasing;
    public float attackRangeX;
    public float attackRangeY;
    public LayerMask PlayerMask;
    public float delayAttack;
    bool isAttacking;
    bool canAttack;
    public Transform GroundCheck;
    public int dir;
    [SerializeField]
    float radius = 0.2f;
    float knockbackDuration = 0.2f;
    bool isKnockBack;
    public float sizeOfSightX;
    public float sizeOfSightY;
    public float attackDistance;
    GameObject obj;
    bool isTouchingWall;
    AttackDetails attackDetails;
    protected override void Start()
    {
        detectCount = 0;
        state = State.PATROL;
        HealthController = GetComponentInChildren<HPController>();
        isAttacking = false;
        canAttack = true;
        coll2d = GetComponent<Collider2D>();
        obj = CreateController.Instance.CreateDetect(transform.position + new Vector3(0, 1.5f, 0));
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        base.Start();
    }
    private void FixedUpdate()
    {
        isOnGround = Physics2D.Raycast(wallCheck.position, Vector2.down, distance, layerCheck);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, distance, layerCheck);
        Color color = isOnGround ? Color.red : Color.green;
        Color color2 = isTouchingWall ? Color.red : Color.green;
        Debug.DrawRay(wallCheck.position, -transform.right * distance, color2);
        Debug.DrawRay(wallCheck.position, Vector2.down * distance, color);
        KnockBack();
    }
    protected override void Update()
    {
        attackDetails.damage = damage;
        attackDetails.position = transform.position;
        anim.SetInteger("speed", Mathf.Abs(dir));
        base.Update();
        Patrol();
        Flip();
    }
    public void Flip()
    {
        if (dir == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Patrol()
    {
        if (state == State.PATROL)
        {
            obj.SetActive(false);
            detectCount = 0;
            speed = normalSpeed;
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (isOnGround == false || isTouchingWall == true)
            {
                if (dir == 1)
                {

                    dir = -1;
                }
                else
                {

                    dir = 1;
                }
            }

        }

    }
    protected override void Attack()
    {
        Transform playerPos = PlayerController.Instance.transform;

        if (state == State.CHASING && !isKnockBack && !isAttacking)
        {
            Vector2 target = new Vector2(playerPos.position.x, rb2d.position.y);


            if (isOnGround)
            {
                Vector2 newPos = Vector2.MoveTowards(rb2d.position, target, speed * Time.deltaTime);
                transform.position = newPos;
                obj.SetActive(true);
                detectCount = 1;
                state = State.CHASING;
                speed = speedChasing;
                if (playerPos.position.x - transform.position.x < 0)
                {
                    dir = -1;
                }
                if (playerPos.position.x - transform.position.x > 0)
                {
                    dir = 1;
                }
            }



        }
        if (state == State.ATTACK && canAttack == true)
        {
            canAttack = false;
            anim.SetTrigger("isAttack");
            StartCoroutine(DelayAttack());
        }
    }
    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
        canAttack = true;
    }
    protected override void CheckDistance()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector2.Distance(playerPos, transform.position) <= sizeOfSightX)
        {
            if (Mathf.Abs(playerPos.y - transform.position.y) > sizeOfSightY)
            {
                state = State.PATROL;
                return;
            }
            if (Vector2.Distance(playerPos, transform.position) < attackDistance)
            {
                state = State.ATTACK;
            }
            else
            {
                state = State.CHASING;
            }

        }

        if (Vector2.Distance(playerPos, transform.position) > sizeOfSightX)
        {
            state = State.PATROL;
        }
    }
    public override void TakenDamage(int damage)
    {
        anim.Play("hurt");
        isKnockBack = true;
        HealthController.ChangeHP(damage);
        base.TakenDamage(damage);
        StartCoroutine(KnockBackTime());
    }
    void KnockBack()
    {
        if (isKnockBack)
        {
            rb2d.velocity = new Vector2(-dir * 2, 0);
        }
    }
    IEnumerator KnockBackTime()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockBack = false;
    }
    protected override void EnemyDie()
    {
        obj.SetActive(false);
        anim.Play("die");
        base.EnemyDie();
    }
    void DealDamage()
    {
        attackDetails.damage = damage;
        attackDetails.position = transform.position;
        Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 0, PlayerMask);
        foreach (var item in hit)
        {
            item.SendMessage("TakenDamage", attackDetails);
            break;
        }
    }
    void OnAttack()
    {
        isAttacking = true;
        rb2d.velocity = Vector2.zero;
    }
    void EndAttack()
    {
        isAttacking = false;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("TakenDamage", attackDetails);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRangeX, attackRangeY, 0) * dir);
        Gizmos.DrawWireCube(transform.position, new Vector3(sizeOfSightX * 2, sizeOfSightY * 2, 0));
    }
}
