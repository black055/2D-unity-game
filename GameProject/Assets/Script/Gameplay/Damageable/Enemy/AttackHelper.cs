using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelper : MonoBehaviour
{
    BasicEnemyController basicController;
    EliteEnemyController eliteController;

    private void Start() {
        basicController = transform.parent.gameObject.GetComponent<BasicEnemyController>();
        eliteController = transform.parent.gameObject.GetComponent<EliteEnemyController>();
    }

    private void finishAttack() {
        if (basicController != null) basicController.finishAttack();
        if (eliteController != null) eliteController.finishAttack();
    }
}
