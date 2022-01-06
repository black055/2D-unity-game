using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask knightLayer;
    [SerializeField]
    private float width, height;
    [SerializeField]
    private Transform checkPositon;

    private GameObject interact;
    private Vector2 botLeftDamagePoint, topRightDamagePoint;
    private bool isNearPlayer;

    // Start is called before the first frame update
    void Start()
    {
        interact = transform.Find("Interact").gameObject;
        botLeftDamagePoint.Set(checkPositon.transform.position.x - (width / 2), checkPositon.transform.position.y - (height / 2));
        topRightDamagePoint.Set(checkPositon.transform.position.x + (width / 2), checkPositon.transform.position.y + (height / 2));
    }

    // Update is called once per frame
    void Update()
    {
        isNearPlayer = Physics2D.OverlapArea(botLeftDamagePoint, topRightDamagePoint, knightLayer);
        interact.SetActive(isNearPlayer);

        if (isNearPlayer && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("finish");
        }
    }

    private void OnDrawGizmos()
    {
        botLeftDamagePoint.Set(checkPositon.transform.position.x - (width / 2), checkPositon.transform.position.y - (height / 2));
        topRightDamagePoint.Set(checkPositon.transform.position.x + (width / 2), checkPositon.transform.position.y + (height / 2));

        Vector2 botleft = new Vector2(botLeftDamagePoint.x, botLeftDamagePoint.y);
        Vector2 botright = new Vector2(topRightDamagePoint.x, botLeftDamagePoint.y);
        Vector2 topleft = new Vector2(botLeftDamagePoint.x, topRightDamagePoint.y);
        Vector2 topright = new Vector2(topRightDamagePoint.x, topRightDamagePoint.y);

        Gizmos.DrawLine(botleft, botright);
        Gizmos.DrawLine(topleft, topright);
        Gizmos.DrawLine(botleft, topleft);
        Gizmos.DrawLine(topright, botright);
    }
}
