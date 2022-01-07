using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelper : MonoBehaviour
{
    BasicEnemyController basicController;
    EliteEnemyController eliteController;
    RangeEnemyController rangeController;

    private void Start() {
        basicController = transform.parent.gameObject.GetComponent<BasicEnemyController>();
        eliteController = transform.parent.gameObject.GetComponent<EliteEnemyController>();
        rangeController = transform.parent.gameObject.GetComponent<RangeEnemyController>();
    }

    private void finishAttack() {
        if (basicController != null) basicController.finishAttack();
        if (eliteController != null) eliteController.finishAttack();
        if (rangeController != null) rangeController.finishAttack();
    }
}
