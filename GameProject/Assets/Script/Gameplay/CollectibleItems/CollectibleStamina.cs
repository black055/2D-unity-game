using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleStamina : MonoBehaviour
{
  [SerializeField]
  private float amountOfStaminaRecovered;
  [SerializeField]
  private AudioClip pickUpClip;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight")
    {
      StatController knightStatController = other.GetComponent<StatController>();
      if (knightStatController.RecoverStamina(amountOfStaminaRecovered))
      {
        if (pickUpClip)
          AudioSource.PlayClipAtPoint(pickUpClip, other.transform.position);
        Destroy(gameObject);
      }
    }
  }
}
