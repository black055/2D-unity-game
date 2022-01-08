using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;
    CombatController combatController;
    StatController statController;
    SpriteRenderer spriteRenderer;
    SoundManager soundManager;

    private int facingDirection = 1;
    private float horizontal = 0f;
    private float nextSlideTime = 0f;
    private float nextDashTime = 0f;
    private float nextTimeBeingAttack = 0f;
    private float nextTimeMove = 0f;

    private bool isMoving;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isTouchingWallCorner;
    private bool isClimbingWall = false;
    private bool wallCornerDetected;
    private bool isCrouching;
    private bool isSliding;
    private bool isUnderneathSomething;
    private bool isDashing;
    private bool isAttacking;
    private bool knockback;

    [SerializeField]
    private float speed,slideSpeed, dashSpeed, crouchSpeed, jumpForce, wallSlideSpeed;
    [SerializeField]
    private float groundCheckRadius, ceilCheckRadius, wallCheckDistance;
    [SerializeField]
    private float wallJumpTime, slidingTime, dashingTime, slideCooldownTime, dashCooldownTime, beingAttackCooldownTime;
    [SerializeField]
    private float wallClimbXOffset1, wallClimbYOffset1, wallClimbXOffset2, wallClimbYOffset2;
    [SerializeField]
    private float slideStaminaCost, dashStaminaCost, wallJumpStaminaCost, wallSlideCost;
    [SerializeField]
    private float knockbackSpeedX, knockbackSpeedY, knockbackDuration, stunTime;

    private Vector2 wallCornerBottom;
    private Vector2 wallClimbPos1;
    private Vector2 wallClimbPos2;

    [SerializeField]
    private Vector2 wallJumpDirection;
    [SerializeField]
    private Transform wallCornerCheck, groundCheck, wallCheck;
    [SerializeField]
    private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        combatController = GetComponent<CombatController>();
        statController = GetComponent<StatController>();
        wallJumpDirection.Normalize();
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundManager = SoundManager.instance;
    }

    void Update() {
        if (!statController.IsDead()) {
            HandleInput();
            CheckMove();
            UpdateAnimations();
        }
        if (Time.time < nextTimeBeingAttack) {
            spriteRenderer.color = new Color(1, 0.5f, 0.5f);
        } else spriteRenderer.color = Color.white;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!(isClimbingWall || statController.IsDead())) {
            UpdatePosition();
            CheckSurroundings();
        }
    }

    private void UpdatePosition() {
        if (!(isSliding || isDashing) && isMoving) {
            if (isCrouching && isGrounded)
                transform.position += new Vector3(crouchSpeed * horizontal * Time.deltaTime, 0, 0);
            else
                transform.position += new Vector3(speed * horizontal * Time.deltaTime, 0, 0);
        }

        if (isSliding) {
            transform.position += new Vector3(slideSpeed * facingDirection * Time.deltaTime, 0, 0);
        } 

        if (knockback) {
            rb2d.velocity = new Vector2(knockbackSpeedX * -facingDirection, knockbackSpeedY);
        }

        if (isDashing) {
            transform.position += new Vector3(dashSpeed * facingDirection * Time.deltaTime, 0, 0);
            if (isTouchingWall) SetDashingToFalse();
        }
    }

    private void HandleInput() {
        if (!(isSliding || isDashing) && Time.time > nextTimeMove) horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && !isSliding && !isAttacking && !isClimbingWall) {
            if (isGrounded && !isUnderneathSomething) {
                soundManager.PlaySound("KnightJump");
                rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
                
            if (isWallSliding) {
                if (statController.UseStamina(wallJumpStaminaCost)) {
                    soundManager.PlaySound("KnightJump");
                    isWallJumping = true;
                    Invoke("SetWallJumpingToFalse", wallJumpTime);
                }
            }
        }

        // Check is crouching
        if (Input.GetButton("Crouch")) {
            isCrouching = true;
        } else {
            if (isUnderneathSomething && isGrounded) isCrouching = true;
            else isCrouching = false;
        }

        // Check if can slide (grounded, no crouching, no spam slide)
        if (Input.GetButtonDown("Slide") && isGrounded && !isCrouching && !isSliding && !isAttacking && Time.time >= nextSlideTime) {
            if (statController.UseStamina(slideStaminaCost)) {
                soundManager.PlaySound("KnightSlide");
                isSliding = true;
                nextSlideTime = Time.time + slideCooldownTime;
                animator.SetTrigger("isSliding");
                Invoke("SetSlidingToFalse", slidingTime);
            }
        }

        // Check if can slide (grounded, no touching wall, no crouching, no spam dash)
        if (Input.GetButtonDown("Dash") && !isTouchingWall  && !isCrouching && !isDashing && !isAttacking && Time.time >= nextDashTime) {
            if (statController.UseStamina(dashStaminaCost)) {
                soundManager.PlaySound("KnightDash");
                isDashing = true;
                nextDashTime = Time.time + dashCooldownTime;
                animator.SetBool("isDashing", true);
                Invoke("SetDashingToFalse", dashingTime);
            }
        }
    }

    private void SetWallJumpingToFalse() {
        isWallJumping = false;
    }

    private void SetSlidingToFalse() {
        isSliding = false;
    }

    private void SetDashingToFalse() {
        animator.SetBool("isDashing", false);
        isDashing = false;
    }

    private void CheckMove() {
        // Check move direction
        if(facingDirection == 1 && horizontal < 0)
        {
            FlipX();
        }
        else if(facingDirection == -1 && horizontal > 0)
        {
            FlipX();
        }

        // Check is moving
        if(horizontal != 0)
        {
            isMoving = true;
            if (isGrounded && !isDashing && !isSliding) {
                if (isCrouching) soundManager.PlaySound("KnightCrouchFootstep");
                else soundManager.PlaySound("KnightFootstep");
            }
        }
        else
        {
            isMoving = false;
        }

        // Check is wall is wall sliding
        if (isTouchingWall && !isGrounded && rb2d.velocity.y < 0 && statController.UseStamina(wallSlideCost * Time.deltaTime)) {
            isWallSliding = true;
        } else {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            if(rb2d.velocity.y < -wallSlideSpeed)
            {
                soundManager.PlaySound("KnightWallSlide");
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
        }

        if (isWallJumping) {
            rb2d.velocity = new Vector2(wallJumpDirection.x * horizontal * jumpForce, wallJumpDirection.y * jumpForce);
        }

        // Check climb wall
        if (wallCornerDetected && !isClimbingWall) {
            isClimbingWall = true;
            animator.SetBool("isClimbing", true);
            if (facingDirection == 1) {
                wallClimbPos1 = new Vector2(wallCornerBottom.x - wallClimbXOffset1, wallCornerBottom.y + wallClimbYOffset1);
                wallClimbPos2 = new Vector2(wallCornerBottom.x + wallClimbXOffset2, wallCornerBottom.y + wallClimbYOffset2);
            } else {
                wallClimbPos1 = new Vector2(wallCornerBottom.x + wallClimbXOffset1, wallCornerBottom.y + wallClimbYOffset1);
                wallClimbPos2 = new Vector2(wallCornerBottom.x - wallClimbXOffset2, wallCornerBottom.y + wallClimbYOffset2);
            }
            Invoke("climbFinished", 0.6f);
        }

        if (isClimbingWall) {
            transform.position = wallClimbPos1;
        }

        isAttacking = combatController.GetAttacking();
    }

    private void FlipX()
    {
        if (!(isClimbingWall || isWallSliding || isDashing)) {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void UpdateAnimations() {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb2d.velocity.y);
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("isCrouching", isCrouching);
    }

    private void CheckSurroundings()
    {
        if (isGrounded == false && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)) {
            soundManager.PlaySound("KnightFootstep");
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
        isTouchingWallCorner = Physics2D.Raycast(wallCornerCheck.position, transform.right, wallCheckDistance, groundLayer);
        isUnderneathSomething = Physics2D.Raycast(wallCheck.position, transform.up, ceilCheckRadius, groundLayer);
  
        if (isTouchingWall && !isTouchingWallCorner && !wallCornerDetected) {
            wallCornerDetected = true;
            wallCornerBottom = new Vector2 (Mathf.Floor(wallCheck.position.x +  wallCheckDistance), Mathf.Floor(wallCheck.position.y));
        }
    }

    private void climbFinished() {
        animator.SetBool("isClimbing", false);
        transform.position = wallClimbPos2;
        wallCornerDetected = false;
        isClimbingWall = false;
        rb2d.velocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        // Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        // Gizmos.DrawLine(wallCornerCheck.position, new Vector3(wallCornerCheck.position.x + wallCheckDistance, wallCornerCheck.position.y, wallCornerCheck.position.z));

        // Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x, wallCheck.position.y + ceilCheckRadius, wallCheck.position.z));
    }

    public bool GetState(string state) {
        switch(state) {
            case "isGrounded":
                return isGrounded;
            case "isCrouching":
                return isCrouching;
            default: return false;
        }
    }

    public int GetFacingDirection() {
        return facingDirection;
    }

    public bool CheckCanRegenStamina() {
        return !(isSliding || isWallSliding || isDashing || !isGrounded);
    }

    public void Damage(float attackDamage, float xPosition) {
        if (Time.time > nextTimeBeingAttack && !isDashing) {
            int attackerDirection = transform.position.x > xPosition ? -1 : 1;
            if (facingDirection != attackerDirection) FlipX();
            Knockback();
            statController.ChangeHealth(-attackDamage);
        }
        
    }

    private void Knockback() {
        horizontal = 0;
        isSliding = false;
        knockback = true;
        nextTimeMove = Time.time + stunTime;
        nextTimeBeingAttack = Time.time + beingAttackCooldownTime;
        Invoke("SetKnockbackToFalse", knockbackDuration);
        combatController.StopAttack();
        soundManager.PlaySound("KnightHurt");
    }

    private void SetKnockbackToFalse() {
        knockback = false;
    }
}
