using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask knightLayer;

    private GameObject interact;
    private bool isNearPlayer;

    // Start is called before the first frame update
    void Start()
    {
        interact = transform.Find("Interact").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNearPlayer && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("finish");
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
            interact.SetActive(false);
        }
    }
}
