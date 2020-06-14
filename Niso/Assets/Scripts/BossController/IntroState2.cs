using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState2 : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.SetTrigger("Chasing");
    }
}
