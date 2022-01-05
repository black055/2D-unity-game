using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float knockbackSpeedX, knockbackSpeedY, knockbackDuration;
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private EnemyHealthBar healthBar;

    private float currentHealth;
    private float KnockbackEnd;
    private int knightFacingDirection;

    private bool knockback;

    private KnightController kc;
    private GameObject aliveObject, dummyTopObject, dummyBotObject;
    private Rigidbody2D rbAlive, rbTop, rbBot;
    private Animator animator;

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        kc = GameObject.Find("Knight").GetComponent<KnightController>();

        aliveObject = transform.Find("Alive").gameObject;
        dummyTopObject = transform.Find("DummyTop").gameObject;
        dummyBotObject = transform.Find("DummyBottom").gameObject;

        rbAlive = aliveObject.GetComponent<Rigidbody2D>();
        rbTop = dummyTopObject.GetComponent<Rigidbody2D>();
        rbBot = dummyBotObject.GetComponent<Rigidbody2D>();

        animator = aliveObject.GetComponent<Animator>();

        aliveObject.SetActive(true);
        dummyTopObject.SetActive(false);
        dummyBotObject.SetActive(false);
    }

    private void Update() {
        if (Time.time > KnockbackEnd && knockback) {
            knockback = false;
            rbAlive.velocity = new Vector2(0f, knockbackSpeedY);

        }
    }

    private void Damage(float[] attackDetails) {
        currentHealth -= attackDetails[0];
        healthBar.SetHealth(currentHealth);
        knightFacingDirection = (attackDetails[1] < aliveObject.transform.position.x) ? 1 : -1;
        Instantiate(hitEffect, aliveObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        animator.SetBool("playerOnLeft", attackDetails[1] < aliveObject.transform.position.x);
        animator.SetTrigger("damage");

        if (currentHealth > 0.0f) {
            Knockback();
        } else {
            Die();
        }
    }

    private void Knockback() {
        knockback = true;
        KnockbackEnd = Time.time + knockbackDuration;
        rbAlive.velocity = new Vector2(knockbackSpeedX * knightFacingDirection, knockbackSpeedY);
    }

    private void Die() {
        aliveObject.SetActive(false);
        dummyTopObject.SetActive(true);
        dummyBotObject.SetActive(true);

        dummyTopObject.transform.position = aliveObject.transform.position;
        dummyBotObject.transform.position = aliveObject.transform.position;

        rbTop.velocity = new Vector2(knockbackSpeedX * knightFacingDirection, knockbackSpeedY);
        rbBot.velocity = new Vector2(knockbackSpeedX * knightFacingDirection, knockbackSpeedY);
    }
}
