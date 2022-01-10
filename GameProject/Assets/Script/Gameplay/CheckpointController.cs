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
    private GameObject fire, glow, spark;
    private SoundManager soundManager;

    void Start()
    {
        isChecked = false;
        fire = transform.Find("Fire").gameObject;
        glow = transform.Find("Glow").gameObject;
        spark = transform.Find("Spark").gameObject;
        soundManager = SoundManager.instance;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!isChecked && (((1<<collider.gameObject.layer) & knightLayer) != 0)) {
            // If player walk in
            isChecked = true;
            collider.gameObject.GetComponent<StatController>().Checkpoint(checkPoint.position);
            fire.SetActive(true);
            glow.SetActive(true);
            spark.SetActive(true);
            soundManager.PlaySound("Checkpoint");
        }
    }
}
