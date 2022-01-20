using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
  [SerializeField]
  private float amountOfHealthRecovered;
  [SerializeField]
  private AudioClip pickUpClip;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight")
    {
      StatController knightStatController = other.GetComponent<StatController>();
      if (knightStatController.IsFullHealth())
        return;
      knightStatController.ChangeHealth(amountOfHealthRecovered);
      if (pickUpClip)
        AudioSource.PlayClipAtPoint(pickUpClip, other.transform.position);
      Destroy(gameObject);
    }
  }
}
