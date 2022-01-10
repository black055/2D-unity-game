using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    [SerializeField]
    public Vector3 offset;
    [SerializeField]
    private LayerMask knightLayer;

    private bool isNearPlayer, isRescued;
    private GameObject interact, chatbox;
    private Animator animator;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        interact = transform.Find("Interact").gameObject;
        chatbox = transform.Find("ChatBox").gameObject;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isRescued && isNearPlayer && Input.GetKeyDown(KeyCode.E)) {
            animator.SetTrigger("Rescued");
            isRescued = true;
            interact.SetActive(false);
            chatbox.SetActive(true);
            gameManager.RescuseVillager();
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!isRescued && ((1<<collider.gameObject.layer) & knightLayer) != 0) {
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
