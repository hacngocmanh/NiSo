using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChasingBehaviour : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    [SerializeField]
    float attackRange;
    Transform thisTranform;
    BossController boss;
    public int direction;
    [SerializeField]
    float speed;
    GameObject go;
    public float attackRate = 2;
    float nextAttackTime = 0;
    bool canAttack = true;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = PlayerController.Instance.transform;
        rb = animator.gameObject.GetComponent<Rigidbody2D>();
        boss = animator.gameObject.GetComponent<BossController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.position, animator.transform.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
       if (boss.health.Value <= boss.health.MaxValue/2)
        {
            animator.SetBool("Tranform", true);
        }
         Vector2 target = new Vector2(player.transform.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
        rb.MovePosition(newPos);
        boss.Flip();
       
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

}
