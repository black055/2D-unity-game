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
  private int facingDirection;
  private float targetPositionX;
  private int attackLeft;
  private int currentWaypoint;

  private bool knockback;
  private bool isAttacking;
  private bool isChasing;
  private bool isReturning;

  [SerializeField]
  private Transform damageCheck, attackCheck, startingPoint;
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
    facingDirection = 1;
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
      if (knightController != null)
      {
        if (knightController.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
          isReturning = true;
          // 1 sec delay
          Invoke("FindNewTarget", 5f + 1f);
        }
      }

      Chase();
      Patrol();
      FlipX();

      CheckAttackRange();
    }
  }

  private void FindNewTarget()
  {
    knightController = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightController>();
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

    if (isChasing || isReturning)
      animator.SetBool("isMoving", true);

    rbAlive.transform.position = Vector2.MoveTowards(rbAlive.transform.position, path.vectorPath[currentWaypoint], moveSpeed * Time.deltaTime);

    float distance = Vector2.Distance(rbAlive.position, path.vectorPath[currentWaypoint]);

    if (distance < nextWaypointDist) currentWaypoint++;
  }

  private void StartDrawChasingPath()
  {
    if (knightController == null) return;
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
    if (knightController == null) return;
    // Return back to the start spot
    if (isReturning || Vector2.Distance(aliveObject.transform.position, knightController.transform.position) > maxFollowDistance)
    {
      isReturning = true;
      ReturnStartPoint();
      return;
    }

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

  private void FlipX()
  {
    if (knightController == null) return;

    if (isChasing)
      targetPositionX = knightController.transform.position.x;
    else if (isReturning)
      targetPositionX = startingPoint.transform.position.x;
    else return;

    bool isOnLeft = aliveObject.transform.position.x > targetPositionX;
    if ((facingDirection == -1 && isOnLeft) || (facingDirection == 1 && !isOnLeft)) return;


    facingDirection *= -1;
    rbAlive.transform.Rotate(0f, 180f, 0f);
  }

  private void Damage(float[] attackDetails)
  {
    currentHealth -= attackDetails[0];
    healthBar.SetHealth(currentHealth);

    Instantiate(hitEffect, aliveObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

    animator.SetTrigger("hurt");

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
        animator.SetTrigger("attack");
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
      isAttacking = false;
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(attackCheck.position, attackRange);

  }
}
