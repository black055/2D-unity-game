using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleStamina : MonoBehaviour
{
  [SerializeField]
  private float amountOfStaminaRecovered;
  SoundManager soundManager;

  void Start()
  {
    soundManager = SoundManager.instance;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight")
    {
      StatController knightStatController = other.GetComponent<StatController>();
      if (knightStatController.RecoverStamina(amountOfStaminaRecovered))
      {
        soundManager.PlaySound("PickupStamina");
        Destroy(gameObject);
      }
    }
  }
}
