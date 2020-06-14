using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    PlayerInput input;
    AttackDetails attackDetails;
    Animator anim;
    [SerializeField]
    public float damage;
    [SerializeField]
    float attackRate = 2f;
    PlayerController player;
    bool isAttack = true;
    Vector3 attackPoint;
    [SerializeField]
    float attackRangeX;
    [SerializeField]
    float attackRangeY;
    bool attackUp;
    bool canMove = true;
    public LayerMask layerToDamage;
    void Start()
    {
        player = PlayerController.Instance;
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        player.canMove = canMove;
        Attack();
    }
    void Attack()
    {
        if (!attackUp)
        {
            attackPoint.x = transform.position.x + 0.7f * player.currentDirection;
            attackPoint.y = transform.position.y;
            attackRangeX = 2f;
            attackRangeY = 1;
        }

        if (input.keyW && input.attackPressed && isAttack)
        {
            anim.Play("attack3");
            attackUp = true;
            isAttack = false;
            StartCoroutine(AttackRate());
        }
        else if (input.attackPressed && isAttack)
        {
            int a = Random.Range(1, 3);
            anim.Play("attack" + a);
            isAttack = false;
            StartCoroutine(AttackRate());
        }
    }
    void TakenDamage(AttackDetails attackDetails)
    {
        int direction;
        if (attackDetails.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        PlayerController.Instance.GetKnockBackDirection(direction,attackDetails.damage);
        PlayerController.Instance.DecreaseHealth(attackDetails.damage);
    }
    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(attackRate);
        isAttack = true;
    }
    void DisableMove()
    {
        if (attackUp == true)
        {
            attackRangeX = 1;
            attackRangeY = 2;
            attackPoint = new Vector3(transform.position.x, transform.position.y + 1, 0);
            canMove = false;
        }
        canMove = false;
    }
    void DealDamage()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint, new Vector2(attackRangeX, attackRangeY), 0, layerToDamage);

        foreach (var item in hit)
        {
            item.SendMessage("TakenDamage", damage);
        }
    }
    void EnableMove()
    {
        canMove = true;
        attackUp = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(attackPoint, new Vector2(attackRangeX, attackRangeY));
    }
}
