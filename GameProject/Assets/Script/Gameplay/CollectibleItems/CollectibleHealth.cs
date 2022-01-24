using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
  [SerializeField]
  private float amountOfHealthRecovered;
  SoundManager soundManager;

  void Start() {
    soundManager = SoundManager.instance;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight")
    {
      StatController knightStatController = other.GetComponent<StatController>();
      if (knightStatController.IsFullHealth())
        return;
      knightStatController.ChangeHealth(amountOfHealthRecovered);
      soundManager.PlaySound("PickupHealth");
      Destroy(gameObject);
    }
  }
}
