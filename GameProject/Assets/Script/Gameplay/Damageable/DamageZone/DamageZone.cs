using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField]
    private Transform damagePoint;
    [SerializeField]
    private float width, height, damage;
    [SerializeField]
    private LayerMask knightLayer;

    private Vector2 botLeftDamagePoint, topRightDamagePoint; 

    void Start()
    {
        botLeftDamagePoint.Set(damagePoint.position.x - (width / 2), damagePoint.position.y - (height / 2));
        topRightDamagePoint.Set(damagePoint.position.x + (width / 2), damagePoint.position.y + (height / 2));
    }

    void Update()
    {
        Collider2D knight = Physics2D.OverlapArea(botLeftDamagePoint, topRightDamagePoint, knightLayer);

        if (knight != null) {
            knight.GetComponent<KnightController>().Damage(damage, damagePoint.position.x);
        }
    }

    private void OnDrawGizmos()
    {
        // botLeftDamagePoint.Set(damagePoint.position.x - (width / 2), damagePoint.position.y - (height / 2));
        // topRightDamagePoint.Set(damagePoint.position.x + (width / 2), damagePoint.position.y + (height / 2));

        // Vector2 botleft = new Vector2(botLeftDamagePoint.x, botLeftDamagePoint.y);
        // Vector2 botright = new Vector2(topRightDamagePoint.x, botLeftDamagePoint.y);
        // Vector2 topleft = new Vector2(botLeftDamagePoint.x, topRightDamagePoint.y);
        // Vector2 topright = new Vector2(topRightDamagePoint.x, topRightDamagePoint.y);

        // Gizmos.DrawLine(botleft, botright);
        // Gizmos.DrawLine(topleft, topright);
        // Gizmos.DrawLine(botleft, topleft);
        // Gizmos.DrawLine(topright, botright);
    }
}
