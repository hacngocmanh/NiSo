using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : SingletonMono<BaseController>
{
    public float _damage;
    bool canAttack = true;
    int direction = 0;
    public int currentDirection ;
    [SerializeField]
    float attackRangeX;
    [SerializeField]
    float attackRangeY;
    [SerializeField]
    Transform attackPoint;
    [SerializeField]
    float attackDistance;
    [SerializeField]
    float AttackDelay;
    bool canFlip = true;
    bool isAttacking;
    bool isShooting = false;
    bool canShoot = true;
    bool canHurt = true;
    AttackDetails attackDetails;
    public HPController health;
    Animator anim;
    public Collider2D coll;
    void Start()
    {
        health = GetComponentInChildren<HPController>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        attackDetails.damage = _damage;
        attackDetails.position = transform.position;

        
    }
    public void Attack()
    {
           
    }
    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(AttackDelay);
        canAttack = true;
    }

    void TakenDamage(int damage)
    {
        CreateController.Instance.CreateBleed(transform.position + new Vector3(1f, 0, 0) * -direction);
        CreateController.Instance.CreateHit(transform.position);
        CreateController.Instance.CreateLighting(transform.position);
        health.ChangeHP(damage);
    }
    public void Flip()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if (playerPos.x - transform.position.x < 0 && canFlip)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            direction = 1;
        }
        if (playerPos.x - transform.position.x > 0 && canFlip)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = -1;
        }
        currentDirection = direction;

    }

    void OnAttack()
    {
        canFlip = false;
    }
    void DealDamage()
    {

        Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 0, LayerMask.GetMask("Player"));
        foreach (var item in hit)
        {
            item.SendMessage("TakenDamage", attackDetails);
            break;
        }
    }
    void EndAtatck()
    {
        canFlip = true;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("TakenDamage", attackDetails);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Gizmos.DrawWireSphere(leg.position,distance);
        //Gizmos.DrawRay(transform.position, Vector2.right * sizeOfSightX * dir);
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        //Gizmos.DrawWireSphere(transform.position, visibleSightY);
        //Gizmos.DrawRay(transform.position, Vector2.down * sizeOfSightY * dir);
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRangeX, attackRangeY, 0));
    }
}
