using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EngragedBehaviour : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    BossController boss;
    public float colliderAttack;
    public float attackRange;
    public float speed;
    GameObject go;
    int effCount = 0;
    int direction;
    float distance;
    AttackDetails attackDetails;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = PlayerController.Instance.transform;
        rb = animator.gameObject.GetComponent<Rigidbody2D>();
        boss = animator.gameObject.GetComponent<BossController>();
        distance = player.position.x - animator.transform.position.x;
        if (effCount == 0)
        {
            effCount = 1;
            go = CreateController.Instance.FireEffect(animator.transform.position);
            go.transform.parent = animator.transform;
        }

    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        attackDetails.damage = 10;
        attackDetails.position = animator.transform.position;
        if (Vector2.Distance(player.position, animator.transform.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
        boss.Flip();
        // Vector2 target = new Vector2(player.transform.position.x, rb.position.y);
        // Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
        // rb.MovePosition(newPos);
        if (distance > 0)
        {
            direction = 1;
        }
        if (distance < 0)
        {
            direction = -1;
        }
        if (Vector2.Distance(player.position,animator.transform.position) < colliderAttack)
        {
            player.SendMessage("TakenDamage",attackDetails);
        }
        rb.transform.position += (Vector3)Vector2.right * direction * Time.deltaTime * speed;
        if (boss.health.Value <= 0)
        {
            Destroy(go);
            boss.enabled = false;
            boss.coll.enabled = false;
            animator.SetBool("Die", true);
        }
    }
}
