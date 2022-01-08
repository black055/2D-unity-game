using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, moveSpeed, damage;
    [SerializeField]
    private float knockbackSpeedX, knockbackSpeedY, knockbackDuration;
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private float groundCheckDistance, playerDetectDistance, attackRange, attackCooldown, stunTime, idleTime, moveDistance;
    [SerializeField]
    private int numAttackTime;
    [SerializeField]
    private EnemyHealthBar healthBar;

    private float nextTimeAttack = 0f;
    private float nextTimeMove;
    private float currentHealth;
    private float KnockbackEnd;
    private float xLimitLeft, xLimitRight;
    private int facingDirection;
    private int faceToPlayer;
    private int attackLeft;

    private bool knockback;
    private bool isNearCorner;
    private bool isTouchingWall;
    private bool isGrounded;
    private bool isAttacking;
    private bool isMoving;
    private bool playerDetected;
    private bool isTooFar;

    [SerializeField]
    private Transform wallCheck, groundCheck, cornerCheck, damageCheck, attackCheck;
    [SerializeField]
    private LayerMask groundLayer, knightLayer;

    private KnightController knightController;
    private GameObject aliveObject;
    private Rigidbody2D rbAlive;
    private Animator animator;

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        knightController = GameObject.Find("Knight").GetComponent<KnightController>();
        aliveObject = transform.Find("Alive").gameObject;
        rbAlive = aliveObject.GetComponent<Rigidbody2D>();
        animator = aliveObject.GetComponent<Animator>();
        facingDirection = 1;
        nextTimeMove = Time.time + idleTime;
        xLimitLeft = transform.position.x - moveDistance;
        xLimitRight = transform.position.x + moveDistance;
    }

    private void Update() {
        if (currentHealth > 0) {
            UpdateAnimations();
            UpdatePosition();
            CheckSurroundings();
            CheckAttackRange();
        }
    }

    private void UpdateAnimations() {
        isMoving = (Time.time >= nextTimeAttack && Time.time >= nextTimeMove);
        
        animator.SetBool("isMoving", isMoving);
    }

    private void UpdatePosition() {
        if (knockback && Time.time > KnockbackEnd) {
            knockback = false;
            rbAlive.velocity = new Vector2(0f, knockbackSpeedY);
        }

        if (!(knockback || isAttacking) && isMoving) {
            rbAlive.velocity = new Vector2(moveSpeed * facingDirection, rbAlive.velocity.y);
        }
    }

    private void CheckSurroundings() {
        isNearCorner = Physics2D.Raycast(cornerCheck.position, transform.up * -1f, groundCheckDistance, groundLayer);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, aliveObject.transform.right, groundCheckDistance, groundLayer);
        isGrounded = Physics2D.Raycast(groundCheck.position, transform.up * -1f, groundCheckDistance, groundLayer);
        isTooFar = cornerCheck.position.x < xLimitLeft || cornerCheck.position.x > xLimitRight;
        playerDetected = Physics2D.Raycast(damageCheck.position, aliveObject.transform.right, playerDetectDistance, knightLayer);

        if (isTooFar && !playerDetected) {
            if (cornerCheck.position.x < xLimitLeft && facingDirection == -1) {
                FlipX();
            }
            if (cornerCheck.position.x > xLimitRight && facingDirection == 1) {
                FlipX();
            }
        }

        if ((!isNearCorner || isTouchingWall) && !isTooFar) {
            FlipX();
        }
    }

    private void Damage(float[] attackDetails) {
        currentHealth -= attackDetails[0];
        healthBar.SetHealth(currentHealth);
        faceToPlayer = aliveObject.transform.position.x > attackDetails[1] ? -1 : 1;

        if (faceToPlayer != facingDirection) {
            FlipX();
        }

        Instantiate(hitEffect, aliveObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        animator.SetTrigger("damage");

        if (currentHealth > 0.0f) {
            Knockback();
        } else {
            Die();
        }
    }

    private void Knockback() {
        knockback = true;
        isAttacking = false;
        nextTimeAttack = Time.time + stunTime;
        KnockbackEnd = Time.time + knockbackDuration;
        rbAlive.velocity = new Vector2(knockbackSpeedX * -facingDirection, knockbackSpeedY);
    }

    private void Die() {
        animator.SetBool("isDead", true);
        aliveObject.layer = LayerMask.NameToLayer("Dead");
    }

    private void FlipX() {
        if (isGrounded && !knockback) {
            rbAlive.velocity = new Vector2(0f, rbAlive.velocity.y);
            nextTimeMove = Time.time + idleTime;
            facingDirection *= -1;
            rbAlive.transform.Rotate(0f, 180f, 0f);
        }
    }

    private void CheckAttackRange() {
        if (!isAttacking && Time.time > nextTimeAttack) {
            isAttacking = Physics2D.OverlapCircle(attackCheck.position, attackRange - 0.3f, knightLayer);
            if (isAttacking) {
                isMoving = false;
                attackLeft = numAttackTime;
                animator.SetTrigger("attack");
            }
        }
    }

    public void finishAttack() {
        Collider2D knight = Physics2D.OverlapCircle(attackCheck.position, attackRange, knightLayer);

        if (knight != null) {
            knight.GetComponent<KnightController>().Damage(damage, damageCheck.position.x);
        }

        attackLeft--;
        if (attackLeft == 0) {
            nextTimeAttack = Time.time + attackCooldown;
            isAttacking = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(attackCheck.position, attackRange);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + groundCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(cornerCheck.position, new Vector3(cornerCheck.position.x, cornerCheck.position.y - groundCheckDistance, cornerCheck.position.z));
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        //Gizmos.DrawLine(damageCheck.position, new Vector3(damageCheck.position.x + playerDetectDistance, damageCheck.position.y, damageCheck.position.z));
    }
}
