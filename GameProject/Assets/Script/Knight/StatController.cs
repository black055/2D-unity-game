using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, maxStamina, staminaRegenPerSec;
    public StaminaBar staminaBar;

    KnightController knightController;

    private float currentStamina, currentHealth;

    void Start()
    {
        knightController = GetComponent<KnightController>();
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        staminaBar.SetMaxStamina(maxStamina);
    }

    void Update()
    {
        if (currentStamina < maxStamina && knightController.CheckCanRegenStamina()) {
            currentStamina += Time.deltaTime * staminaRegenPerSec;
        }
        staminaBar.SetStamina(currentStamina);

        if (Input.GetKeyDown(KeyCode.R)) { currentStamina -= 20f; }
    }

    // return true if enough and false if not
    public bool UseStamina(float amount) {
        if (currentStamina >= amount) {
            currentStamina -= amount;
            return true;
        }
        else return false;
    }
}
