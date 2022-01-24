using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDoubleDamage : MonoBehaviour
{
  [SerializeField]
  private float duration = 30f;
  SoundManager soundManager;

  void Start() {
    soundManager = SoundManager.instance;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight")
    {
      CombatController knightCombatController = other.GetComponent<CombatController>();
      knightCombatController.CollectDoubleDamage(duration);
      soundManager.PlaySound("PickupDamage");
      Destroy(gameObject);
    }
  }
}
