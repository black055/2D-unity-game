using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLife : MonoBehaviour
{
  [SerializeField]
  private AudioClip pickUpClip;
  GameManager gameManager;

  void Start()
  {
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight" && gameManager != null)
    {
      gameManager.ChangeLife(1);
      if (pickUpClip)
        AudioSource.PlayClipAtPoint(pickUpClip, other.transform.position);
      Destroy(gameObject);
    }
  }
}
