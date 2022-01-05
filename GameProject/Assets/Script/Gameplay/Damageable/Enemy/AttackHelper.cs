using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelper : MonoBehaviour
{
    BasicEnemyController controller;

    private void Start() {
        controller = transform.parent.gameObject.GetComponent<BasicEnemyController>();
    }

    private void finishAttack() {
        controller.finishAttack();
    }
}
