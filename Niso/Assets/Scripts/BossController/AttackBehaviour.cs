using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackBehaviour : StateMachineBehaviour
{
    Transform player;
    BossController boss;
    Rigidbody2D rb;
    public float speed;
    Vector2 target;
    float distance;
    int direction;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = PlayerController.Instance.transform;
        boss = animator.gameObject.GetComponent<BossController>();
        rb = animator.gameObject.GetComponent<Rigidbody2D>();
        target = new Vector2(player.transform.position.x, rb.position.y);
        distance = player.position.x - animator.transform.position.x; 
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (boss.health.Value <= 0)
        {
            animator.SetBool("Die", true);
        }
       
   
        if (distance > 0)
        {
            direction = 1;
        }
        if (distance < 0)
        {
            direction = -1;
        }
        //Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
        
    }
}
