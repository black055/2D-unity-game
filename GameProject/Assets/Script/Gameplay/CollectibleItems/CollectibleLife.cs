using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLife : MonoBehaviour
{
  GameManager gameManager;
  SoundManager soundManager;

  void Start()
  {
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    soundManager = SoundManager.instance;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight" && gameManager != null)
    {
      gameManager.ChangeLife(1);
      soundManager.PlaySound("PickupLife");
      Destroy(gameObject);
    }
  }
}
