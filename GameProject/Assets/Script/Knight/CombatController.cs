using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{

    Animator animator;
    SoundManager soundManager;
    KnightController knightController;
    private bool isAttacking;
    private bool secondAttacking;
    private int attackWeight;

    [SerializeField]
    private GameObject buffEffect;
    [SerializeField]
    private float attackDamage, attackRange;
    [SerializeField]
    private Transform attackCheck;
    [SerializeField]
    private LayerMask damageableLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        knightController = GetComponent<KnightController>();
        soundManager = SoundManager.instance;
        isAttacking = false;
        secondAttacking = false;
        attackWeight = 1;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Q) && knightController.GetState("isGrounded")) {
            if (!isAttacking) {
                isAttacking = true;
                if (knightController.GetState("isCrouching")) secondAttacking = true;
                animator.SetBool("isAttacking", isAttacking);
            } else if (secondAttacking == false) {
                secondAttacking = true;
                animator.SetBool("secondAttacking", secondAttacking);
            }
        }
    }

    private void firstAttackSound() {
        soundManager.PlaySound("KnightAttack1");
    }

    private void secondAttackSound() {
        soundManager.PlaySound("KnightAttack2");
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

        float[] attackDetails = new float[2];
        attackDetails[0] = attackDamage * attackWeight;
        attackDetails[1] = transform.position.x;

        foreach (Collider2D enemy in hitedEnemies) {
            soundManager.PlaySound("HitEnemy");
            enemy.transform.parent.SendMessage("Damage", attackDetails);
        }
    }

    public void StopAttack() {
        secondAttackFinished();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackRange);
    }

    public bool GetAttacking() {
        return (isAttacking || secondAttacking);
    }

    public void CollectDoubleDamage(float duration) {
        attackWeight = 2;
        buffEffect.SetActive(true);
        Invoke("DoubleDamageEnd", duration);
    }

    private void DoubleDamageEnd() {
        attackWeight = 1;
        buffEffect.SetActive(false);
    }
}
