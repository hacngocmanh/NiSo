using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMono<PlayerController>
{

    //Ground Movement
    [SerializeField]
    float speed;
    bool isOnGround;
    public Vector2 posLeg;
    public int playerDirection;
    public int currentDirection = 1;
    //Attack
 
    public bool canMove = true;
    //WallSlice
    bool isWallSliding;
    public float wallSliceSpeed;
    public float wallJumpTime = 0.2f;
    //jump
    public Vector2 wallJumpDir;
    public float jumpForce;
    public float wallJumpForce;
    bool isTouchingWall;
    public float dashForce;
    bool canWallJump = false;
    public float raylength;
    bool isKnockBack = false;
    //dash
    bool canDash = true;
    bool dash;
    //UnlockAbility
    bool unlockedDash = false;
    bool unlockedWallJump = false;
    PlayerInput input;
    Rigidbody2D rigidBody;
    int knockbackDirection;
    Animator anim;
    GameObject dashEffect;
    Collider2D[] coll;
    [SerializeField]
    PlayerHPbar HPcontrol;
    void Start()
    {
        isWallSliding = false;
        input = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponents<Collider2D>();
        dashEffect = GameObject.Find("Dash");
        dashEffect.SetActive(false);
        HPcontrol.dieEvent = PlayerDie;
    }
    private void FixedUpdate()
    {
        PhysicCheck();
        GroundMovement();
        KnocBack();
        Jumping();
        Dash();
    }
    private void Update()
    {
        if (canMove) anim.SetFloat("speed", Mathf.Abs(input.horizontal));
        anim.SetBool("isOnGround", isOnGround);
        if (!isKnockBack) anim.SetFloat("verticalVelocity", rigidBody.velocity.y);
        anim.SetBool("isWallSlide", isWallSliding);
        WallSlide();
        CheckJump();
        CheckDash();
    }
    void PlayerDie()
    {
        this.enabled = false;
        foreach (var item in coll)
        {
            item.enabled = false;
        }
        Observer.Instance.Notify(TOPPICNAME.PLAYERDIE);
        anim.Play("PlayerDie");
    }
    
    void PhysicCheck()
    {
        isOnGround = false;
        RaycastHit2D rightLeg = Raycast(posLeg, Vector2.down, 0.25f);
        RaycastHit2D leftLeg = Raycast(new Vector2(-posLeg.x, posLeg.y), Vector2.down, 0.25f);
        if (rightLeg || leftLeg)
        {
            isOnGround = true;
        }

        if (currentDirection == 1)
        {
            isTouchingWall = Raycast(Vector2.zero, -Vector2.left, raylength);
        }
        if (currentDirection == -1)
        {
            isTouchingWall = Raycast(new Vector2(-Vector2.zero.x, Vector2.zero.y), Vector2.left, raylength);
        }
    }
    void WallSlide()
    {
        if (isTouchingWall && !isOnGround && input.horizontal != 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    void CheckJump()
    {
        if (isWallSliding && input.horizontal != playerDirection)
        {
            canWallJump = true;
            StartCoroutine(WallJumpTimeDelay());
        }
    }
    IEnumerator WallJumpTimeDelay()
    {
        yield return new WaitForSeconds(wallJumpTime);
        canWallJump = false;
    }
    void Dash()
    {
        if (dash)
        {
            anim.SetTrigger("dash");
            dashEffect.SetActive(true);
            rigidBody.velocity = Vector2.zero;
            canMove = false;
            rigidBody.velocity = new Vector2(dashForce * currentDirection, 1);
            playerDirection = 0;
            StartCoroutine(DashTime());
        }
    }
    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(0.15f);
        dashEffect.SetActive(false);
        canMove = true;
        dash = false;
    }
    void CheckDash()
    {
        if (input.dashPressed && canDash)
        {
            dash = true;
            StartCoroutine(DashReload());
        }

    }
    IEnumerator DashReload()
    {
        yield return new WaitForSeconds(2);
        canDash = true;
    }
    void GroundMovement()
    {
        float xVelocity = input.horizontal * speed;
        if (input.horizontal < 0)
        {
            playerDirection = -1;
            currentDirection = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (input.horizontal > 0)
        {
            playerDirection = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            currentDirection = 1;
        }
        else if (input.horizontal == 0)
        {
            playerDirection = 0;
        }

        if (canMove)
        {
            rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
        }
        if (!canMove)
        {
            rigidBody.velocity = new Vector2(1 * playerDirection, rigidBody.velocity.y);
        }
        if (isWallSliding)
        {
            if (rigidBody.velocity.y < wallSliceSpeed)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, wallSliceSpeed);
            }

        }
    }
    void Jumping()
    {
        if (input.jumpPressed)
        {
            if (isOnGround)
            {
                rigidBody.velocity = Vector2.zero;
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            }

            if (canWallJump && unlockedWallJump)
            {
                Vector2 force = new Vector2(wallJumpForce * wallJumpDir.x * playerDirection, wallJumpForce * wallJumpDir.y);
                rigidBody.velocity = Vector2.zero;
                rigidBody.AddForce(force, ForceMode2D.Impulse);
            }
        }

    }
    void KnocBack()
    {
        if (isKnockBack)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(new Vector2(10 * knockbackDirection, 5), ForceMode2D.Impulse);
        }

    }
    public void GetKnockBackDirection(int direction,float damage)
    {
        isKnockBack = true;
        anim.SetTrigger("hurt");
        knockbackDirection = direction;
        canMove = false;
        StartCoroutine(KnockBackTime());
    }
    IEnumerator KnockBackTime()
    {
        yield return new WaitForSeconds(0.2f);
        isKnockBack = false;
        canMove = true;
       
    }
    public void DecreaseHealth(float health)
    {
        HPcontrol.ChangeHP((int)health);
    }
   
    #region 
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        return Raycast(offset, rayDirection, length, LayerMask.GetMask("Ground"));
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
    #endregion


}
