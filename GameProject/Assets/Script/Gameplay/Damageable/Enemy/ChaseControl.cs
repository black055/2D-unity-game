using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseControl : MonoBehaviour
{
  [SerializeField]
  FlyingEnemyController[] flyingMobs;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Knight")
    {
      foreach (FlyingEnemyController mob in flyingMobs)
      {
        mob.setChasing(true);
      }
    }
  }

  // private void OnTriggerExit2D(Collider2D other)
  // {
  //   if (other.tag == "Knight")
  //   {
  //     foreach (FlyingEnemyController mob in flyingMobs)
  //     {
  //       mob.setChasing(false);
  //     }
  //   }
  // }
}
