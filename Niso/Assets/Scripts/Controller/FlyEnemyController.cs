using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlyEnemyController : BaseEnemy
{
    public float circleRadius;
    Vector3 currentPos;
    public GameObject rightCheck, roofCheck, groundCheck;
    public LayerMask groundLayer;
    private bool facingRight = true, groundTouch, roofTouch, righttouch;
    public float dirX, dirY;
    bool isAttack = true;
    Rigidbody2D rigidBody;
    Animator animator;
    public float lineOfsite;
    public float attackRange;
    public float speedAttack;
    Vector2 lastPos;
    [SerializeField] GameObject bloom;
    protected override void Start()
    {
        base.Start();
        HealthController = GetComponentInChildren<HPController>();
    }
    protected override void Update()
    {
        Attack();
    }
    void FixedUpdate()
    {

       rigidBody.velocity = new Vector2(dirX, dirY) * speed * Time.deltaTime;
        HitDetection();
    }
    void Dead()
    {
        animator.Play("die");
        //rigidBody.AddForce(new Vector2(5f, 3f), ForceMode2D.Impulse);
        this.enabled = false;
    }
    void HitDetection()
    {
        righttouch = Physics2D.OverlapCircle(rightCheck.transform.position, circleRadius, groundLayer);
        roofTouch = Physics2D.OverlapCircle(roofCheck.transform.position, circleRadius, groundLayer);
        groundTouch = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);
        HitLogic();
    }

    void HitLogic()
    {
        if (righttouch && facingRight)
        {
            Flip();
        }
        else if (righttouch && !facingRight)
        {
            Flip();
        }
        if (roofTouch)
        {
            dirY = -0.25f;
        }
        else if (groundTouch)
        {
            dirY = 0.25f;
        }


    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        dirX = -dirX;
    }

    public override void TakenDamage(int damage)
    {
        HealthController.ChangeHP(damage);
        base.TakenDamage(damage);
    }
    IEnumerator DeLayAttack()
    {
        yield return new WaitForSeconds(3);
        isAttack = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rightCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(roofCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(transform.position, lineOfsite);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected override void Attack()
    {
        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);

        if (distance < lineOfsite)
        {
            speedAttack = 5;
            transform.position = Vector2.MoveTowards(transform.position,PlayerController.Instance.transform.position,Time.deltaTime *speedAttack);
            if (distance < 1)
            {
                transform.position = Vector2.MoveTowards(transform.position,PlayerController.Instance.transform.position + new Vector3(2,2,0),Time.deltaTime *speedAttack);
            }
            if (distance <= attackRange && isAttack)
            {
                isAttack = false;
                Instantiate(bloom,transform.position,Quaternion.identity);
                //PlayerController.Instance.TakenDamage(20);
                StartCoroutine(DeLayAttack());
            }
        }
    }

    protected override void CheckDistance()
    {
       
    }
}