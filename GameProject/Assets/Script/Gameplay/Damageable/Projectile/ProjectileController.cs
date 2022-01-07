using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float gravity, damageRadius;
    [SerializeField]
    private LayerMask groundLayer, knightLayer;
    [SerializeField]
    private Transform damagePosition;

    private float attackRange, speed, damage;

    private bool isOutOfRange, hitedGround;
    private float startPosition;
    Rigidbody2D rb2d;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.velocity = transform.right * speed;
        rb2d.gravityScale = 0f;

        startPosition = transform.position.x;
    }

    private void FixedUpdate() {
        if (!hitedGround) {
            Collider2D hitGround = Physics2D.OverlapCircle(damagePosition.position, damageRadius, groundLayer);
            Collider2D hitKnight = Physics2D.OverlapCircle(damagePosition.position, damageRadius, knightLayer);

            if (Mathf.Abs(startPosition - transform.position.x) >= attackRange && !isOutOfRange) {
                isOutOfRange = true;
                rb2d.gravityScale = gravity;
            }

            if (isOutOfRange) {
                 float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
                 transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if (hitKnight) {
                hitKnight.GetComponent<KnightController>().Damage(damage, damagePosition.position.x);
                Destroy(gameObject);
            }

            if (hitGround) {
                hitedGround = true;
                rb2d.gravityScale = 0f;
                rb2d.velocity = Vector2.zero;
                Destroy(gameObject, 5f);
            }
        }
    }

    public void Launch(float damage, float speed, float attackRange) {
        this.attackRange = attackRange;
        this.speed = speed;
        this.damage = damage;
    }
}
