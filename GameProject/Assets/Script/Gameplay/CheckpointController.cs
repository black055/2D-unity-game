using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField]
    private LayerMask knightLayer;
    [SerializeField]
    private Transform checkPoint;

    private bool isChecked;

    void Start()
    {
        isChecked = false;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!isChecked && (((1<<collider.gameObject.layer) & knightLayer) != 0)) {
            // If player walk in
            isChecked = true;
            collider.gameObject.GetComponent<StatController>().Checkpoint(checkPoint.position);
            Debug.Log("New Respawn Position");
        }
    }
}
