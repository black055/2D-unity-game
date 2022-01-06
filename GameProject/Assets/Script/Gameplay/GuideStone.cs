using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideStone : MonoBehaviour
{
    [SerializeField]
    private float width, height;
    [SerializeField]
    public Vector3 offset;
    [SerializeField]
    private LayerMask knightLayer;
    [SerializeField]
    private Image dialog;
    [SerializeField]
    private Text text;

    private Vector2 botLeftDamagePoint, topRightDamagePoint;
    private bool isReading, isNearPlayer;
    private GameObject interact;

    void Start()
    {
        interact = transform.Find("Interact").gameObject;
        isReading = false;
        botLeftDamagePoint.Set(transform.position.x - (width / 2), transform.position.y - (height / 2));
        topRightDamagePoint.Set(transform.position.x + (width / 2), transform.position.y + (height / 2));
        dialog.transform.position += offset;
    }

    void Update()
    {
        isNearPlayer = Physics2D.OverlapArea(botLeftDamagePoint, topRightDamagePoint, knightLayer);
        interact.SetActive(isNearPlayer && !isReading);

        if (isNearPlayer && Input.GetKeyDown(KeyCode.E)) {
            isReading = !isReading;
        }
        
        dialog.gameObject.SetActive(isNearPlayer && isReading);
        isReading = (!isNearPlayer) ? false : isReading;
    }

    private void OnDrawGizmos()
    {
        // botLeftDamagePoint.Set(transform.position.x - (width / 2), transform.position.y - (height / 2));
        // topRightDamagePoint.Set(transform.position.x + (width / 2), transform.position.y + (height / 2));

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
