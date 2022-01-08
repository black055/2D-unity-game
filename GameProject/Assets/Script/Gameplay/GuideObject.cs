using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideObject : MonoBehaviour
{
    [SerializeField]
    public Vector3 offset;
    [SerializeField]
    private LayerMask knightLayer;
    [SerializeField]
    private Image dialog;

    private bool isReading, isNearPlayer;
    private GameObject interact;

    private void Start()
    {
        interact = transform.Find("Interact").gameObject;
        isReading = false;
        dialog.transform.position += offset;
    }

    void Update()
    {
        if (isNearPlayer && Input.GetKeyDown(KeyCode.E)) {
            isReading = !isReading;
            interact.SetActive(isNearPlayer && !isReading);
            dialog.gameObject.SetActive(isNearPlayer && isReading);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (((1<<collider.gameObject.layer) & knightLayer) != 0) {
            // If player walk in
            isNearPlayer = true;
            interact.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (((1<<collider.gameObject.layer) & knightLayer) != 0) {
            // If player walk in
            isNearPlayer = false;
            isReading = false;
            interact.SetActive(false);
            dialog.gameObject.SetActive(false);
        }
    }
}
