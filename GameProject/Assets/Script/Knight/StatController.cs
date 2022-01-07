using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, maxStamina, staminaRegenPerSec;
    [SerializeField]
    private StaminaBar staminaBar;
    [SerializeField]
    private HealthBar healthBar;

    SpawnManager spawnManager;
    KnightController knightController;
    Animator animator;

    private float currentStamina, currentHealth;

    void Start()
    {
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
        knightController = GetComponent<KnightController>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        staminaBar.SetMaxStamina(maxStamina);
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (currentStamina < maxStamina && knightController.CheckCanRegenStamina()) {
            currentStamina += Time.deltaTime * staminaRegenPerSec;
        }
        staminaBar.SetStamina(currentStamina);
        healthBar.SetHealth(currentHealth);
    }

    // return true if enough and false if not
    public bool UseStamina(float amount) {
        if (currentStamina >= amount) {
            currentStamina -= amount;
            return true;
        }
        else return false;
    }

    public bool RecoverStamina(float amount) {
        if (currentStamina <= maxStamina) {
            currentStamina += amount;
            return true;
        }
        return false;
    }

    public void ChangeHealth(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth <= 0f) {
            animator.SetBool("IsDead", true);
            gameObject.layer = LayerMask.NameToLayer("Dead");
            Destroy(gameObject, 5f);
            spawnManager.Respawn();
        }
    }

    public bool IsDead() {
        return currentHealth <= 0f;
    }

    public void Checkpoint(Vector3 checkpointPosition) {
        spawnManager.UpdateRespawnPosition(checkpointPosition);
    }
}
