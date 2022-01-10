using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemyController : MonoBehaviour
{
  [SerializeField]
  private float maxHealth, moveSpeed, damage;
  [SerializeField]
  private float knockbackSpeedX, knockbackSpeedY, knockbackDuration;
  [SerializeField]
  private GameObject hitEffect;
  [SerializeField]
  private float groundCheckDistance, attackRange, attackCooldown, stunTime, maxFollowDistance;
  [SerializeField]
  private int numAttackTime;
  [SerializeField]
  private EnemyHealthBar healthBar;
  [SerializeField]
  private float nextWaypointDist = 3f;

  private float nextTimeAttack = 0f;
  private float currentHealth;
  private float KnockbackEnd;
  private int attackLeft;
  private int currentWaypoint;

  private bool knockback;
  private bool isNearCorner;
  private bool isTouchingWall;
  private bool isGrounded;
  private bool isAttacking;
  private bool isChasing;
  private bool isReturning;

  [SerializeField]
  private Transform wallCheck, groundCheck, cornerCheck, damageCheck, attackCheck, startingPoint, target;
  [SerializeField]
  private LayerMask groundLayer, knightLayer;

  Path path;
  Seeker seeker;

  private KnightController knightController;
  private GameObject aliveObject;
  private Rigidbody2D rbAlive;
  private Animator animator;

  private void Start()
  {
    currentHealth = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
    knightController = GameObject.Find("Knight").GetComponent<KnightController>();
    aliveObject = transform.Find("Alive").gameObject;
    rbAlive = aliveObject.GetComponent<Rigidbody2D>();
    animator = aliveObject.GetComponent<Animator>();
    seeker = gameObject.GetComponent<Seeker>();
  }

  private void Update()
  {
    if (currentHealth > 0)
    {
      if (knockback && Time.time > KnockbackEnd)
      {
        knockback = false;
        rbAlive.velocity = new Vector2(knockbackSpeedX, knockbackSpeedY);
      }

      Chase();
      Patrol();

      CheckSurroundings();
      CheckAttackRange();
      FlipTransform();
    }
  }

  private void OnPathComplete(Path p)
  {
    if (!p.error)
    {
      path = p;
      currentWaypoint = 0;
    }
  }

  private void Patrol()
  {
    if (path == null) return;

    if (currentWaypoint >= path.vectorPath.Count)
      return;

    if (isChasing)
    {
      animator.SetBool("isMoving", true);
      animator.SetFloat("moveX", knightController.transform.position.x - rbAlive.position.x);
      animator.SetFloat("moveY", knightController.transform.position.y - rbAlive.position.y);
    }
    else if (isReturning)
    {
      animator.SetBool("isMoving", true);
      animator.SetFloat("moveX", startingPoint.position.x - rbAlive.position.x);
      animator.SetFloat("moveY", startingPoint.position.y - rbAlive.position.y);
    }

    rbAlive.transform.position = Vector2.MoveTowards(rbAlive.transform.position, path.vectorPath[currentWaypoint], moveSpeed * Time.deltaTime);

    float distance = Vector2.Distance(rbAlive.position, path.vectorPath[currentWaypoint]);

    if (distance < nextWaypointDist) currentWaypoint++;
  }

  private void StartDrawChasingPath()
  {
    int onLeftSide = aliveObject.transform.position.x > knightController.transform.position.x ? -1 : 1;
    Vector3 relativeCloseToKnightPos = knightController.transform.position + .6f * (onLeftSide * Vector3.left) + .3f * (Vector3.up);
    if (seeker.IsDone())
      seeker.StartPath(rbAlive.position, relativeCloseToKnightPos, OnPathComplete);
  }

  private void StartDrawReturnPath()
  {
    if (seeker.IsDone())
      seeker.StartPath(rbAlive.position, startingPoint.position, OnPathComplete);
  }

  private void Chase()
  {
    // Return back to the start spot
    if (knightController && (isReturning || Vector2.Distance(aliveObject.transform.position, knightController.transform.position) > maxFollowDistance))
    { isReturning = true; ReturnStartPoint(); return; }

    if (!isChasing) return;

    Invoke("StartDrawChasingPath", .5f);
  }

  private void ReturnStartPoint()
  {
    if (Vector2.Distance(aliveObject.transform.position, startingPoint.position) <= .5f)
    {
      // update animation when landing the start spot
      animator.SetBool("isMoving", false);
      isReturning = false;
    }

    Invoke("StartDrawReturnPath", .5f);
    isChasing = false;
  }

  private void ContinueChasing()
  {
    isChasing = true;
  }

  public void setChasing(bool chase)
  {
    isChasing = chase;
  }

  private void FlipTransform()
  {
    if (knightController == null) return;
    bool isOnLeft = aliveObject.transform.position.x > knightController.transform.position.x;

    if ((attackCheck.localPosition.x < 0 && isOnLeft) || (attackCheck.localPosition.x > 0 && !isOnLeft)) return;

    // wallCheck, groundCheck, cornerCheck, damageCheck, attackCheck
    Vector3 newWallCheck = wallCheck.localPosition;
    Vector3 newGroundCheck = groundCheck.localPosition;
    Vector3 newCornerCheck = cornerCheck.localPosition;
    Vector3 newDamageCheck = damageCheck.localPosition;
    Vector3 newAttackCheck = attackCheck.localPosition;


    newWallCheck.x *= -1;
    newGroundCheck.x *= -1;
    newCornerCheck.x *= -1;
    newDamageCheck.x *= -1;
    newAttackCheck.x *= -1;

    wallCheck.localPosition = newWallCheck;
    groundCheck.localPosition = newGroundCheck;
    cornerCheck.localPosition = newCornerCheck;
    damageCheck.localPosition = newDamageCheck;
    attackCheck.localPosition = newAttackCheck;
  }

  private void CheckSurroundings()
  {
    isNearCorner = Physics2D.Raycast(cornerCheck.position, aliveObject.transform.up * -1f, groundCheckDistance, groundLayer);
    isTouchingWall = Physics2D.Raycast(wallCheck.position, aliveObject.transform.right, groundCheckDistance, groundLayer);
    isGrounded = Physics2D.Raycast(groundCheck.position, aliveObject.transform.up * -1f, groundCheckDistance, groundLayer);
  }


  private void Damage(float[] attackDetails)
  {
    currentHealth -= attackDetails[0];
    healthBar.SetHealth(currentHealth);

    Instantiate(hitEffect, aliveObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

    bool isOnLeft = aliveObject.transform.position.x > knightController.transform.position.x;
    animator.SetTrigger("hurt" + (isOnLeft ? "Left" : "Right"));

    if (currentHealth > 0.0f)
    {
      Knockback();
    }
    else
    {
      Die();
    }
  }

  private void Knockback()
  {
    knockback = true;
    isAttacking = false;
    isChasing = false;
    nextTimeAttack = Time.time + stunTime;
    KnockbackEnd = Time.time + knockbackDuration;
    Invoke("ContinueChasing", knockbackDuration);
  }

  private void Die()
  {
    animator.SetBool("isDead", true);
    aliveObject.layer = LayerMask.NameToLayer("Dead");
    rbAlive.gravityScale = 8;
  }

  private void CheckAttackRange()
  {
    if (!isAttacking && Time.time > nextTimeAttack)
    {
      isAttacking = Physics2D.OverlapCircle(attackCheck.position, attackRange - .0f, knightLayer);
      if (isAttacking)
      {
        attackLeft = numAttackTime;

        bool isOnLeft = aliveObject.transform.position.x > knightController.transform.position.x;
        animator.SetTrigger("attack" + (isOnLeft ? "Left" : "Right"));
      }
    }
  }

  public void finishAttack()
  {
    Collider2D knight = Physics2D.OverlapCircle(attackCheck.position, attackRange, knightLayer);

    if (knight != null)
    {
      knight.GetComponent<KnightController>().Damage(damage, damageCheck.position.x);
    }

    attackLeft--;
    if (attackLeft == 0)
    {
      nextTimeAttack = Time.time + attackCooldown;
      Debug.Log(nextTimeAttack);
      isAttacking = false;
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(attackCheck.position, attackRange);
    Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + groundCheckDistance, wallCheck.position.y, wallCheck.position.z));
    Gizmos.DrawLine(cornerCheck.position, new Vector3(cornerCheck.position.x, cornerCheck.position.y - groundCheckDistance, cornerCheck.position.z));
    Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);

  }
}
