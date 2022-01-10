using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, moveSpeed, damage, arrowSpeed;
    [SerializeField]
    private float knockbackSpeedX, knockbackSpeedY, knockbackDuration;
    [SerializeField]
    private GameObject hitEffect, bloodEffect;
    [SerializeField]
    private float groundCheckDistance, playerDetectRange, attackRange, attackCooldown, stunTime, idleTime, moveDistance;
    [SerializeField]
    private EnemyHealthBar healthBar;
    [SerializeField]
    private GameObject arrowObject;

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
    private SoundManager soundManager;

    private void Start() {
        soundManager = SoundManager.instance;
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

        if (isTooFar) {
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

        animator.SetTrigger("damage");

        if (currentHealth > 0.0f) {
            Instantiate(hitEffect, aliveObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            Knockback();
        } else {
            Instantiate(bloodEffect, aliveObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
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
        soundManager.PlaySound("EnemyDead");
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
            isAttacking = Physics2D.Raycast(attackCheck.position, aliveObject.transform.right, playerDetectRange, knightLayer);
            if (isAttacking) {
                isMoving = false;
                animator.SetTrigger("attack");
            }
        }
    }

    public void finishAttack() {
        isAttacking = false;
        nextTimeAttack = Time.time + attackCooldown;
        nextTimeMove = nextTimeAttack + 0.5f;
        GameObject arrow = GameObject.Instantiate(arrowObject, attackCheck.position, rbAlive.transform.rotation);
        arrow.GetComponent<ProjectileController>().Launch(damage, arrowSpeed, attackRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + groundCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(cornerCheck.position, new Vector3(cornerCheck.position.x, cornerCheck.position.y - groundCheckDistance, cornerCheck.position.z));
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        Gizmos.DrawLine(attackCheck.position, new Vector3(attackCheck.position.x + attackRange, attackCheck.position.y, attackCheck.position.z));

    }
}
