using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelper : MonoBehaviour
{
    BasicEnemyController basicController;
    EliteEnemyController eliteController;
    RangeEnemyController rangeController;
    FlyingEnemyController flyingController;

    private void Start() {
        basicController = transform.parent.gameObject.GetComponent<BasicEnemyController>();
        eliteController = transform.parent.gameObject.GetComponent<EliteEnemyController>();
        rangeController = transform.parent.gameObject.GetComponent<RangeEnemyController>();
        flyingController = transform.parent.gameObject.GetComponent<FlyingEnemyController>();
    }

    private void finishAttack() {
        if (basicController != null) basicController.finishAttack();
        if (eliteController != null) eliteController.finishAttack();
        if (rangeController != null) rangeController.finishAttack();
        if (flyingController != null) flyingController.finishAttack();
    }
}
