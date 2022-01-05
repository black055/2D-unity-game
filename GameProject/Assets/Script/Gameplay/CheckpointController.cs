using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{

    [SerializeField]
    private Transform checkPoint;
    [SerializeField]
    private float width, height;
    [SerializeField]
    private LayerMask knightLayer;

    private bool isChecked;
    private Vector2 botLeftDamagePoint, topRightDamagePoint; 


    void Start()
    {
        isChecked = false;
        botLeftDamagePoint.Set(checkPoint.position.x - (width / 2), checkPoint.position.y - (height / 2));
        topRightDamagePoint.Set(checkPoint.position.x + (width / 2), checkPoint.position.y + (height / 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChecked) {
            Collider2D knight = Physics2D.OverlapArea(botLeftDamagePoint, topRightDamagePoint, knightLayer);

            if (knight != null) {
                isChecked = true;
                knight.GetComponent<StatController>().Checkpoint(checkPoint.position);
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        // botLeftDamagePoint.Set(checkPoint.position.x - (width / 2), checkPoint.position.y - (height / 2));
        // topRightDamagePoint.Set(checkPoint.position.x + (width / 2), checkPoint.position.y + (height / 2));

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
