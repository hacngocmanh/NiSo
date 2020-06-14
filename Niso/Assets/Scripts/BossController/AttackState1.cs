using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackState1 : StateMachineBehaviour {
    BossController boss;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        boss = animator.gameObject.GetComponent<BossController>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
      if (boss.health.Value <= boss.health.MaxValue/2)
        {
            animator.SetBool("Tranform", true);
        }
    }
}
