using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask knightLayer;

    private GameObject interact;
    private bool isNearPlayer;
    private GameManager gameManager;
    [SerializeField] GameObject finishScreen;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        interact = transform.Find("Interact").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNearPlayer && gameManager.IsFinishedStage() && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("finish");
            finishScreen.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (gameManager.IsFinishedStage() && ((1<<collider.gameObject.layer) & knightLayer) != 0) {
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
