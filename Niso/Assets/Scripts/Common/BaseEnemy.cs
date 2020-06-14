using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    protected float speed = 0f;
    protected  Rigidbody2D rb2d ;
    protected Animator anim;
    protected Collider2D coll2d;
    protected HPController HealthController;
    protected enum State
    {
        PATROL,
        ATTACK,
        CHASING
    }
    protected State state;
    protected virtual void Update() {
        Attack();
        CheckDistance();
    }
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        HealthController.dieEvent = EnemyDie;
    }
    public virtual void TakenDamage(int damage)
    {
        CreateController.Instance.CreateBleed(transform.position + new Vector3(1f, 0, 0));
        CreateController.Instance.CreateHit(transform.position);
        CreateController.Instance.CreateLighting(transform.position);
    }
    protected virtual void EnemyDie()
    {
        rb2d.AddForce(new Vector2(2f, 2f), ForceMode2D.Impulse);
        coll2d.enabled = false;
        this.enabled = false;
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    protected abstract void Attack();
    protected abstract void CheckDistance();
}
