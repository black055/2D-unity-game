using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{

    Animator animator;

    KnightController knightController;
    private bool isAttacking;
    private bool secondAttacking;

    [SerializeField]
    private float attackDamage, attackRange;

    public Transform attackCheck;

    public LayerMask damageableLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        knightController = GetComponent<KnightController>();
        isAttacking = false;
        secondAttacking = false;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Q) && knightController.GetState("isGrounded")) {
            if (!isAttacking) {
                isAttacking = true;
                animator.SetBool("isAttacking", isAttacking);
            } else if (secondAttacking == false) {
                secondAttacking = true;
                animator.SetBool("secondAttacking", secondAttacking);
            }
            
        }
    }

    private void firstAttackFinished() {
        if (!secondAttacking)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking); 
        }
    }

    private void secondAttackFinished() {
        isAttacking = false;
        secondAttacking = false;
        animator.SetBool("isAttacking", isAttacking); 
        animator.SetBool("secondAttacking", secondAttacking);
    }

    private void enemyDetect() {
        Collider2D[] hitedEnemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRange, damageableLayer);

        foreach (Collider2D enemy in hitedEnemies) {
            enemy.transform.parent.SendMessage("Damage", attackDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackRange);
    }

    public bool GetAttacking() {
        return (isAttacking || secondAttacking);
    }
}
