using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField]
     float damage;
    Animator animator;
    Collider2D coll2D;
    AttackDetails attackDetails;
    private void Start() {
        animator = GetComponent<Animator>();
        coll2D = GetComponent<Collider2D>();
    }
     void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            attackDetails.damage = damage;
            attackDetails.position = transform.position;
            animator.Play("attack");
           
            coll2D.enabled = false;
        }
    }
}
