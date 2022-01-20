using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDoubleDamage : MonoBehaviour
{
  [SerializeField]
  private AudioClip pickUpClip;
  [SerializeField]
  private float duration = 30f;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight")
    {
      CombatController knightCombatController = other.GetComponent<CombatController>();
      knightCombatController.CollectDoubleDamage(duration);
      if (pickUpClip)
        AudioSource.PlayClipAtPoint(pickUpClip, other.transform.position);
      Destroy(gameObject);
    }
  }
}
